using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models.MyMetaData
{
    public class DebtExpiryProfileModelMetaData
    {
        [Required]
        public object Party_Code { get; set; }
  
     [Range(0, Int64.MaxValue, ErrorMessage = "Debt Amount should be a valid amount")]
        public object Expiring_Debt_Amount { get; set; }
    }
}