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
    [MetadataType(typeof(SecurityListDetailModelMetaData))]
    public partial class Security_List_Detail
    {
        public decimal Entity_ID { get; set; }
        public string Security_List_Code { get; set; }
        public string Security_List_Name { get; set; }
        public string Description { get; set; }
        public bool System_Locked { get; set; }
    
        public virtual Entity Entity { get; set; }

    }
}