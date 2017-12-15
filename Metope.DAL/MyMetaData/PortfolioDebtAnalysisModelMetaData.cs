using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MetopeMVCApp.Models.MyMetaData
{
        [MetadataType(typeof(PartyDebtAnalysisModelMetaData))]

    public class PartyDebtAnalysisModelMetaData
    {
        [Required]
        public object Party_Code { get; set; }  
        [Required] 
        public object Financials_Type { get; set; }
        [Required] 
        public object Financials_Date { get; set; }


    }
}