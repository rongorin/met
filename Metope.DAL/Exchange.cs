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
    
    public partial class Exchange
    {
        public Exchange()
        {
            this.Security_Detail = new HashSet<Security_Detail>();
            this.Security_Detail1 = new HashSet<Security_Detail>();
            this.Order_Detail = new HashSet<Order_Detail>();
        }
    
        public string Exchange_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public string Exchange_Name { get; set; }
        public string Country_Code { get; set; }
        public string ISO_Exchange_Code { get; set; }
        public string Exchange_Type { get; set; }
        public Nullable<bool> system_locked { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual ICollection<Security_Detail> Security_Detail { get; set; }
        public virtual ICollection<Security_Detail> Security_Detail1 { get; set; }
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
    }
}
