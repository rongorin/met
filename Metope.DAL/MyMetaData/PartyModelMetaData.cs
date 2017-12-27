 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
  using System.ComponentModel.DataAnnotations;

namespace Metope.DAL.MyMetaData
 {
    [MetadataType(typeof(PartyModelMetaData))]

    public class PartyModelMetaData
    { 

        public object Entity_ID { get; set; }

        [Required]
        [Display(Name = "Party Code")]
        public object Party_Code { get; set; }

        [Display(Name = "Fin Year End")]
        public object Financial_Year_End { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Party name should be between 4 and 80 characters")]
        [Display(Name = "Party Name")]
        public object Party_Name { get; set; }

        [Required]
        [Display(Name = "Party Type")]
        public object Party_Type { get; set; }

        [Required]
        [Display(Name = "Country")]
        public object Country_Code { get; set; }
                 
        [Display(Name = "Swift Id")]
        public object SWIFT_ID { get; set; } 
        [Display(Name = "BIC Code")]
        public object BIC_Code { get; set; }

        [Display(Name = "Locked?")]
        public object System_Locked { get; set; }
    }
}