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
    //using MetopeMVCApp.Models.MyMetaData;
    //using System.ComponentModel.DataAnnotations;
    //[MetadataType(typeof(PartyFinancialsModelMetaData))]
    
    public partial class Party_Financials_History
    {
        public string Party_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public string Actual_Forecast_Indicator { get; set; }
        public Nullable<decimal> Security_ID { get; set; }
        public System.DateTime Financials_Date { get; set; }
        public Nullable<decimal> Full_Adjusted_Share_NAV { get; set; }
        public Nullable<decimal> Enterprise_Value { get; set; }
        public Nullable<decimal> Dividend_Yield { get; set; }
        public Nullable<decimal> Net_Present_Value { get; set; }
        public Nullable<decimal> Net_Property_Income { get; set; }
        public Nullable<decimal> Vacancies_Overall { get; set; }
        public Nullable<decimal> Portfolio_Leases_Expiring { get; set; }
        public Nullable<decimal> Assumed_Ave_Escalation_Rate { get; set; }
        public Nullable<decimal> Cap_Rate_Movemt_Assump { get; set; }
        public Nullable<decimal> Reversion_Exp_Rent_Reviews { get; set; }
        public Nullable<decimal> Ungeared_Yield { get; set; }
        public Nullable<decimal> Net_Operating_Profit { get; set; }
        public Nullable<decimal> Revenue_Growth { get; set; }
        public Nullable<decimal> Net_Property_Income_Growth { get; set; }
        public Nullable<decimal> Net_Operating_Profit_Growth { get; set; }
        public Nullable<decimal> Interest_Paid_Growth { get; set; }
        public Nullable<decimal> Loan_To_Value { get; set; }
        public Nullable<decimal> Interest_Cover_Ratio { get; set; }
        public Nullable<decimal> Revenue { get; set; }
        public Nullable<decimal> Interest_Paid { get; set; }
        public Nullable<decimal> Property_Expenses { get; set; }
        public Nullable<decimal> Property_Expenses_Growth { get; set; }
        public Nullable<decimal> Investment_Income { get; set; }
        public Nullable<decimal> Admin_Expenses { get; set; }
        public Nullable<decimal> Admin_Expenses_Growth { get; set; }
        public Nullable<decimal> Taxation { get; set; }
        public Nullable<decimal> Other_Income { get; set; }
        public Nullable<decimal> Distributable_Earnings { get; set; }
        public Nullable<decimal> Distributions_Growth { get; set; }
        public Nullable<decimal> Linked_Units_Issued { get; set; }
        public Nullable<decimal> Borrowings { get; set; }
        public Nullable<decimal> Units_Issued { get; set; }
        public Nullable<decimal> Purchases_And_Developments { get; set; }
        public Nullable<decimal> Sales { get; set; }
        public Nullable<decimal> Total_Assets { get; set; }
        public Nullable<decimal> Current_Liabilities { get; set; }
        public Nullable<decimal> Interest_Bearing_Borrowings { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
        public Nullable<decimal> Forecast_Distributions_Growth { get; set; }
        public System.DateTime Record_Date { get; set; }
        public decimal Session_ID { get; set; }
        public Nullable<System.DateTime> Hist_Last_Update_Date { get; set; }
        public string Hist_Last_Update_User { get; set; }
        public decimal Property_Portfolio_Value { get; set; }
    
        public virtual Entity Entity { get; set; }
    }
}
