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
    
    public partial class Currency_Pair
    {
        public Currency_Pair()
        {
            this.Security_Detail = new HashSet<Security_Detail>();
        }
    
        public string Currency_Pair_Code { get; set; }
        public string Base_Currency_Code { get; set; }
        public string Counter_Currency_Code { get; set; }
        public int Calculation_Precision { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
        public virtual ICollection<Security_Detail> Security_Detail { get; set; }
    }
}