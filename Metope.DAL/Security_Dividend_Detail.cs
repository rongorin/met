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
    using Metope.DAL.MyMetaData;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(SecurityDividendDetailModelMetaData))]

    public partial class Security_Dividend_Detail : IValidatableObject
    {
        public decimal Entity_ID { get; set; }
        public decimal Security_ID { get; set; }
        public decimal Dividend_Seq_Number { get; set; }
        public Nullable<int> Dividend_Annual_Number { get; set; }
        public Nullable<System.DateTime> Forecast_Dividend_Payment_Date { get; set; }
        public string Dividend_Currency_Code { get; set; }
        public Nullable<System.DateTime> Actual_Dividend_Payment_Date { get; set; }
        public Nullable<System.DateTime> Actual_Last_Date_To_Register { get; set; }
        public Nullable<System.DateTime> Actual_Ex_Dividend_Date { get; set; }
        public decimal Dividend_Split { get; set; }
        public Nullable<decimal> Forecast_Dividend { get; set; }
        public Nullable<decimal> Actual_Dividend { get; set; }
        public string Dividend_Type { get; set; }
        public Nullable<System.DateTime> Forecast_Last_Date_to_Register { get; set; }
        public Nullable<System.DateTime> Forecast_Ex_Dividend_Date { get; set; }
        public System.DateTime Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
        public short Financial_Year { get; set; }
        public Nullable<decimal> Actual_FX_Rate { get; set; }
        public Nullable<bool> Lock_Flag { get; set; }
        public Nullable<decimal> Actual_NonRecurring_Dividend { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User User { get; set; }
        public virtual Security_Detail Security_Detail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {   //if any of the actual values are filled in, then all must be filled in, otherwise fail validation
            if (Actual_Dividend != null || Actual_Dividend_Payment_Date != null || Actual_Ex_Dividend_Date != null || Actual_Last_Date_To_Register != null)
            {

                if (Actual_Dividend == null || Actual_Dividend_Payment_Date == null || Actual_Ex_Dividend_Date == null || Actual_Last_Date_To_Register == null)
                {
                    yield return new ValidationResult("Must fill in all the Actual values");
                }


            }
        }
    }
}
