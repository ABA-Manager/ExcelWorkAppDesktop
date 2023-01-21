using ClinicDOM;
using ClinicDOM.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGenLib
{
    public class ExcelGenService
    {
        public static ExcelGenService Instance { get; set; }
        private Clinic_AppContext db { get; set; }
        public ExcelGenService(Clinic_AppContext db)
        {
            this.db = db;
        }

        public async Task<List<Company>> GetCompanies()
        {
            var companyQry = from c in db.Company
                             select c;

            return await companyQry.ToListAsync();
            //foreach (var co in companyQry.ToList())
            //    CompanyData.Add(co.Acronym, co.Name);            
        }

        public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
        {
            var periodQry = from p in db.Period
                            where (p.StartDate < DateTime.Now)
                            orderby p.StartDate descending
                            select new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod };

            return await periodQry.ToListAsync();
        }

        public async Task<Period> GetPeriodAsync(int PeriodId = -1)
        {
            var periodQry = (from p in db.Period
                                 //join pp in db.Period on p.StartDate equals DbFunctions.AddDays(pp.EndDate, 1)
                             where (PeriodId == -1 && p.EndDate < DateTime.Now) || (PeriodId != -1 && p.Id == PeriodId)
                             orderby p.StartDate descending
                             select p).Take(1);

            return await periodQry.SingleOrDefaultAsync();
        }

        public async Task<List<ExtendedContractor>> GetExContractorsAsync(string companyCode)
        {
            var contractorsQry = (from ag in db.Agreement
                                  where ag.Payroll.ContractorTypeId == 1 && (companyCode == "" | ag.Company.Acronym == companyCode)
                                  select new ExtendedContractor { contractor = ag.Payroll.Contractor, company = ag.Company }).Distinct();

            return await contractorsQry.ToListAsync();
        }

        public async Task<List<Agreement>> GetAgreementsAsync(int ContractorId, int CompanyId)
        {
            var agreementQry = from ag in db.Agreement
                               where ag.Payroll.ContractorId == ContractorId && ag.CompanyId == CompanyId
                               select ag;

            return await agreementQry.ToListAsync();
        }

        public async Task<List<ExtendedAgrUnitDetail>> GetExAgrUnitDetails(int ClientId, int ContractorId, int CompanyId, int PeriodId)
        {
            var unitDetQry = from ag in db.Agreement
                             join pr in db.Payroll on ag.PayrollId equals pr.Id
                             join sl in db.ServiceLog on new { ag.ClientId, pr.ContractorId } equals new { sl.ClientId, sl.ContractorId }
                             join ud in db.UnitDetail on sl.Id equals ud.ServiceLogId
                             where sl.ClientId == ClientId &&
                               ((ag.Payroll.ContractorTypeId == 1 && sl.ContractorId == ContractorId) ||
                                (ag.Payroll.ContractorTypeId != 1 && !(from inag in db.Agreement where inag.CompanyId == CompanyId && inag.ClientId == ClientId && inag.Payroll.ContractorId < ContractorId && inag.Payroll.ContractorTypeId == 1 select inag).Any())) &&
                               ag.CompanyId == CompanyId &&
                               sl.PeriodId == PeriodId
                             //sl.CreatedDate > DbFunctions.AddDays(previousPeriod.DocumentDeliveryDate, 2) &&
                             //sl.CreatedDate <= DbFunctions.AddDays(period.DocumentDeliveryDate, 2)                                   
                             orderby sl.ClientId, ag.Payroll.ContractorTypeId, sl.ContractorId, ud.SubProcedureId, ud.DateOfService
                             select new ExtendedAgrUnitDetail
                             {
                                 serviceLog = sl,
                                 unitDetail = ud,
                                 agreement = ag
                             };
            return await unitDetQry.ToListAsync();
        }
    }
}
