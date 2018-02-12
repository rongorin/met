using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Metope.DAL;

namespace MetopeMVCApp.Models
{
    public class PositionsSODIndexViewModel
    {
        //o	Ticker
        //public Security_Detail SecurityDetails { get; set; }
        //public IEnumerable<Security_Price> PositionsSOD { get; set; }

        public string Portfolio_Code { get; set; }
        public string Ticker { get; set; } 

        public decimal Entity_ID { get; set; }
        public decimal Security_ID { get; set; }
        public string Long_Short_Indicator { get; set; }
        public System.DateTime Position_Date { get; set; }
        public Nullable<decimal> Dealt_Quantity { get; set; }
        public Nullable<decimal> Dealt_AllIn_Mkt_Value_BaseCur { get; set; } 
  

    }
}