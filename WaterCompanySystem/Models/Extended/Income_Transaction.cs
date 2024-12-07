using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCompanySystem.Models.Extended
{
    [MetadataType(typeof(Income_TransactionMetaData))]  
    public partial class Income_Transaction
    {
       
    }
    public class Income_TransactionMetaData
    {
        public int id { get; set; }
        // [Display(Name = "Date Of Brith")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> transaction_date { get; set; }
        public int transaction_type_id { get; set; }
        public Nullable<decimal> amount { get; set; }
        public int custmor_id { get; set; }
      
        public Nullable<int> quantity { get; set; }
        public Nullable<int> suplier_type_id { get; set; }
        public Nullable<int> payment_type_id { get; set; }
        public string note { get; set; }

        public virtual Custmor Custmor { get; set; }
        public virtual Payment_Type Payment_Type { get; set; }
        public virtual SuplierTypePrice SuplierTypePrice { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}