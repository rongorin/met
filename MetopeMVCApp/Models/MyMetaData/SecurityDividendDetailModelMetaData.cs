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
            public object Dividend_Type { get; set; }
  
    }
}