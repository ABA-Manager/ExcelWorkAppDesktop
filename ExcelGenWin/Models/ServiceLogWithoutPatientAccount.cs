using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Models
{
    public class ServiceLogWithoutPatientAccount
    {
        public string Client { get; set; }
        public string Contractor { get; set; }
        public DateTime DateOfService { get; set; }
        public string Procedure { get; set; }
    }
}
