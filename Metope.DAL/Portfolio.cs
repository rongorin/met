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
    [MetadataType(typeof(PortfolioModelMetatData))]
    
    public partial class Portfolio
    {
        public Portfolio()
        {
            this.Portfolio_List = new HashSet<Portfolio_List>();
            this.Portfolio_Valuation_History = new HashSet<Portfolio_Valuation_History>();
            this.Position_SOD = new HashSet<Position_SOD>();
            this.Cash_Transactions = new HashSet<Cash_Transactions>();
            this.Security_Performance_History = new HashSet<Security_Performance_History>();
            this.Security_Performance = new HashSet<Security_Performance>();
            this.Order_Allocation = new HashSet<Order_Allocation>();
            this.Security_Detail = new HashSet<Security_Detail>();
        }
    
        public decimal Entity_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public string Portfolio_Name { get; set; }
        public string Manager { get; set; }
        public string Portfolio_Type { get; set; }
        public string Portfolio_Base_Currency { get; set; }
        public string PortfolIo_Domicile { get; set; }
        public string Portfolio_Report_Currency { get; set; }
        public Nullable<System.DateTime> Inception_Date { get; set; }
        public Nullable<System.DateTime> Financial_Year_End { get; set; }
        public string Custodian_Code { get; set; }
        public bool Active_Flag { get; set; }
        public Nullable<bool> System_Locked { get; set; }
        public string Portfolio_Status { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
        public virtual Currency Currency2 { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual ICollection<Portfolio_List> Portfolio_List { get; set; }
        public virtual User User { get; set; }
        public virtual Portfolio_Valuation Portfolio_Valuation { get; set; }
        public virtual ICollection<Portfolio_Valuation_History> Portfolio_Valuation_History { get; set; }
        public virtual ICollection<Position_SOD> Position_SOD { get; set; }
        public virtual ICollection<Cash_Transactions> Cash_Transactions { get; set; }
        public virtual ICollection<Security_Performance_History> Security_Performance_History { get; set; }
        public virtual ICollection<Security_Performance> Security_Performance { get; set; }
        public virtual ICollection<Order_Allocation> Order_Allocation { get; set; }
        public virtual ICollection<Security_Detail> Security_Detail { get; set; }
    }
}
