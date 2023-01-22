using ABABillingAndClaim.Models;
using ClinicDOM;
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
        public static ManagerService Instance { get; private set; }
        private Clinic_AppContext db;
        public ManagerService(Clinic_AppContext db)
        {
            this.db = db;
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public async Task<ActionResult<IEnumerable<ManagerBiller>>> GetServiceLogsBilled(int period, int company)
        {
            //var query = await (from sl in _db.ServiceLog
            //                   join cl in _db.Client on sl.ClientId equals cl.Id
            //                   join co in _db.Contractor on sl.ContractorId equals co.Id
            //                   join us in _db.AspNetUsers on sl.Biller equals us.Id
            //                   where sl.PeriodId == period && cl.Agreement.Any(ag => ag.CompanyId == company) && sl.Biller != null
            //                   select new ManagerBiller
            //                   {
            //                       Id = sl.Id,
            //                       BilledDate = (DateTime)sl.BilledDate,
            //                       Biller = us.UserName,
            //                       ClientName = cl.Name,
            //                       ContractorName = co.Name,
            //                       CreationDate = (DateTime)sl.CreatedDate
            //                   }).OrderBy(x => x.ClientName).ToListAsync();
            //return query;

            var client = new RestClient($"{_memoryService.BaseEndPoint}/GetServiceLogsBilled/{period}/{company}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_memoryService.Token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            if ((int)response.StatusCode == 200 || (int)response.StatusCode == 409)
                return JsonConvert.DeserializeObject<Task<ActionResult<IEnumerable<ManagerBiller>>>>(response.Content);
            else
                return null;

        }

        public async Task<string> UpdateBilling(int servicelog)
        {
            var service = await _db.ServiceLog.FirstOrDefaultAsync(sl => sl.Id == servicelog);
            if (service == null) return "Service Log not found";
            else
            {
                service.Biller = null;
                service.BilledDate = null;

                await _db.SaveChangesAsync();
                return "Service log has been successfully updated";
            }
        }
    }
}
