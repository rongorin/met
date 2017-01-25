using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
  using System.ComponentModel.DataAnnotations;

namespace MetopeMVCApp.Models.MyMetaData
{  
    public class PortfolioModelMetatData
    {
        public object Entity_ID { get; set; }

        [Required] 
        [Display(Name = "Portfolio Code")]  
        public object Portfolio_Code { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Portfolio name should be between 6 and 100 characters")]
        [Display(Name = "Portfolio Name")]
        public object Portfolio_Name { get; set; }
        
        [Required]
        public object Manager { get; set; }

        [Required]
        [Display(Name = "Base Currency")]
        public object Portfolio_Base_Currency { get; set; }
        [Required]
        [Display(Name = "Report Currency")]
        public object Portfolio_Report_Currency { get; set; }
        [Required]
        [Display(Name = "Domicile")] 
        public object PortfolIo_Domicile  { get; set; }
        [Required]
        [Display(Name = "Custodian")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Custodian should be between 1 and 20 characters")]
        public object Custodian_Code { get; set; }  

        [Required]
        [Display(Name = "Portfolio Type")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Type should be between 1 and 20 characters")]
        public object Portfolio_Type { get; set; }

        [Required]
        [Display(Name = "Status")]
        public object Portfolio_Status { get; set; }
         
        [Display(Name = "Inception Date")] 
        public object Inception_Date { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = "{0:yyyy-MM-dd}")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fin Year End")]  
        public object Financial_Year_End { get; set; }

        [Display(Name = "System Locked?")]
        public object System_Locked { get; set; }

        [Display(Name = "Active?")]
        public object Active_Flag { get; set; }
    }
}