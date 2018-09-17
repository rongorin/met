using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    // Use this custom model for the checkboxes in the Add of a new SecurityListDetail
    // Ref : https://www.exceptionnotfound.net/simple-checkboxlist-in-asp-net-mvc/
 
    public class CheckBoxListItem
    {
        public decimal ID { get; set; }
        public string Display { get; set; }
        public bool IsChecked { get; set; }
    }
}