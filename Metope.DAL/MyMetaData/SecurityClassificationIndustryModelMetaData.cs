using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metope.DAL.MyMetaData
{
    class SecurityClassificationIndustryModelMetaData
    {
        [Required] 
        public object Classification_Code { get; set; }
        [Required] 

        public System.DateTime Effective_Date { get; set; }
        [Required]  
        public string Industry_Code { get; set; }
    }
}
