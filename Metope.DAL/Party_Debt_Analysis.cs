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
    using System;
    using System.Collections.Generic;
    using Metope.DAL.MyMetaData;
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(PartyDebtAnalysisModelMetaData))]
    
    public partial class Party_Debt_Analysis
    {
        public string Party_Code { get; set; }
        public string Financials_Type { get; set; }
        public decimal Entity_ID { get; set; }
        public System.DateTime Financials_Date { get; set; }
        public Nullable<decimal> Investment_Properties { get; set; }
        public Nullable<decimal> Other_Investments { get; set; }
        public Nullable<decimal> Total_Investments { get; set; }
        public Nullable<decimal> Weighted_Investments { get; set; }
        public Nullable<decimal> ST_Interest_Bearing_Debt { get; set; }
        public Nullable<decimal> LT_Interest_Bearing_Debt { get; set; }
        public Nullable<decimal> Total_Interest_Bearing_Borrowings { get; set; }
        public Nullable<decimal> Debt_Hedged_Amount { get; set; }
        public Nullable<decimal> Debt_Hedged_Percent { get; set; }
        public Nullable<decimal> Floating_Debt_Amount { get; set; }
        public Nullable<decimal> Floating_Debt_Percent { get; set; }
        public Nullable<decimal> Total_Weighted_Debt_Cost { get; set; }
        public Nullable<decimal> Capital_Markets_Debt { get; set; }
        public Nullable<decimal> Traditional_Bank_Debt { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
    
        public virtual Entity Entity { get; set; }
    }
}
