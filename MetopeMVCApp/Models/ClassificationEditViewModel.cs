using Metope.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class ClassificationEditViewModel
    {
        public decimal Entity_ID { get; set; }
        public string Classification_Code { get; set; }
        public string Description { get; set; }
        //public Nullable<decimal> All_In_Price { get; set; } 
        public   ICollection<Classification_Industry> Classification_Industry { get; set; }

    }
}