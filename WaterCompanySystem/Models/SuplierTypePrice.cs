//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaterCompanySystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SuplierTypePrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SuplierTypePrice()
        {
            this.Income_Transaction = new HashSet<Income_Transaction>();
        }
    
        public int id { get; set; }
        public Nullable<int> suplier_Type_id { get; set; }
        public string nick { get; set; }
        public Nullable<decimal> price { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Income_Transaction> Income_Transaction { get; set; }
        public virtual SuplierType SuplierType { get; set; }
    }
}
