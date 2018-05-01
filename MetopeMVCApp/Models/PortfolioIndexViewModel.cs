using Metope.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class PortfolioIndexViewModel
    {
        public decimal Entity_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public string Portfolio_Name { get; set; }
        public Nullable<System.DateTime> Financial_Year_End { get; set; }
        public bool Active_Flag { get; set; }
        //public string Manager { get; set; }
        //public string Portfolio_Type { get; set; }
        //public string Portfolio_Base_Currency { get; set; }
        //public string PortfolIo_Domicile { get; set; }
        //public string Portfolio_Report_Currency { get; set; }
        //public Nullable<System.DateTime> Inception_Date { get; set; }
        //public string Custodian_Code { get; set; }
        //public Nullable<bool> System_Locked { get; set; }
        //public string Portfolio_Status { get; set; }   
        public string HasValuation { get; set; }
        public Portfolio_Valuation Portfolio_Valuation{ get; set; } 


    }
}