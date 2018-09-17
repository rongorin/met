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
    
    public partial class Security_Attribution_History
    {
        public decimal Security_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public Nullable<decimal> Excess_Weight_Quarterly { get; set; }
        public Nullable<decimal> Relative_Contribution_Quarterly { get; set; }
        public Nullable<decimal> Allocation_Selection_Interaction_Quarterly { get; set; }
        public Nullable<decimal> StockVs_TotBenchmarkReturn_Quarterly { get; set; }
        public Nullable<decimal> Selection_Interaction_Quarterly { get; set; }
        public Nullable<decimal> Asset_Allocation_Quarterly { get; set; }
        public Nullable<decimal> Excess_Weight_Monthly { get; set; }
        public Nullable<decimal> Relative_Contribution_Monthly { get; set; }
        public Nullable<decimal> Allocation_Selection_Interaction_Monthly { get; set; }
        public Nullable<decimal> StockVs_TotBenchmarkReturn_Monthly { get; set; }
        public Nullable<decimal> Selection_Interaction_Monthly { get; set; }
        public Nullable<decimal> Asset_Allocation_Monthly { get; set; }
        public Nullable<decimal> Excess_Weight_MonthToDate { get; set; }
        public Nullable<decimal> Relative_Contribution_MonthToDate { get; set; }
        public Nullable<decimal> Allocation_Selection_Interaction_MonthToDate { get; set; }
        public Nullable<decimal> StockVs_TotBenchmarkReturn_MonthToDate { get; set; }
        public Nullable<decimal> Selection_Interaction_MonthToDate { get; set; }
        public Nullable<decimal> Asset_Allocation_MonthToDate { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
        public Nullable<decimal> Security_Weight_Portfolio_Monthly { get; set; }
        public Nullable<decimal> Security_Weight_Portfolio_MonthToDate { get; set; }
        public Nullable<decimal> Security_Weight_Portfolio_Quarterly { get; set; }
        public Nullable<decimal> Security_Weight_Benchmark_Monthly { get; set; }
        public Nullable<decimal> Security_Weight_Benchmark_MonthToDate { get; set; }
        public Nullable<decimal> Security_Weight_Benchmark_Quarterly { get; set; }
        public Nullable<decimal> Total_Return_Security_Price_Monthly { get; set; }
        public Nullable<decimal> Total_Return_Security_Price_MonthToDate { get; set; }
        public Nullable<decimal> Total_Return_Security_Price_Quarterly { get; set; }
        public Nullable<decimal> TW_Return_Portfolio_Monthly { get; set; }
        public Nullable<decimal> TW_Return_Portfolio_MonthToDate { get; set; }
        public Nullable<decimal> TW_Return_Portfolio_Quarterly { get; set; }
        public System.DateTime Record_Date { get; set; }
        public decimal Session_ID { get; set; }
        public Nullable<System.DateTime> Hist_Last_Update_Date { get; set; }
        public string Hist_Last_Update_User { get; set; }
        public Nullable<decimal> Benchmark_Price_Performance_Monthly { get; set; }
        public Nullable<decimal> Benchmark_Price_Performance_MonthToDate { get; set; }
        public Nullable<decimal> Benchmark_Price_Performance_Quarterly { get; set; }
    
        public virtual Entity Entity { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }
        public virtual User User { get; set; }
    }
}
