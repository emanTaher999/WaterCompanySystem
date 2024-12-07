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
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Deliveries = new HashSet<Delivery>();
        }
    
        public int id { get; set; }
        public string emp_name { get; set; }
        public Nullable<int> position_id { get; set; }
        public Nullable<decimal> salary { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> hire_date { get; set; }
        public Nullable<int> vacation_days { get; set; }
        public Nullable<decimal> alwance { get; set; }
        public int userid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual Position Position { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}
