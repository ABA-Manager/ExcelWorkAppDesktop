using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Models
{
    public class ManagerBiller
    {
        public int Id { get; set; }
        public string ClientName { get; set; }  
        public string ContractorName { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string Biller { get; set; }
        public DateTimeOffset BilledDate { get; set; }
    }
}
