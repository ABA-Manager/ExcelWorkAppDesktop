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
        public IEnumerable<ProfitHistory> GetProfit(int company_id)
        {
            var sqlQuery = "EXEC GET_PROFIT @company_id";
            var companyParam = new SqlParameter("@company_id", company_id);
            return _db.Database.SqlQuery<ProfitHistory>(sqlQuery, companyParam).ToList(); ;
            // return _db.Profit_Dashboard.Where(d=> d.IdCompany==company_id).Select( x=> new 
            //{
            //    Id = x.Id
            //    Billed = x.Billed,
            //    Payment = x.Payment,
            //    PayPeriod = x.PayPeriod,
            //    Profit = x.Profit
            //})
            //    .OrderByDescending(x=> x.Id)
            //    .Take(10)
            //    .ToList()
            //    .OrderBy(x => x.PayPeriod);
        }

        public ServicesLogStatus GetServicesLgStatus(int company_id, int period_id)
        {
            var sqlQuery = "EXEC GET_STATUS_SERVICELOGS @company_id, @period_id";
            var companyParam = new SqlParameter("@company_id", company_id);
            var periodParam = new SqlParameter("@period_id", period_id);

            return _db.Database.SqlQuery<ServicesLogStatus>(sqlQuery, companyParam, periodParam).FirstOrDefault();
            //return _db.Client.Select(x => new ServicesLogStatus { Billed = 12, NotBilled = 114, Pending=98}).FirstOrDefault();
        }

        public IEnumerable<ServiceLogWithoutPatientAccount> GetServiceLogWithoutPatientAccount(int company_id, int period_id)
        {
            var sqlQuery = $"EXEC GET_SERVICELOG_WITHOUT_PATIENTACCOUNT @company_id, @period_id";
            var companyParam = new SqlParameter("@company_id", company_id);
            var periodParam = new SqlParameter("@period_id", period_id);

            return _db.Database.SqlQuery<ServiceLogWithoutPatientAccount>(sqlQuery, companyParam, periodParam).ToList();
        }

        public GeneralData GetGeneralData(int company_id, int period_id)
        {
            return new GeneralData
            {
                Client = _db.Client.Where(x => x.Agreement.Any(a => a.CompanyId == company_id)).Count(),
                Contractor = _db.Contractor.Where(x => x.Payroll.Any(p => p.CompanyId == company_id)).Count(),
                ServiceLog = _db.ServiceLog.Where(x => x.PeriodId == period_id && x.Client.Agreement.Any(a => a.CompanyId == company_id) && x.Contractor.Payroll.Any(p => p.CompanyId == company_id) ).Count()
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
