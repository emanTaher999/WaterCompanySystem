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
    
    public partial class Custmor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Custmor()
        {
            this.Deliveries = new HashSet<Delivery>();
            this.Income_Transaction = new HashSet<Income_Transaction>();
        }
    
        public int id { get; set; }
        public string cusmor_name { get; set; }
        public string location_cus { get; set; }
        public string contact_number { get; set; }
        public string email { get; set; }
        public Nullable<int> number_of_bottel { get; set; }
        public int bottel_type_id { get; set; }
        public System.DateTime creat_at { get; set; }
    
        public virtual BottleType BottleType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Delivery> Deliveries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Income_Transaction> Income_Transaction { get; set; }
    }
}
