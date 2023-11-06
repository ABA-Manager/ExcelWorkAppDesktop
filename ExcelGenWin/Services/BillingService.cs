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
using System.Runtime.Remoting.Contexts;
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

                var des = //await Task.Factory.StartNew(() => 
                JsonConvert.DeserializeObject<List<ExtendedPeriod>>(response.Content);//);
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
            IEnumerable<TvFullData> queryRes;
            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                queryRes = JsonConvert.DeserializeObject<IEnumerable<TvFullData>>(response.Content);
            else
                return null;

            TvClient lastClient = null;
            TvContractor lastContractor = null;
            TvServiceLog lastServiceLog = null;

            var clientList = new List<TvClient>();
            foreach (var it in queryRes)
            {
                if (it.clientName != null && it.contractorName != null && it.serviceLogCreatedDate != null)
                {
                    var paNum = it.patientAccountAuxiliar != null ? it.patientAccountAuxiliar : it.patientAccountLicenseNumber; //it.pa != null ? (sufixList.Contains(it.sp.Name.Substring(3) + ";") ? it.pa.Auxiliar : it.pa.LicenseNumber) : it.cl.AuthorizationNUmber;
                    if (it.clientId.ToString() + $"_{paNum}" != lastClient?.Id)
                    {
                        clientList.Add(lastClient = new TvClient()
                        {
                            Id = it.clientId.ToString() + $"_{paNum}",
                            Name = it.clientName.Trim() + $" ({paNum})",
                        });
                        lastContractor = null; lastServiceLog = null;
                    }
                    if (lastContractor == null || int.Parse(lastContractor.Id) != it.contractorId)
                    {
                        lastClient.Contractors.Add(lastContractor = new TvContractor()
                        {
                            Id = it.contractorId.ToString(),
                            Name = it.contractorName.Trim(),
                            ContratorType = it.contractorTypeName,
                            Client = lastClient
                        });
                        lastServiceLog = null;
                    }

                    if (lastServiceLog == null || int.Parse(lastServiceLog.Id) != it.serviceLogId)
                        lastContractor.ServiceLogs.Add(lastServiceLog = new TvServiceLog()
                        {
                            Id = it.serviceLogId.ToString(),
                            CreatedDate = it.serviceLogCreatedDate,
                            Status = (it.serviceLogBilledDate != null) ? "billed" : "empty",
                            Contractor = lastContractor
                        });
                }
            }
            
            return clientList;
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

        public async Task<IEnumerable<UnitDetail>> GetExUnitDetailsAsync(int periodId, int contractorId, int clientId, string pAccount, string sufixList)
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
                return JsonConvert.DeserializeObject<IEnumerable<UnitDetail>>(response.Content);
            else
                return null;

        }

        public async Task<IEnumerable<UnitDetail>> GetExUnitDetailsAsync(int serviceLogId, string pAccount, string sufixList)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetExUnitDetailsAux/{serviceLogId}/{pAccount}/{sufixList}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<UnitDetail>>(response.Content);
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

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
            {
                return JsonConvert.DeserializeObject(response.Content);
            }
            else return null;
        }
    }
}
