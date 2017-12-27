using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Metope.DAL.MyMetaData
{
    [MetadataType(typeof(SecurityListDetailModelMetaData))]
    public class SecurityListDetailModelMetaData
    {
        [Required]
        [StringLength(15, ErrorMessage =
               "Security List Code should be max 15 characters ")]
        public object Security_List_Code { get; set; }

        [Required] 
        public object Security_List_Name { get; set; }
        [Required] 
        public object Description{ get; set; }  
       
    }
}