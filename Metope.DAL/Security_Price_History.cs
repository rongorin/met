//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Metope.DAL
{
    using Metope.DAL.MyMetaData;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(SecurityPriceHistoryModelMetaData))]
    
    public partial class Security_Price_History
    {
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
        public System.DateTime Price_DateTime { get; set; }
        public System.DateTime Record_Date { get; set; }
        public decimal Session_ID { get; set; }
        public Nullable<System.DateTime> Hist_Last_Update_Date { get; set; }
        public string Hist_Last_Update_User { get; set; }
        public Nullable<decimal> Issued_Amount { get; set; }
        public Nullable<decimal> Free_Float_Issued_Amount { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User User { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }
    }
}
