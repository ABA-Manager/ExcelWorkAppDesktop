using ABABillingAndClaim.Models;
using ClinicDOM;
using Newtonsoft.Json;
using Npgsql;
using RestSharp;
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

        private MemoryService _memoryService;

        public DashboardService(MemoryService memoryService)
        {
            _memoryService = memoryService;
            Instance = this;
        }
        public IEnumerable<ProfitHistory> GetProfit(int company_id)
        {
            //var sqlQuery = $"select \"PayPeriod\", " +
            //    $"round(cast(sum((\"Unit\") * CASE WHEN sp.\"Id\" = 1 then pro.\"Rate\" ELSE sp.\"Rate\" END) as numeric),2) as Billed, " +
            //    $"round(cast(sum((\"Unit\"/4) * ag.\"RateEmployees\") as numeric),2) as Payment, " +
            //    $"round(cast(sum((\"Unit\") * CASE WHEN sp.\"Id\" = 1 then pro.\"Rate\" ELSE sp.\"Rate\" END) as numeric),2) - " +
            //    $"round(cast(sum((\"Unit\"/4) * ag.\"RateEmployees\") as numeric),2) as Profit " +
            //    $"from \"Agreement\" ag " +
            //    $"inner join \"Payroll\" pr on ag.\"PayrollId\" = pr.\"Id\" and pr.\"CompanyId\" = ag.\"CompanyId\" " +
            //    $"inner join \"Procedure\" pro on pro.\"Id\" = pr.\"ProcedureId\" " +
            //    $"inner join \"ServiceLog\" sl on sl.\"ClientId\" = ag.\"ClientId\" and pr.\"ContractorId\" = sl.\"ContractorId\" " +
            //    $"inner join \"Period\" p on p.\"Id\" = sl.\"PeriodId\" " +
            //    $"inner join \"UnitDetail\" ud on sl.\"Id\" = ud.\"ServiceLogId\" " +
            //    $"inner join \"SubProcedure\" sp on ud.\"SubProcedureId\" = sp.\"Id\" " +
            //    $"WHERE ag.\"CompanyId\" = {company_id} " +
            //    $"group by p.\"Id\", p.\"PayPeriod\" " +
            //    $"order by p.\"Id\";";
            //return _db.Database.SqlQuery<ProfitHistory>(sqlQuery).ToList();

            var client = new RestClient($"{_memoryService.BaseEndPoint}/dashboard/Profit/{company_id}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<ProfitHistory>>(response.Content);
            else
                return null;
        }

        public ServicesLogStatus GetServicesLogStatus(int company_id, int period_id)
        {
            //var sqlQuery = $"SELECT SUM(CASE WHEN \"Biller\" IS NOT NULL THEN 1 ELSE 0 END) AS Billed, " +
            //    $"SUM(CASE WHEN \"Pending\" IS NOT NULL THEN 1 ELSE 0 END) AS Pending, " +
            //    $"SUM(CASE WHEN \"Biller\" IS NULL AND \"Pending\" IS NULL THEN 1 ELSE 0 END) AS NotBilled " +
            //    $"from \"Agreement\" ag " +
            //    $"inner join \"Payroll\" pr on ag.\"PayrollId\" = pr.\"Id\" and pr.\"CompanyId\" = ag.\"CompanyId\" " +
            //    $"inner join \"ServiceLog\" sl on sl.\"ClientId\" = ag.\"ClientId\" and pr.\"ContractorId\" = sl.\"ContractorId\" " +
            //    $"inner join \"Period\" p on p.\"Id\" = sl.\"PeriodId\" " +
            //    $"WHERE sl.\"PeriodId\" = {period_id} " +
            //    $"  and  ag.\"CompanyId\" = {company_id}; ";

            //return _db.Database.SqlQuery<ServicesLogStatus>(sqlQuery).FirstOrDefault();

            var client = new RestClient($"{_memoryService.BaseEndPoint}/dashboard/GetServicesLogStatus/{company_id}/{period_id}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<ServicesLogStatus>(response.Content);
            else
                return null;
        }

        public IEnumerable<ServiceLogWithoutPatientAccount> GetServiceLogWithoutPatientAccount(int company_id, int period_id)
        {
            //var sqlQuery = $"SELECT cl.\"Id\", cl.\"Name\" Client, ct.\"Id\", ct.\"Name\" Contractor, " +
            //    $"ud.\"DateOfService\", ud.\"SubProcedureId\", sp.\"Name\" \"Procedure\" " +
            //    $"FROM \"Agreement\" ag " +
            //    $"INNER JOIN \"Company\" c ON c.\"Id\" = ag.\"CompanyId\" AND c.\"Id\" = {company_id} " +
            //    $"INNER JOIN \"Client\" cl ON cl.\"Id\" = ag.\"ClientId\" " +
            //    $"INNER JOIN \"Payroll\" pr ON pr.\"Id\" = ag.\"PayrollId\" " +
            //    $"INNER JOIN \"Contractor\" ct ON ct.\"Id\" = pr.\"ContractorId\" " +
            //    $"INNER JOIN \"ServiceLog\" sl ON sl.\"ClientId\" = cl.\"Id\" AND sl.\"ContractorId\" = ct.\"Id\" AND sl.\"PeriodId\"= {period_id} " +
            //    $"INNER JOIN \"UnitDetail\" ud ON sl.\"Id\" = ud.\"ServiceLogId\" " +
            //    $"INNER JOIN \"SubProcedure\" sp ON sp.\"Id\" = ud.\"SubProcedureId\" " +
            //    $"left JOIN \"PatientAccount\" pa ON pa.\"ClientId\" = cl.\"Id\" AND ud.\"DateOfService\" BETWEEN pa.\"CreateDate\" AND pa.\"ExpireDate\" " +
            //    $"and coalesce(CASE WHEN(sp.\"Name\" LIKE '%51' OR sp.\"Name\" LIKE '%51TS') " +
            //    $"THEN pa.\"Auxiliar\" ELSE pa.\"LicenseNumber\" END, 'DOES NOT APPLY') <> 'DOES NOT APPLY' " +
            //    $"WHERE pa.\"Id\" is null " +
            //    $"ORDER BY cl.\"Name\", ct.\"Name\", ud.\"DateOfService\";";

            //return _db.Database.SqlQuery<ServiceLogWithoutPatientAccount>(sqlQuery).ToList();

            var client = new RestClient($"{_memoryService.BaseEndPoint}/dashboard/GetServiceLogWithoutPatientAccount/{company_id}/{period_id}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<ServiceLogWithoutPatientAccount>>(response.Content);
            else
                return null;
        }

        public GeneralData GetGeneralData(int company_id, int period_id)
        {
            //var sqlClient = $"select count(*) from \"Client\" c inner join \"Agreement\" a ON a.\"ClientId\" = c.\"Id\" and a.\"CompanyId\" = {company_id}";
            //var sqlContractor = $"select count(*) from \"Contractor\" c inner join \"Payroll\" p on p.\"ContractorId\" = c.\"Id\" and p.\"CompanyId\" = {company_id}";
            //var sqlServiceLog = $"select count(*) from \"ServiceLog\" sl inner join \"Client\" c on sl.\"ClientId\" = c.\"Id\" " +
            //    $"inner join \"Agreement\" a on a.\"ClientId\" = c.\"Id\" " +
            //    $"inner join \"Contractor\" c2 on sl.\"ContractorId\" = c2.\"Id\" " +
            //    $"inner join \"Payroll\" p on p.\"ContractorId\" = c2.\"Id\" " +
            //    $"where sl.\"PeriodId\" = {period_id} and a.\"CompanyId\" = {company_id}";

            //var tClient = _db.Database.SqlQuery<int>(sqlClient).FirstOrDefault();//_db.Client.Where(x => x.Agreement.Any(a => a.CompanyId == company_id)).Count();
            //var tContractor = _db.Database.SqlQuery<int>(sqlContractor).FirstOrDefault();//_db.Contractor.Where(x => x.Payroll.Any(p => p.CompanyId == company_id)).Count();
            //var tServiceLog = _db.Database.SqlQuery<int>(sqlServiceLog).FirstOrDefault(); //_db.ServiceLog.Where(x => x.PeriodId == period_id && x.Client.Agreement.Any(a => a.CompanyId == company_id) && x.Contractor.Payroll.Any(p => p.CompanyId == company_id)).Count();

            //return new GeneralData
            //{
            //    Client = tClient,
            //    Contractor = tContractor,
            //    ServiceLog = tServiceLog
            //};

            var client = new RestClient($"{_memoryService.BaseEndPoint}/dashboard/GetGeneralData/{company_id}/{period_id}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<GeneralData>(response.Content);
            else
                return null;
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            //return await _db.Company.ToListAsync();

            var client = new RestClient($"{_memoryService.BaseEndPoint}/dashboard/GetCompanies")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = await Task.Factory.StartNew(() => client.Execute(request));

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Company>>(response.Content));
            else
                return null;
        }

        public async Task<IEnumerable<Period>> GetPeriods()
        {
            //var periodQry = await _db.Period
            //     .Where(p => p.StartDate < DateTime.Now)
            //     .OrderByDescending(p => p.StartDate)
            //     .ToListAsync();

            //return periodQry;

            var client = new RestClient($"{_memoryService.BaseEndPoint}/dashboard/GetPeriods")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = await Task.Factory.StartNew(() => client.Execute(request));

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Period>>(response.Content));
            else
                return null;
        }

        public async Task<DashboardSetting> FillDasboardSettings()
        {
            return new DashboardSetting() { Company = (await GetCompanies()).FirstOrDefault(), Period = (await GetPeriods()).FirstOrDefault() };
        }
    }
}
