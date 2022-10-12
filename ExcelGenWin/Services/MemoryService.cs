using ABABillingAndClaim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public class MemoryService
    {

        public bool Connected { get; set; }
        public string Token { get; set; }
        public User LoggedOndUser { get; set; }
        public string BaseEndPoint { get; set; } 

        public MemoryService()
        {
            this.Connected = false;
            this.BaseEndPoint = Properties.Settings.Default.BASE_ENDPOINT;
        }
    }
}
