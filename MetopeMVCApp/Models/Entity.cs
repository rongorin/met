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
            this.Users = new HashSet<User>();
            this.Portfolio_List = new HashSet<Portfolio_List>();
            this.Exchanges = new HashSet<Exchange>();
            this.Security_Type = new HashSet<Security_Type>();
            this.Code_Miscellaneous = new HashSet<Code_Miscellaneous>();
            this.Portfolios = new HashSet<Portfolio>();
            this.Parties = new HashSet<Party>();
            this.Security_Price = new HashSet<Security_Price>();
            this.Security_Price_History = new HashSet<Security_Price_History>();
            this.Security_Detail = new HashSet<Security_Detail>();
            this.Party_Debt_Analysis = new HashSet<Party_Debt_Analysis>();
            this.Party_Financials = new HashSet<Party_Financials>();
            this.Debt_Expiry_Profile = new HashSet<Debt_Expiry_Profile>();
            this.Party_Financials_History = new HashSet<Party_Financials_History>();
        }
    
        public decimal Entity_ID { get; set; }
        public string Entity_Code { get; set; }
        public string Entity_Name { get; set; }
        public string Import_Folder { get; set; }
        public string Export_Folder { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Portfolio_List> Portfolio_List { get; set; }
        public virtual ICollection<Exchange> Exchanges { get; set; }
        public virtual ICollection<Security_Type> Security_Type { get; set; }
        public virtual ICollection<Code_Miscellaneous> Code_Miscellaneous { get; set; }
        public virtual ICollection<Portfolio> Portfolios { get; set; }
        public virtual ICollection<Party> Parties { get; set; }
        public virtual ICollection<Security_Price> Security_Price { get; set; }
        public virtual ICollection<Security_Price_History> Security_Price_History { get; set; }
        public virtual ICollection<Security_Detail> Security_Detail { get; set; }
        public virtual ICollection<Party_Debt_Analysis> Party_Debt_Analysis { get; set; }
        public virtual ICollection<Party_Financials> Party_Financials { get; set; }
        public virtual ICollection<Debt_Expiry_Profile> Debt_Expiry_Profile { get; set; }
        public virtual ICollection<Party_Financials_History> Party_Financials_History { get; set; }
    }
}
