using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ABABillingAndClaim.Models
{
    public class ResetPasswordViewModel
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
}


