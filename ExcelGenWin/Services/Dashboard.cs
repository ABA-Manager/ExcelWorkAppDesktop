using ABABillingAndClaim.Models;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public class Dashboard
    {
        private readonly Clinic_AppContext _db;


        public Dashboard(Clinic_AppContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ProfitHistory>> GetProfit(int company_id)
        {
            var sqlQuery = "EXEC GET_PROFIT @company_id";
            var companyParam = new SqlParameter("@company_id", company_id);
            return await _db.Database.SqlQuery<ProfitHistory>(sqlQuery, companyParam).ToListAsync(); ;
        }

        public async Task<ServicesLogStatus> GetServicesLgStatus(int company_id, int period_id)
        {
            var sqlQuery = "EXEC GET_STATUS_SERVICELOGS @company_id, @period_id";
            var companyParam = new SqlParameter("@company_id", company_id);
            var periodParam = new SqlParameter("@period_id", period_id);

            return await _db.Database.SqlQuery<ServicesLogStatus>(sqlQuery, companyParam, periodParam).FirstOrDefaultAsync();
            //return _db.Client.Select(x => new ServicesLogStatus { Billed = 12, NotBilled = 114, Pending=98}).FirstOrDefault();
        }

        public async Task<IEnumerable<ServiceLogWithoutPatientAccount>> GetServiceLogWithoutPatientAccount(int company_id, int period_id)
        {
            var sqlQuery = $"EXEC GET_SERVICELOG_WITHOUT_PATIENTACCOUNT @company_id, @period_id";
            var companyParam = new SqlParameter("@company_id", company_id);
            var periodParam = new SqlParameter("@period_id", period_id);

            return await _db.Database.SqlQuery<ServiceLogWithoutPatientAccount>(sqlQuery, companyParam, periodParam).ToListAsync();
        }

        public async Task<GeneralData> GetGeneralData(int company_id, int period_id)
        {
            return new GeneralData
            {
                Client = await _db.Client.Where(x => x.Agreement.Any(a => a.CompanyId == company_id)).CountAsync(),
                Contractor = await _db.Contractor.Where(x => x.Payroll.Any(p => p.CompanyId == company_id)).CountAsync(),
                ServiceLog = await _db.ServiceLog.Where(x => x.PeriodId == period_id && x.Client.Agreement.Any(a => a.CompanyId == company_id) && x.Contractor.Payroll.Any(p => p.CompanyId == company_id) ).CountAsync()
            };
        }

        public async Task<IEnumerable<Company>> GetCompanies() 
        {
            return await _db.Company.ToListAsync();
        }

        public async Task<IEnumerable<Period>> GetPeriods()
        {
            var periodQry = await _db.Period
                 .Where(p => p.StartDate < DateTime.Now)
                 .OrderByDescending(p => p.StartDate)
                 .ToListAsync();

            return periodQry;
        }
    }
}
