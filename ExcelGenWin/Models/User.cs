﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Models
{
    public class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public List<string> roles { get; set; }
        public string email { get; set; }

    }
}
