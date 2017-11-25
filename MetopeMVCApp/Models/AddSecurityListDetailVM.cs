using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class AddSecurityListDetailVM
    {
        public AddSecurityListDetailVM()
        {
            Secs = new List<CheckBoxListItem>();
        }

        public decimal Entity_ID { get; set; }
        public string Security_List_Code { get; set; }
        public string Security_List_Name { get; set; }
        public string Description { get; set; }
        //public Security_List_Detail SecurityListDetail{ get; set; }
        public bool System_Locked { get; set; }

        public List<CheckBoxListItem> Secs { get; set; }
    }
}