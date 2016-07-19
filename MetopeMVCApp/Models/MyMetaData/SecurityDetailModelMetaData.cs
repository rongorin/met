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
        [Display(Name = "Security Type Code")]
        [StringLength(20, ErrorMessage =
                 "Security Type Code should be max 10 characters ")] 
        public object Security_Type_Code { get; set; }
        	
    }
}