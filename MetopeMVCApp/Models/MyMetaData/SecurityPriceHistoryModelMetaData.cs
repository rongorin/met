using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models.MyMetaData
{
    [MetadataType(typeof(SecurityPriceHistoryModelMetaData))]

    public class SecurityPriceHistoryModelMetaData
    { 
        [Required]
        [Display(Name = "Security")]  
        public object Security_ID { get; set; }

        [Required]
        [Display(Name = "Price Curr")]
        public object Price_Curr { get; set; }

        [Display(Name = "All In Price")]
        [Range(0, 999999999)]
        public object All_In_Price { get; set; }

        [Display(Name = "Clean Price")]
        [Range(0, 999999999)]
        public object Clean_Price { get; set; }

        [Display(Name = "Accrued Income Price")]
        [Range(0, 999999999)]
        public object Accrued_Income_Price { get; set; }

        [Required]
        [Display(Name = "Price Source")]
        public object Price_Source { get; set; }

        [Display(Name = "Yield To Maturity")]
        [Range(0, 999999999)]
        public object Yield_To_Maturity { get; set; }

        [Display(Name = "Discount Rate")]
        [Range(0, 999999999)]
        public object Discount_Rate { get; set; }
         
        [Display(Name = "Price DateTime")]
        public object Price_DateTime { get; set; }

        [Display(Name = "Issued Amount")]
        [Range(0, 999999999999999)]
        public object Issued_Amount { get; set; }

        [Display(Name = "Free Float Issued Amount")]  
        [Range(0, 999999999999999)]
        public object Free_Float_Issued_Amount { get; set; } 
    }
}