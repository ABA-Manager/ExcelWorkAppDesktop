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
    
    public partial class ServiceLog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceLog()
        {
            this.UnitDetail = new HashSet<UnitDetail>();
        }
    
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<System.DateTimeOffset> BilledDate { get; set; }
        public string Biller { get; set; }
        public string Pending { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual Period Period { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitDetail> UnitDetail { get; set; }
    }
}
