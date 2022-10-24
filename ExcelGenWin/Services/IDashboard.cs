using ABABillingAndClaim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public interface IDashboard
    {
        IEnumerable<ProfitHistory> GetPayroll();
    }
}
