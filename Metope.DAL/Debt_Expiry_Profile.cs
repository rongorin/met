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
    [MetadataType(typeof(DebtExpiryProfileModelMetaData))]
    public partial class Debt_Expiry_Profile
    {
        public string Party_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public System.DateTime Record_Date { get; set; }
        public System.DateTime Financial_Year_End { get; set; }
        public Nullable<decimal> Expiring_Debt_Amount { get; set; }
    
        public virtual Entity Entity { get; set; }
    }
}
