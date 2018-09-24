using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metope.DAL.MyMetaData
{

    [MetadataType(typeof(ForexForecastModelMetaData))]

    public class ForexForecastModelMetaData
    {
        [Required]
        [Display(Name = "Security")]
        public object Security_ID { get; set; }

        //[RegularExpression("[0-999]{1,}", ErrorMessage = "Value for {0} must be an integer")]
        [Required]
        [MaxLength(6, ErrorMessage = "Value for {0} must be in format mmyyyy")]
        [MinLength(6, ErrorMessage = "Value for {0} must be in format mmyyyy")] 
        [RegularExpression("^[0-9]*$", ErrorMessage = "Value for {0} must be numeric")]
        public object Month_Year {get;set;} 
 

    }


}
