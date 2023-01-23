using ABABillingAndClaim.Models;
using ABABillingAndClaim.Utils;
using ClinicApp.MSBilling.Dtos;
using ClinicDOM;
using ClinicDOM.DAO;
using ExcelGenLib;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public class BillingService
    {
        private MemoryService _memoryService;

        public static BillingService Instance { get; private set; }

        public BillingService(MemoryService memoryService)
        {
            _memoryService = memoryService;
            Instance = this;
        }

        public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetPeriods")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
            {

                var des = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<ExtendedPeriod>>(response.Content));
                return des;
            }
            else
                return null;
        }

        public async Task<Period> GetPeriodAsync(int periodId)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetPeriod/{periodId}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<Period>(response.Content);
            else
                return null;

        }


        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetCompanies")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<Company>>(response.Content);
            else
                return null;
        }

        public async Task<IEnumerable<TvClient>> GetContractorAndClientsAsync(string companyCode, int periodId)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetContractorAndClients/{companyCode}/{periodId}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<TvClient>>(response.Content);
            else
                return null;
        }

        public async Task<Agreement> GetAgreementAsync(string companyCode, int periodID, int contractorID, int clientID)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetAgreement/{companyCode}/{periodID}/{contractorID}/{clientID}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<Agreement>(response.Content);
            else
                return null;
        }

        public async Task<IEnumerable<ExtendedUnitDetail>> GetExUnitDetailsAsync(int periodId, int contractorId, int clientId, string pAccount, string sufixList)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetExUnitDetails/{periodId}/{contractorId}/{clientId}/{pAccount}/{sufixList}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<ExtendedUnitDetail>>(response.Content);
            else
                return null;

        }

        public async Task<IEnumerable<ExtendedUnitDetail>> GetExUnitDetailsAsync(int serviceLogId, string pAccount, string sufixList)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetExUnitDetails/{serviceLogId}/{pAccount}/{sufixList}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<ExtendedUnitDetail>>(response.Content);
            else
                return null;
        }

        public async Task<ExtendedServiceLog> GetExServiceLogAsync(string companyCode, int serviceLogId)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetExServiceLog/{companyCode}/{serviceLogId}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<ExtendedServiceLog>(response.Content);
            else
                return null;

        }

        public async Task<object> SetServiceLogBilled(int serviceLogId, string userId)
        {
            var client = new RestClient(_memoryService.BaseEndPoint + $"/billing/SetServiceLogBilled/{serviceLogId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");
            Dictionary<string, string> user = new Dictionary<string, string>();
            user.Add("userId", userId);

            var body = JsonConvert.SerializeObject(user);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject(response.Content);
            }
            else return null;
        }

        public async Task<object> SetServiceLogBilled(int periodId, int contratorId, int clientId, string userId)
        {
            var client = new RestClient(_memoryService.BaseEndPoint + $"/billing/SetServiceLogBilled");
            var billed = new SetServiceLogBilledRequest
            {
                ClientId = clientId,
                ContratorId = contratorId,
                UserId = userId,
                PeriodId = periodId
            };
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(billed);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject(response.Content);
            }
            else return null;
        }

        public async Task<object> SetServiceLogPendingReason(int serviceLogId, string reason)
        {
            var client = new RestClient(_memoryService.BaseEndPoint + $"/billing/SetServiceLogPendingReason/{serviceLogId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");
            Dictionary<string, string> user = new Dictionary<string, string>();
            user.Add("reason", reason);

            var body = JsonConvert.SerializeObject(user);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject(response.Content);
            }
            else return null;
        }
    }
}
