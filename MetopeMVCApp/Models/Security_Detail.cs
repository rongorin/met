//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MetopeMVCApp.Models
{
    using System;
    using System.Collections.Generic;
    using MetopeMVCApp.Models.MyMetaData;
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(SecurityDetailModelMetaData))]
    
    public partial class Security_Detail
    {
        public Security_Detail()
        {
            this.Currencies = new HashSet<Currency>();
            this.Currencies1 = new HashSet<Currency>();
            this.Security_Price_History = new HashSet<Security_Price_History>();
            this.Security_Price = new HashSet<Security_Price>();
        }
    
        public decimal Security_ID { get; set; }
        public decimal Entity_ID { get; set; }
        public string Security_Name { get; set; }
        public string Short_Name { get; set; }
        public string Primary_Exch { get; set; }
        public string Secondary_Exch { get; set; }
        public string Country_Of_Domicile { get; set; }
        public string Country_Of_Risk { get; set; }
        public string Security_Type_Code { get; set; }
        public decimal Price_Multiplier { get; set; }
        public Nullable<decimal> Income_Frequency { get; set; }
        public string Issuer_Code { get; set; }
        public string Ultimate_Issuer_Code { get; set; }
        public string Asset_Currency { get; set; }
        public Nullable<decimal> Min_Lot_Size { get; set; }
        public int Decimal_Precision { get; set; }
        public int AvePrice_Rounding { get; set; }
        public Nullable<System.DateTime> Issue_Date { get; set; }
        public Nullable<System.DateTime> Maturity_Date { get; set; }
        public Nullable<decimal> Coupon_Rate { get; set; }
        public string Price_Exchange { get; set; }
        public string Trade_Currency { get; set; }
        public string Price_Curr { get; set; }
        public string Currency_Pair_Code { get; set; }
        public string Share_Class { get; set; }
        public decimal Current_Market_Price { get; set; }
        public string Index_Type { get; set; }
        public string Clean_Price_Formula { get; set; }
        public string Accrued_Income_Price_Formula { get; set; }
        public Nullable<System.DateTime> Odd_First_Coupon_Date { get; set; }
        public Nullable<System.DateTime> Odd_Last_Coupon_Date { get; set; }
        public string Coupon_Anniversary_Indicator { get; set; }
        public Nullable<bool> Track_EOM_Flag { get; set; }
        public Nullable<System.DateTime> Next_Coupon_Date { get; set; }
        public Nullable<System.DateTime> Previous_Coupon_Date { get; set; }
        public string Payment_Frequency { get; set; }
        public string Coupon_BusDay_Adjustment { get; set; }
        public Nullable<System.DateTime> Next_Ex_Div_Date { get; set; }
        public string Ex_Div_BusDay_Adjustment { get; set; }
        public string Ex_Div_Period { get; set; }
        public string Ticker { get; set; }
        public string Inet_ID { get; set; }
        public string Bloomberg_ID { get; set; }
        public string External_Sec_ID { get; set; }
        public string Reuters_ID { get; set; }
        public string ISIN { get; set; }
        public Nullable<bool> Call_Account_Flag { get; set; }
        public bool System_Locked { get; set; }
        public string Last_Update_User { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public string Security_Status { get; set; }
        public string Benchmark_Portfolio { get; set; }
        public Nullable<bool> Active_flag { get; set; }
        public decimal Dividend_FX_Security_ID { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }
        public virtual ICollection<Currency> Currencies1 { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
        public virtual Currency Currency2 { get; set; }
        public virtual Currency Currency3 { get; set; }
        public virtual Currency_Pair Currency_Pair { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Exchange Exchange { get; set; }
        public virtual Exchange Exchange1 { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual ICollection<Security_Price_History> Security_Price_History { get; set; }
        public virtual ICollection<Security_Price> Security_Price { get; set; }
    }
}
