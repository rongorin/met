using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MetopeMVCApp.Models.MyMetaData
{
        [MetadataType(typeof(SecurityDividendDetailModelMetaData))]
    public class SecurityDividendDetailModelMetaData
    { 
            [Required] 
            public object Dividend_Currency_Code { get; set; }

            [Required]
            [MaxLength(1, ErrorMessage = "Value for {0} can be only one character long")]
            public object Dividend_Type { get; set; }
            [Required ]
            [Range(1, 9.999999, ErrorMessage = "Value for {0} must be less than {2}")]
            public object Dividend_Split { get; set; }

           [RegularExpression("[0-9]{1,}", ErrorMessage = "Value for {0} must be an integer value")]
            public object Dividend_Seq_Number { get; set; }
  
    }
}