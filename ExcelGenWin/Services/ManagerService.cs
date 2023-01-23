using ABABillingAndClaim.Models;
using ClinicDOM;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public class ManagerService
    {
        private MemoryService _memoryService;
        public static ManagerService Instance { get; private set; }

        public ManagerService(MemoryService memoryService)
        {
            _memoryService = memoryService;
            Instance = this;
        }

        public async Task<IEnumerable<ManagerBiller>> GetServiceLogsBilled(int period, int company)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/GetServiceLogsBilled/{period}/{company}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<ManagerBiller>>(response.Content);
            else
                return null;

        }

        public async Task<object> UpdateBilling(int servicelog)
        {
            var client = new RestClient($"{_memoryService.BaseEndPoint}/billing/UpdateBilling/{servicelog}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<IEnumerable<ManagerBiller>>(response.Content);
            else
                return null;
        }
    }
}
