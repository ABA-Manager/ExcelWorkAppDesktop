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
    
    public partial class UnitDetail
    {
        public int Id { get; set; }
        public string Modifiers { get; set; }
        public int PlaceOfServiceId { get; set; }
        public System.DateTime DateOfService { get; set; }
        public int Unit { get; set; }
        public int ServiceLogId { get; set; }
        public int SubProcedureId { get; set; }
    
        public virtual PlaceOfService PlaceOfService { get; set; }
        public virtual ServiceLog ServiceLog { get; set; }
        public virtual SubProcedure SubProcedure { get; set; }
    }
}
