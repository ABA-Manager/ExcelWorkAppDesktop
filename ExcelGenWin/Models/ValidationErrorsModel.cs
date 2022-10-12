using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Models
{
    public class ValidationErrorsModel
    {
        public Dictionary<string, List<string>> Errors { get; set; }
        public string Type{ get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string TraceId { get; set; }

    }
}
