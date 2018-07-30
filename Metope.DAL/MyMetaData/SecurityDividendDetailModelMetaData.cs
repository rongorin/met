using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using MetopeMVCApp.Validators;

namespace Metope.DAL.MyMetaData
{
    [MetadataType(typeof(SecurityDividendDetailModelMetaData))]
    public class SecurityDividendDetailModelMetaData
    {
        [Required]
        [ValidatePriceCurr]
        public object Dividend_Currency_Code { get; set; }

        [Required]
        [MaxLength(1, ErrorMessage = "Value for {0} can be only one character long")]
        public object Dividend_Type { get; set; }
        [Required]
        [Range(0.05, 10, ErrorMessage = "Value for {0} must be less than {2}")]
        public object Dividend_Split { get; set; }

        [RegularExpression("[0-9]{1,}", ErrorMessage = "Value for {0} must be an integer")]
        public object Dividend_Seq_Number { get; set; }


    }
}
 