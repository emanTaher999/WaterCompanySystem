using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCompanySystem.ViewModel
{
    public class AllStoreVM
    {

        public string suplier_type { get; set; }
        public int FirstQuantity { get;   set; }
        public int TotalExpenseQuantity { get; set; }
        public int TotalIncomeQuantity { get; set; }
        public int StoreQuantity { get; set; }

    }
}