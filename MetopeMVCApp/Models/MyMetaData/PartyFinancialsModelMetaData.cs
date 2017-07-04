using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models.MyMetaData
{
    public class PartyFinancialsModelMetaData
    { 
        [Required]
        public object Party_Code { get; set; }
        [Required]
        public object Actual_Forecast_Indicator { get; set; }

 
    }
}