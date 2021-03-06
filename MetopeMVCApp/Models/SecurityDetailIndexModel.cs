﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Metope.DAL;
namespace MetopeMVCApp.Models
{
    public class SecurityDetailIndexModel
    {
        public decimal Security_ID { get; set; }
        public decimal Entity_ID { get; set; } 
        public string Security_Type_Code { get; set; }
        public string Security_Name { get; set; }
        public string Short_Name { get; set; }
        public decimal? Current_Market_Price { get; set; }
        public string Ticker { get; set; }
        public Nullable< DateTime> Maturity_Date { get; set; }
        public string Primary_Exch { get; set; }
        public string  Security_Status { get; set; }
        public int? NumberOfRows { get; set; }
        public int? HasAnalystics { get; set; } 
        public Security_Analytics Analytics { get; set; }
    }
}