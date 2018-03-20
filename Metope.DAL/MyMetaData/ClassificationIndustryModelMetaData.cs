using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metope.DAL.MyMetaData
{
    [MetadataType(typeof(ClassificationIndustryModelMetaData))]
    class ClassificationIndustryModelMetaData
    {
        [Required] 
        public object Industry_Code { get; set; }

        [Required]
        public object Classification_Code { get; set; }

        [Required] 
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Description should be between 4 and 100 characters")]
        public object Description { get; set; }
    }
}
