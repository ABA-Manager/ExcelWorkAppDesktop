using ABABillingAndClaim.Models;
using ClinicDOM;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public class DashboardService
    {
        public static DashboardService Instance { get; private set; }

        private readonly Clinic_AppContext _db;

        public DashboardService(Clinic_AppContext db)
        {
            _db = db;
            DashboardService.Instance = this;
        }
        public IEnumerable<ProfitHistory> GetProfit(int company_id)
        {
            var sqlQuery = $"select \"PayPeriod\", " +
                $"round(cast(sum((\"Unit\") * CASE WHEN sp.\"Id\" = 1 then pro.\"Rate\" ELSE sp.\"Rate\" END) as numeric),2) as Billed, " +
                $"round(cast(sum((\"Unit\"/4) * ag.\"RateEmployees\") as numeric),2) as Payment, " +
                $"round(cast(sum((\"Unit\") * CASE WHEN sp.\"Id\" = 1 then pro.\"Rate\" ELSE sp.\"Rate\" END) as numeric),2) - " +
                $"round(cast(sum((\"Unit\"/4) * ag.\"RateEmployees\") as numeric),2) as Profit " +
                $"from \"Agreement\" ag " +
                $"inner join \"Payroll\" pr on ag.\"PayrollId\" = pr.\"Id\" and pr.\"CompanyId\" = ag.\"CompanyId\" " +
                $"inner join \"Procedure\" pro on pro.\"Id\" = pr.\"ProcedureId\" " +
                $"inner join \"ServiceLog\" sl on sl.\"ClientId\" = ag.\"ClientId\" and pr.\"ContractorId\" = sl.\"ContractorId\" " +
                $"inner join \"Period\" p on p.\"Id\" = sl.\"PeriodId\" " +
                $"inner join \"UnitDetail\" ud on sl.\"Id\" = ud.\"ServiceLogId\" " +
                $"inner join \"SubProcedure\" sp on ud.\"SubProcedureId\" = sp.\"Id\" " +
                $"WHERE ag.\"CompanyId\" = {company_id} " +
                $"group by p.\"Id\", p.\"PayPeriod\" " +
                $"order by p.\"Id\";";
            //var companyParam = new NpgsqlParameter("pcompany_id", company_id);
            return _db.Database.SqlQuery<ProfitHistory>(sqlQuery /*, companyParam*/).ToList();
            //return _db.Profit_Dashboard.Where(x=> x.IdCompany==company_id).Select( x=> new ProfitHistory 
            //{
            //    Billed = x.Billed,
            //    Payment = x.Payment,
            //    PayPeriod = x.PayPeriod,
            //    Profit = x.Profit
            //})
            //    .OrderByDescending(x=> x.PayPeriod)
            //    .Take(10)
            //    .ToList()
            //    .OrderBy(x => x.PayPeriod);
        }

        public ServicesLogStatus GetServicesLgStatus(int company_id, int period_id)
        {
            var sqlQuery = $"SELECT SUM(CASE WHEN \"Biller\" IS NOT NULL THEN 1 ELSE 0 END) AS Billed, " +
                $"SUM(CASE WHEN \"Pending\" IS NOT NULL THEN 1 ELSE 0 END) AS Pending, " +
                $"SUM(CASE WHEN \"Biller\" IS NULL AND \"Pending\" IS NULL THEN 1 ELSE 0 END) AS NotBilled " +
                $"from \"Agreement\" ag " +
                $"inner join \"Payroll\" pr on ag.\"PayrollId\" = pr.\"Id\" and pr.\"CompanyId\" = ag.\"CompanyId\" " +
                $"inner join \"ServiceLog\" sl on sl.\"ClientId\" = ag.\"ClientId\" and pr.\"ContractorId\" = sl.\"ContractorId\" " +
                $"inner join \"Period\" p on p.\"Id\" = sl.\"PeriodId\" " +
                $"WHERE sl.\"PeriodId\" = {period_id} " +
                $"  and  ag.\"CompanyId\" = {company_id}; ";
            //var companyParam = new SqlParameter("@company_id", company_id);
            //var periodParam = new SqlParameter("@period_id", period_id);

            return _db.Database.SqlQuery<ServicesLogStatus>(sqlQuery /*, companyParam, periodParam*/).FirstOrDefault();
            //return _db.Client.Select(x => new ServicesLogStatus { Billed = 12, NotBilled = 114, Pending=98}).FirstOrDefault();
        }

        public IEnumerable<ServiceLogWithoutPatientAccount> GetServiceLogWithoutPatientAccount(int company_id, int period_id)
        {
            var sqlQuery = $"SELECT cl.\"Id\", cl.\"Name\" Client, ct.\"Id\", ct.\"Name\" Contractor, " +
                $"ud.\"DateOfService\", ud.\"SubProcedureId\", sp.\"Name\" \"Procedure\" " +
                $"FROM \"Agreement\" ag " +
                $"INNER JOIN \"Company\" c ON c.\"Id\" = ag.\"CompanyId\" AND c.\"Id\" = {company_id} " +
                $"INNER JOIN \"Client\" cl ON cl.\"Id\" = ag.\"ClientId\" " +
                $"INNER JOIN \"Payroll\" pr ON pr.\"Id\" = ag.\"PayrollId\" " +
                $"INNER JOIN \"Contractor\" ct ON ct.\"Id\" = pr.\"ContractorId\" " +
                $"INNER JOIN \"ServiceLog\" sl ON sl.\"ClientId\" = cl.\"Id\" AND sl.\"ContractorId\" = ct.\"Id\" AND sl.\"PeriodId\"= {period_id} " +
                $"INNER JOIN \"UnitDetail\" ud ON sl.\"Id\" = ud.\"ServiceLogId\" " +
                $"INNER JOIN \"SubProcedure\" sp ON sp.\"Id\" = ud.\"SubProcedureId\" " +
                $"left JOIN \"PatientAccount\" pa ON pa.\"ClientId\" = cl.\"Id\" AND ud.\"DateOfService\" BETWEEN pa.\"CreateDate\" AND pa.\"ExpireDate\" " +
                $"and coalesce(CASE WHEN(sp.\"Name\" LIKE '%51' OR sp.\"Name\" LIKE '%51TS') " +
                $"THEN pa.\"Auxiliar\" ELSE pa.\"LicenseNumber\" END, 'DOES NOT APPLY') <> 'DOES NOT APPLY' " +
                $"WHERE pa.\"Id\" is null " +
                $"ORDER BY cl.\"Name\", ct.\"Name\", ud.\"DateOfService\";";
            //var companyParam = new SqlParameter("@company_id", company_id);
            //var periodParam = new SqlParameter("@period_id", period_id);

            return _db.Database.SqlQuery<ServiceLogWithoutPatientAccount>(sqlQuery/*, companyParam, periodParam*/).ToList();
        }

        public GeneralData GetGeneralData(int company_id, int period_id)
        {
            var sqlClient = $"select count(*) from \"Client\" c inner join \"Agreement\" a ON a.\"ClientId\" = c.\"Id\" and a.\"CompanyId\" = {company_id}";
            var sqlContractor = $"select count(*) from \"Contractor\" c inner join \"Payroll\" p on p.\"ContractorId\" = c.\"Id\" and p.\"CompanyId\" = {company_id}";
            var sqlServiceLog = $"select count(*) from \"ServiceLog\" sl inner join \"Client\" c on sl.\"ClientId\" = c.\"Id\" " +
                $"inner join \"Agreement\" a on a.\"ClientId\" = c.\"Id\" " +
                $"inner join \"Contractor\" c2 on sl.\"ContractorId\" = c2.\"Id\" " +
                $"inner join \"Payroll\" p on p.\"ContractorId\" = c2.\"Id\" " +
                $"where sl.\"PeriodId\" = {period_id} and a.\"CompanyId\" = {company_id}";

            var tClient = _db.Database.SqlQuery<int>(sqlClient).FirstOrDefault();//_db.Client.Where(x => x.Agreement.Any(a => a.CompanyId == company_id)).Count();
            var tContractor = _db.Database.SqlQuery<int>(sqlContractor).FirstOrDefault();//_db.Contractor.Where(x => x.Payroll.Any(p => p.CompanyId == company_id)).Count();
            var tServiceLog = _db.Database.SqlQuery<int>(sqlServiceLog).FirstOrDefault(); //_db.ServiceLog.Where(x => x.PeriodId == period_id && x.Client.Agreement.Any(a => a.CompanyId == company_id) && x.Contractor.Payroll.Any(p => p.CompanyId == company_id)).Count();

            return new GeneralData
            {
                Client = tClient,
                Contractor = tContractor,
                ServiceLog = tServiceLog
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

        public async Task<DashboardSetting> FillDasboardSettings()
        {
            return new DashboardSetting() { Company = (await GetCompanies()).FirstOrDefault(), Period = (await GetPeriods()).FirstOrDefault() };
        }
    }
}
