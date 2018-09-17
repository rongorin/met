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
    
    public partial class Order_Detail
    {
        public Order_Detail()
        {
            this.Cash_Transactions = new HashSet<Cash_Transactions>();
            this.Order_Allocation = new HashSet<Order_Allocation>();
        }
    
        public decimal Order_ID { get; set; }
        public decimal Entity_ID { get; set; }
        public decimal Security_ID { get; set; }
        public System.DateTime Trade_Date { get; set; }
        public System.DateTime Settlement_Date { get; set; }
        public string Transaction_Type { get; set; }
        public string Order_Status { get; set; }
        public string Cancel_Reason_Code { get; set; }
        public string Manager { get; set; }
        public string Trader { get; set; }
        public string Operations_User { get; set; }
        public string Order_Instruction { get; set; }
        public string Order_Duration { get; set; }
        public Nullable<decimal> Limit_Price { get; set; }
        public Nullable<decimal> Limit_Yield { get; set; }
        public Nullable<decimal> Limit_Discount { get; set; }
        public string Order_Creator { get; set; }
        public System.DateTime Order_Create_Date { get; set; }
        public Nullable<System.DateTime> Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
        public Nullable<System.DateTime> Sent_To_Trading_Date { get; set; }
        public Nullable<System.DateTime> Sent_To_Accounting_Date { get; set; }
        public bool Value_Based_Indicator { get; set; }
        public Nullable<decimal> Target_Clean_Price { get; set; }
        public Nullable<decimal> Target_Income_Price { get; set; }
        public Nullable<decimal> Target_AllIn_Price { get; set; }
        public Nullable<decimal> Target_Yield { get; set; }
        public Nullable<decimal> Target_Discount { get; set; }
        public Nullable<decimal> Target_Quantity { get; set; }
        public Nullable<decimal> Target_Clean_Amount_TradeCur { get; set; }
        public Nullable<decimal> Target_Income_Amount_TradeCur { get; set; }
        public Nullable<decimal> Target_AllIn_Amount_TradeCur { get; set; }
        public Nullable<decimal> Place_Quantity { get; set; }
        public Nullable<decimal> Execution_Quantity { get; set; }
        public Nullable<decimal> Execution_Clean_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_Income_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_AllIn_Amount_TradeCur { get; set; }
        public string Broker_Code { get; set; }
        public Nullable<decimal> Execution_Clean_Price { get; set; }
        public Nullable<decimal> Execution_Income_Price { get; set; }
        public Nullable<decimal> Execution_AllIn_Price { get; set; }
        public Nullable<decimal> Execution_Yield { get; set; }
        public Nullable<decimal> Execution_Discount { get; set; }
        public string Execution_Currency { get; set; }
        public string Execution_Exchange { get; set; }
        public string Target_FX_Rate { get; set; }
        public string Limit_FX_Rate { get; set; }
        public string Execution_FX_Rate { get; set; }
        public string Buy_Currency_Code { get; set; }
        public string Buy_Currency_Execution_Amount { get; set; }
        public string Buy_Currency_Target_Amount { get; set; }
        public string Sell_Currency_Code { get; set; }
        public Nullable<decimal> Sell_Currency_Execution_Amount { get; set; }
        public Nullable<decimal> Sell_Currency_Target_Amount { get; set; }
        public Nullable<decimal> Commission_Amount_TradeCur { get; set; }
        public string Trade_Type_Indicator { get; set; }
        public Nullable<decimal> Commission_Rate { get; set; }
        public string Commission_Type { get; set; }
        public string Fee_Code1 { get; set; }
        public string Fee_Code2 { get; set; }
        public string Fee_Code3 { get; set; }
        public string Fee_Code4 { get; set; }
        public string Fee_Code5 { get; set; }
        public string Fee_Code6 { get; set; }
        public Nullable<decimal> Fee_Amount1_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount2_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount3_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount4_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount5_TradeCur { get; set; }
        public Nullable<decimal> Fee_Amount6_TradeCur { get; set; }
        public Nullable<decimal> Tax_Amount_TradeCur { get; set; }
        public string Tax_Type { get; set; }
        public string Tax_Charge_FeeNum { get; set; }
        public Nullable<decimal> Execution_Gross_Amount_TradeCur { get; set; }
        public Nullable<decimal> Execution_Net_Amount_TradeCur { get; set; }
        public Nullable<decimal> Related_Order_ID { get; set; }
        public Nullable<decimal> Previous_Order_ID { get; set; }
        public string Authorised_User1 { get; set; }
        public Nullable<System.DateTime> Authorised_User1_DateTime { get; set; }
        public string Authorised_User2 { get; set; }
        public Nullable<System.DateTime> Authorised_User2_DateTime { get; set; }
        public string Authorised_Number { get; set; }
        public string Security_Type_Code { get; set; }
        public string Export_Status { get; set; }
        public string Order_Ack_Nack_Status { get; set; }
        public string Operations_Status { get; set; }
        public decimal Import_Trade_ID { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
        public virtual Currency Currency2 { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Exchange Exchange { get; set; }
        public virtual Security_Type Security_Type { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual User User3 { get; set; }
        public virtual User User4 { get; set; }
        public virtual User User5 { get; set; }
        public virtual ICollection<Cash_Transactions> Cash_Transactions { get; set; }
        public virtual ICollection<Order_Allocation> Order_Allocation { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }
    }
}