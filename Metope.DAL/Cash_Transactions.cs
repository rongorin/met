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
    [MetadataType(typeof(CashTransactionsModelMetatData))]
    
    public partial class Cash_Transactions
    {
        public decimal Cash_Transaction_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public Nullable<decimal> Transaction_Security_ID { get; set; }
        public decimal Cash_Security_ID { get; set; }
        public string Transaction_Source_Code { get; set; }
        public Nullable<decimal> Order_ID { get; set; }
        public Nullable<decimal> Allocation_ID { get; set; }
        public Nullable<decimal> Cashflow_ID { get; set; }
        public Nullable<decimal> Dividend_Seq_Number { get; set; }
        public string External_ID { get; set; }
        public System.DateTime Transaction_Date { get; set; }
        public System.DateTime Value_Date { get; set; }
        public string Cash_Transaction_Type { get; set; }
        public decimal Transaction_Amount { get; set; }
        public string Transaction_Currency_Code { get; set; }
        public decimal BaseCur_Amount { get; set; }
        public string Create_User_Code { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Last_Update_User { get; set; }
        public Nullable<System.DateTime> Last_Update_Date { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Order_Detail Order_Detail { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual Order_Allocation Order_Allocation { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }
        public virtual Security_Detail Security_Detail1 { get; set; }
    }
}
