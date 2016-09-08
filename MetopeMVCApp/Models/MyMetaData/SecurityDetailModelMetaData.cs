using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MetopeMVCApp.Models.MyMetaData

{

    public class SecurityDetailModelMetaData
    {
        [Required]
        [Display(Name = "Security Name")] 
        public object Security_Name { get; set; }

        [Required]
       // [RegularExpression(@"\d{6,10}", ErrorMessage = "Account # must be between 6 and 10 digits.")]
        [Display(Name = "Sec Type Code")]
        [StringLength(20, ErrorMessage =
                 "Security Type Code should be max 10 characters ")] 
        public object Security_Type_Code { get; set; }

        [Display(Name = "Issue Date")]
        public object Issue_Date { get; set; }
        [Display(Name = "Price Multiply")]
        public object Price_Multiplier { get; set; }
        [Display(Name = "2ndry Exchange")]
        public object Secondary_Exch { get; set; }
    }
}