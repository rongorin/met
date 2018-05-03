using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metope.DAL.MyMetaData
{
     [MetadataType(typeof(PortfolioPerformanceModelMetaData))]
    class PortfolioPerformanceModelMetaData
    { 
         [Required]
         [Display(Name = "Portfolio Code")]
         public object Portfolio_Code { get; set; }

    }
}
