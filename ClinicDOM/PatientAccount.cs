//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicDOM
{
    using System;
    using System.Collections.Generic;
    
    public partial class PatientAccount
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ExpireDate { get; set; }
        public string Auxiliar { get; set; }
        public int ClientId { get; set; }
    
        public virtual Client Client { get; set; }
    }
}
