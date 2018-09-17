using Metope.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SecurityDividendDetailViewModel
    {
        public string Security_Name { get; set; }
        public Security_Detail SecurityDetails { get; set; }

        public IEnumerable<Security_Dividend_Detail> SecurityDividendDetail { get; set; }

    }
}