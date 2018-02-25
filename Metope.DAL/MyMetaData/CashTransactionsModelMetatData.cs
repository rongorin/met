using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metope.DAL.MyMetaData
{
    [MetadataType(typeof(CashTransactionsModelMetatData))]

    public class CashTransactionsModelMetatData
    {    

        [Required]
        [Display(Name = "Portfolio")]
        public object Portfolio_Code { get; set; }

        [Required]
        [Display(Name = "Cash Security")]
        public object Cash_Security_ID { get; set; }

        [Required]
        [Display(Name = "Source Code")]
        public object Transaction_Source_Code { get; set; }

        [Required]
        //[StringLength(1, MinimumLength = 1)]
        public object External_ID { get; set; }

        [Required]
        public object Transaction_Date { get; set; }

        [Required]
        public object Value_Date { get; set; }
        [Required]
        public object Transaction_Currency_Code { get; set; }
         
        [Display(Name = "Order Allocation")]
        public object Allocation_ID { get; set; }
         
        [Range(0, 999999999999999)] 
        public object Cashflow_ID { get; set; }



        [Required]
        [Display(Name = "Transaction Type")]
        [StringLength(30, ErrorMessage =
                     "Cash Transaction Type should be max 30 characters ")]
        public object Cash_Transaction_Type { get; set; } 
         

        [Required]
        [Range(0, 999999999999999)]
        public object Transaction_Amount { get; set; }
        [Required]
        [Range(0, 999999999999999)]
        public object BaseCur_Amount { get; set; }
        
        [Required]
        public object Create_User_Code { get; set; }
    }
}
