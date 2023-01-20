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
        private readonly Clinic_AppContext _db;
        public ManagerService(Clinic_AppContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ManagerBiller>> GetServiceLogsBilled(int period, int company)
        {
            var query = await (from sl in _db.ServiceLog
                               join cl in _db.Client on sl.ClientId equals cl.Id
                               join co in _db.Contractor on sl.ContractorId equals co.Id
                               join us in _db.AspNetUsers on sl.Biller equals us.Id
                               where sl.PeriodId == period && cl.Agreement.Any(ag => ag.CompanyId == company) && sl.Biller != null
                               select new ManagerBiller
                               {
                                   Id = sl.Id,
                                   BilledDate = (DateTimeOffset)sl.BilledDate,
                                   Biller = us.UserName,
                                   ClientName = cl.Name,
                                   ContractorName = co.Name,
                                   CreationDate = (DateTimeOffset)sl.CreatedDate
                               }).OrderBy(x => x.ClientName).ToListAsync();
            return query;
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
