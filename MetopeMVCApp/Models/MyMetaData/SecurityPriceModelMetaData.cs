using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;


namespace MetopeMVCApp.Models.MyMetaData
{
    [MetadataType(typeof(SecurityPriceModelMetaData))]

    public class SecurityPriceModelMetaData
    {
        [Required]
        [Display(Name = "Security")]
        public object Security_ID { get; set; }

        [Required]
        [Display(Name = "Price Currency")]
        public object Price_Curr { get; set; }
          
        [Display(Name = "All In Price")]
        public object All_In_Price { get; set; }

        [Display(Name = "Clean Price")]
        public object Clean_Price { get; set; }

        [Display(Name = "Accrued Income Price")]
        public object Accrued_Income_Price { get; set; }
           [Required]
        [Display(Name = "Price Source")]
        public object Price_Source { get; set; }

        [Display(Name = "Yield To Maturity")]
        public object Yield_To_Maturity { get; set; }

        [Display(Name = "Discount Rate")]
        public object Discount_Rate { get; set; }

        [Display(Name = "Issued Amount")]
        public object Issued_Amount { get; set; }

        [Display(Name = "Free Float Issued Amount")]
        public object Free_Float_Issued_Amount { get; set; }
           [Required]
        [Display(Name = "Record Date")]   
        public object Record_Date { get; set; }
    }
}