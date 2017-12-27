using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Metope.DAL;
namespace MetopeMVCApp.Models
{
    public class SecurityPriceEditModel
    { 
        //public Security_Price SecurityPrice{ get; set; }
        //public Security_Detail Security_Detail { get; set; }
 
        public string Security_Name { get; set; } 
        //public SecurityPriceEditModel( )
        //{ 
        //    SecurityPrice = new Security_Price();
        //}
        //public SecurityPriceEditModel(Security_Price price)
        //{
        //    SecurityPrice = price;
        //} 
        public decimal Entity_ID { get; set; }
        public decimal Security_ID { get; set; }
        public string Price_Curr { get; set; }
        public Nullable<decimal> All_In_Price { get; set; }
        public Nullable<decimal> Clean_Price { get; set; }
        public Nullable<decimal> Accrued_Income_Price { get; set; }
        public string Price_Source { get; set; }
        public Nullable<decimal> Yield_To_Maturity { get; set; }
        public Nullable<decimal> Discount_Rate { get; set; }
        public string Last_Update_User { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public Nullable<decimal> Issued_Amount { get; set; }
        public Nullable<decimal> Free_Float_Issued_Amount { get; set; }
        public System.DateTime Record_Date { get; set; }


    }
}