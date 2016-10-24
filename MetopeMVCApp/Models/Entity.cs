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
    
    public partial class Entity
    {
        public Entity()
        {
            this.Portfolios = new HashSet<Portfolio>();
            this.Users = new HashSet<User>();
            this.Portfolio_List = new HashSet<Portfolio_List>();
            this.Security_Detail = new HashSet<Security_Detail>();
            this.Exchanges = new HashSet<Exchange>();
            this.Security_Type = new HashSet<Security_Type>();
            this.Code_Miscellaneous = new HashSet<Code_Miscellaneous>();
            this.Parties = new HashSet<Party>();
        }
    
        public decimal Entity_ID { get; set; }
        public string Entity_Code { get; set; }
        public string Entity_Name { get; set; }
        public string Import_Folder { get; set; }
        public string Export_Folder { get; set; }
    
        public virtual ICollection<Portfolio> Portfolios { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Portfolio_List> Portfolio_List { get; set; }
        public virtual ICollection<Security_Detail> Security_Detail { get; set; }
        public virtual ICollection<Exchange> Exchanges { get; set; }
        public virtual ICollection<Security_Type> Security_Type { get; set; }
        public virtual ICollection<Code_Miscellaneous> Code_Miscellaneous { get; set; }
        public virtual ICollection<Party> Parties { get; set; }
    }
}
