using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class CashTransactionIndexViewModel
    {
        public decimal Cash_Transaction_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public decimal Entity_ID { get; set; }
        public Nullable<decimal> Transaction_Security_ID { get; set; }
        public decimal Cash_Security_ID { get; set; }
        public string Transaction_Source_Code { get; set; } 
        public System.DateTime Transaction_Date { get; set; }
        public System.DateTime Value_Date { get; set; }
        public string Cash_Transaction_Type { get; set; }
        public decimal Transaction_Amount { get; set; }
        public string Transaction_Currency_Code { get; set; }
        public decimal BaseCur_Amount { get; set; } 
 
         
    }
}