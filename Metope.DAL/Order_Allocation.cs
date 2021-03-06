//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Metope.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order_Allocation
    {
        public Order_Allocation()
        {
            this.Cash_Transactions = new HashSet<Cash_Transactions>();
        }
    
        public decimal Allocation_ID { get; set; }
        public decimal Entity_ID { get; set; }
        public decimal Order_ID { get; set; }
        public string Portfolio_Code { get; set; }
        public decimal Target_Quantity { get; set; }
        public Nullable<decimal> Target_Clean_Amount_TradeCur { get; set; }
        public Nullable<decimal> Target_Income_Amount_TradeCur { get; set; }
        public Nullable<decimal> Target_AllIn_Amount_TradeCur { get; set; }
        public Nullable<decimal> Target_Clean_Amount_BaseCur { get; set; }
        public Nullable<decimal> Target_Income_Amount_BaseCur { get; set; }
        public Nullable<decimal> Target_AllIn_Amount_BaseCur { get; set; }
        public Nullable<decimal> Execution_Quantity { get; set; }
        public Nullable<decimal> Place_Quantity { get; set; }
        public Nullable<decimal> Execution_Clean_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_Income_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_AllIn_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_Clean_Amount_BaseCur { get; set; }
        public Nullable<decimal> Execution_Income_Amount_BaseCur { get; set; }
        public Nullable<decimal> Execution_AllIn_Amount_BaseCur { get; set; }
        public Nullable<decimal> Commission_Rate { get; set; }
        public string Commission_Type { get; set; }
        public Nullable<decimal> Commission_Amount_TradeCur { get; set; }
        public Nullable<decimal> Commission_Amount_BaseCur { get; set; }
        public Nullable<decimal> Execution_Gross_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_Gross_Amount_BaseCur { get; set; }
        public Nullable<decimal> Execution_Net_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_Net_Amount_BaseCur { get; set; }
        public Nullable<decimal> Buy_Currency_Target_Amount_TradeCur { get; set; }
        public Nullable<decimal> Sell_Currency_Target_Amount_TradeCur { get; set; }
        public Nullable<decimal> Buy_Currency_Target_Amount_BaseCur { get; set; }
        public Nullable<decimal> Sell_Currency_Target_Amount_BaseCur { get; set; }
        public Nullable<decimal> Buy_Currency_Execution_Amount_TradeCur { get; set; }
        public Nullable<decimal> Sell_Currency_Execution_Amount_TradeCur { get; set; }
        public Nullable<decimal> Buy_Currency_Execution_Amount_BaseCur { get; set; }
        public Nullable<decimal> Sell_Currency_Execution_Amount_BaseCur { get; set; }
        public Nullable<decimal> Fee_Amount1_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount2_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount3_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount4_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount5_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount6_TradeCur { get; set; }
        public Nullable<decimal> Tax_Amount_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount1_BaseCur { get; set; }
        public Nullable<decimal> Fee_Amount2_BaseCur { get; set; }
        public Nullable<decimal> Fee_Amount3_BaseCur { get; set; }
        public Nullable<decimal> Fee_Amount4_BaseCur { get; set; }
        public Nullable<decimal> Fee_Amount5_BaseCur { get; set; }
        public Nullable<decimal> Fee_Amount6_BaseCur { get; set; }
        public Nullable<decimal> Tax_Amount_BaseCur { get; set; }
        public Nullable<decimal> Trade_Base_FX_Rate { get; set; }
        public string Export_Reference { get; set; }
        public string Export_Status { get; set; }
        public string Allocation_Ack_Nack_Status { get; set; }
        public string Operations_Status { get; set; }
        public Nullable<System.DateTime> Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
    
        public virtual ICollection<Cash_Transactions> Cash_Transactions { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Order_Detail Order_Detail { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual User User { get; set; }
    }
}
