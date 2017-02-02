using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SecurityPriceIndexViewModel
    {
        public IEnumerable<Security_Detail> SecurityDetails { get; set; }
        public IEnumerable<Security_Price> SecurityPrices { get; set; }
        public IEnumerable<Security_Price_History> SecurityPriceHistory { get; set; }
    }
}