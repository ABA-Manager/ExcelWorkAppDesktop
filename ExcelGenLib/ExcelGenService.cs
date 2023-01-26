using ClinicDOM;
using ClinicDOM.DAO;
using RestSharp;
using Newtonsoft.Json;
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
        public static ExcelGenService Instance { get; private set; }
        private string _endPoint;
        private string _token;

        public ExcelGenService(string endPoint, string token)
        {
            _endPoint = endPoint;
            _token = token;
            Instance = this;
        }

        public async Task<List<Company>> GetCompanies()
        {
            //var companyQry = from c in db.Company
            //                 select c;

            //return await companyQry.ToListAsync();
            ////foreach (var co in companyQry.ToList())
            ////    CompanyData.Add(co.Acronym, co.Name);            

            var client = new RestClient($"{_endPoint}/excelgen/GetCompanies")
            {
                Timeout = -1
            };

            var request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            return await Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                    return JsonConvert.DeserializeObject<List<Company>>(response.Content);
                else
                    return null;
            });
        }

        public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
        {
            //var periodQry = from p in db.Period
            //                where (p.StartDate < DateTime.Now)
            //                orderby p.StartDate descending
            //                select new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod };

            //return await periodQry.ToListAsync();

            var client = new RestClient($"{_endPoint}/excelgen/GetPeriodsAsync")
            {
                Timeout = -1
            };

            var request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            return await Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                    return JsonConvert.DeserializeObject<List<ExtendedPeriod>>(response.Content);
                else
                    return null;
            });
        }

        public async Task<ExtendedPeriod> GetPeriodAsync(int PeriodId = -1)
        {
            //var periodQry = (from p in db.Period
            //                     //join pp in db.Period on p.StartDate equals DbFunctions.AddDays(pp.EndDate, 1)
            //                 where (PeriodId == -1 && p.EndDate < DateTime.Now) || (PeriodId != -1 && p.Id == PeriodId)
            //                 orderby p.StartDate descending
            //                 select p).Take(1);

            //return await periodQry.SingleOrDefaultAsync();

            var client = new RestClient($"{_endPoint}/excelgen/GetPeriodAsync/{PeriodId}")
            {
                Timeout = -1
            };

            var request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            return await Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                    return JsonConvert.DeserializeObject<ExtendedPeriod>(response.Content);
                else
                    return null;
            });
        }

        public async Task<List<ExtendedContractor>> GetExContractorsAsync(string companyCode)
        {
            //var contractorsQry = (from ag in db.Agreement
            //                      where ag.Payroll.ContractorTypeId == 1 && (companyCode == "" | ag.Company.Acronym == companyCode)
            //                      select new ExtendedContractor { contractor = ag.Payroll.Contractor, company = ag.Company }).Distinct();

            //return await contractorsQry.ToListAsync();

            var client = new RestClient($"{_endPoint}/excelgen/GetExContractorsAsync/{companyCode}")
            {
                Timeout = -1
            };

            var request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            return await Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                    return JsonConvert.DeserializeObject<List<ExtendedContractor>>(response.Content);
                else
                    return null;
            });
        }

        public async Task<List<Agreement>> GetAgreementsAsync(int ContractorId, int CompanyId)
        {
            //var agreementQry = from ag in db.Agreement
            //                   where ag.Payroll.ContractorId == ContractorId && ag.CompanyId == CompanyId
            //                   select ag;

            //return await agreementQry.ToListAsync();

            var client = new RestClient($"{_endPoint}/GetAgreementsAsync/{ContractorId}/{CompanyId}")
            {
                Timeout = -1
            };

            var request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            return await Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                    return JsonConvert.DeserializeObject<List<Agreement>>(response.Content);
                else
                    return null;
            });

        }

        public async Task<List<ExtendedAgrUnitDetail>> GetExAgrUnitDetails(int ClientId, int ContractorId, int CompanyId, int PeriodId)
        {
            //var unitDetQry = from ag in db.Agreement
            //                 join pr in db.Payroll on ag.PayrollId equals pr.Id
            //                 join sl in db.ServiceLog on new { ag.ClientId, pr.ContractorId } equals new { sl.ClientId, sl.ContractorId }
            //                 join ud in db.UnitDetail on sl.Id equals ud.ServiceLogId
            //                 where sl.ClientId == ClientId &&
            //                   ((ag.Payroll.ContractorTypeId == 1 && sl.ContractorId == ContractorId) ||
            //                    (ag.Payroll.ContractorTypeId != 1 && !(from inag in db.Agreement where inag.CompanyId == CompanyId && inag.ClientId == ClientId && inag.Payroll.ContractorId < ContractorId && inag.Payroll.ContractorTypeId == 1 select inag).Any())) &&
            //                   ag.CompanyId == CompanyId &&
            //                   sl.PeriodId == PeriodId
            //                 //sl.CreatedDate > DbFunctions.AddDays(previousPeriod.DocumentDeliveryDate, 2) &&
            //                 //sl.CreatedDate <= DbFunctions.AddDays(period.DocumentDeliveryDate, 2)                                   
            //                 orderby sl.ClientId, ag.Payroll.ContractorTypeId, sl.ContractorId, ud.SubProcedureId, ud.DateOfService
            //                 select new ExtendedAgrUnitDetail
            //                 {
            //                     serviceLog = sl,
            //                     unitDetail = ud,
            //                     agreement = ag
            //                 };
            //return await unitDetQry.ToListAsync();

            var client = new RestClient($"{_endPoint}/GetExAgrUnitDetails/{ClientId}/{ContractorId}/{CompanyId}/{PeriodId}")
            {
                Timeout = -1
            };

            var request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            return await Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                    return JsonConvert.DeserializeObject<List<ExtendedAgrUnitDetail>>(response.Content);
                else
                    return null;
            });
        }
    }
}
