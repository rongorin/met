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
        [StringLength(20, ErrorMessage =
             "Portfolio code should be max 20 characters ")]
        public object Portfolio_Code { get; set; }
        [Required]
        [StringLength(100)]
        public object Portfolio_Name { get; set; }
 
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public object Inception_Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public object Financial_Year_End { get; set; }
  
    }
}