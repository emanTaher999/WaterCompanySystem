using WaterCompanySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCompanySystem.ViewModel
{
    public class CusTest
    {
        public int id { get; set; }
        public string cusmor_name { get; set; }
        public string location_cus { get; set; }
        public string contact_number { get; set; }
        public string email { get; set; }
        public Nullable<int> number_of_bottel { get; set; }
        public int bottel_type_id { get; set; }
        public System.DateTime creat_at { get; set; }

        public virtual BottleType BottleType { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Delivery> Deliveries { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Income_Transaction> Income_Transaction { get; set; }
    }
}