using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SelectOrderAllocEditorViewModel
    {
        public bool Selected { get; set; }

        public decimal Entity_ID { get; set; }
        public decimal Security_ID { get; set; } //doubt we need these

        public decimal Allocation_ID { get; set; } //key for OrderAllocation
        public decimal Order_ID { get; set; } // key for related OrderDetail

        public string Ticker { get; set; } //Security_Detail
        public string Transaction_Type { get; set; } //Order_Detail
        public System.DateTime Trade_Date { get; set; } //Order_Detail

        public string Portfolio_Code { get; set; }//Order_Allocation
        public Nullable<decimal> Execution_Quantity { get; set; }
        public Nullable<decimal> Execution_AllIn_Price { get; set; } // 9 decimals
        public Nullable<decimal> Execution_Net_Amount_TradeCur { get; set; } // 4 decimals
    }
}