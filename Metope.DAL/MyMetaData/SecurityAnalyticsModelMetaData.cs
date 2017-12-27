using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Metope.DAL.MyMetaData
{
    [MetadataType(typeof(SecurityAnalyticsModelMetaData))]

    public class SecurityAnalyticsModelMetaData
    {
        [Display(Name = "Security")] 
        public object Security_ID { get; set; }

        [Range(0, 99.999999, ErrorMessage = "Value for {0} must be less than 100")]
        public object Risk_Premium { get; set; }

        [Range(0, 9.999999, ErrorMessage = "Value for {0} must be less than 10")] 
        public object Earnings_Growth_Compounded { get; set; }

        [RegularExpression("[0-999]{1,}", ErrorMessage = "Value for {0} must be an integer")]
        [Range(0, 999, ErrorMessage = "Value for {0} must be less than 1000")]
        public object Custom_Period { get; set; }

    }
}