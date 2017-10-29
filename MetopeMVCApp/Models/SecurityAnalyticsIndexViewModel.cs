using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SecurityAnalyticsIndexViewModel
    {
        public decimal Security_ID { get; set; }
        public decimal Entity_ID { get; set; }
        public Nullable<decimal> Issued_Amount { get; set; } 
        public Nullable<decimal> Earnings_Forecast_Yr1 { get; set; }
        public Nullable<decimal> Earnings_Forecast_Yr2 { get; set; }
        public Nullable<decimal> Earnings_Forecast_Yr3 { get; set; }
        public Nullable<decimal> Total_Return_ME_1YR { get; set; }
        public Nullable<decimal> Total_Return_ME_2YR { get; set; }
        public Nullable<decimal> Total_Return_ME_3YR { get; set; } 
        public string Short_Name { get; set; }
        public string Ticker { get; set; }
        
        //public Security_Detail SecurityDetails { get; set; }
    }
}