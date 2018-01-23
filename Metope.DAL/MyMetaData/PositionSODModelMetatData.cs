using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Metope.DAL.MyMetaData
{
    class PositionSODModelMetatData
    {
        [Required]
        [Display(Name = "Portfolio")]
        public object Portfolio_Code { get; set; }

        [Required]
        [Display(Name = "Security")]  
        public object Security_ID { get; set; }

        [Required]
        [Display(Name = "Long / Short")]

        [StringLength(1, MinimumLength = 1 )]
        public object Long_Short_Indicator { get; set; }
        [Required] 
        public System.DateTime Position_Date { get; set; }

    }
}
