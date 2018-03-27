using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SecurityAttributionIndexViewModel
    {
        public decimal Entity_ID { get; set; }
        public decimal Security_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public string Ticker { get; set; }

        public Nullable<decimal> Excess_Weight_Quarterly { get; set; }
        public Nullable<decimal> Relative_Contribution_Quarterly { get; set; }
        public Nullable<decimal> Security_Weight_Portfolio_Monthly { get; set; }
    }
}