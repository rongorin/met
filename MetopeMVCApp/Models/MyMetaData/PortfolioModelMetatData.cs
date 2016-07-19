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
        [StringLength(20, ErrorMessage =
             "Portfolio code should be max 20 characters ")]

        public object Portfolio_Code { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Portfolio Name")]
        public object Portfolio_Name { get; set; }

        [Required]
        [Display(Name = "Portfolio Type")]
        [StringLength(20, ErrorMessage =
             "Portfolio type should be max 20 characters ")] 
        public object Portfolio_Type { get; set; }

        [Display(Name ="Inception Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public object Inception_Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fin Year End")]  
        public object Financial_Year_End { get; set; }
  
    }
}