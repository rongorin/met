using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SecurityPerformanceIndexViewModel
    {   
            public decimal Entity_ID { get; set; } 
            public string Portfolio_Code { get; set; } 
            public decimal Security_ID { get; set; }
            public string Ticker { get; set; }
            public DateTime RecordDate { get; set; }
          
            public Nullable<decimal> ModDietz_Performance_Quarterly { get; set; }
            public Nullable<decimal> ModDietz_Performance_Monthly { get; set; }
            public Nullable<decimal> ModDietz_Performance_MonthToDate { get; set; } 
    }
}