using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace Metope.DAL.MyMetaData
{
    [MetadataType(typeof(PortfolioValuationModelMetatData))]
    class PortfolioValuationModelMetatData
    { 
        public object Entity_ID { get; set; }

        [Required] 
        [Display(Name = "Portfolio Code")]  
        public object Portfolio_Code { get; set; }
       
    }
}