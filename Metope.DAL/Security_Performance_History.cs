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
    
    public partial class Security_Performance_History
    {
        public decimal Security_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public Nullable<decimal> ModDietz_Performance_Quarterly { get; set; }
        public Nullable<decimal> ModDietz_Performance_Monthly { get; set; }
        public Nullable<decimal> ModDietz_Performance_MonthToDate { get; set; }
        public Nullable<decimal> Dietz_Performance_Quarterly { get; set; }
        public Nullable<decimal> Dietz_Performance_Monthly { get; set; }
        public Nullable<decimal> Dietz_Performance_MonthToDate { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
        public System.DateTime Record_Date { get; set; }
        public decimal Session_ID { get; set; }
        public Nullable<System.DateTime> Hist_Last_Update_Date { get; set; }
        public string Hist_Last_Update_User { get; set; }
    
        public virtual Entity Entity { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual User User { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }
    }
}
