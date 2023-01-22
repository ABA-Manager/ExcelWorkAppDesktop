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
    
    public partial class Payroll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Payroll()
        {
            this.Agreement = new HashSet<Agreement>();
        }
    
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int ContractorTypeId { get; set; }
        public int ProcedureId { get; set; }
        public int CompanyId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agreement> Agreement { get; set; }
        public virtual Company Company { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ContractorType ContractorType { get; set; }
        public virtual Procedure Procedure { get; set; }
    }
}
