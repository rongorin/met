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
    [MetadataType(typeof(SecurityClassificationIndustryModelMetaData))]
    
    public partial class Security_Classification_Industry
    {
        public string Classification_Code { get; set; }
        public System.DateTime Effective_Date { get; set; }
        public decimal Entity_ID { get; set; }
        public string Industry_Code { get; set; }
        public Nullable<System.DateTime> Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
        public decimal Security_ID { get; set; }
    
        public virtual Classification Classification { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }
        public virtual User User { get; set; }
    }
}
