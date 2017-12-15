using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{ 
    public class SecurityListDetailIndexVM
    { 
        public SecurityListDetailIndexVM()
        {
            SecurityList = new List<SecurityListVM>();
        }

        public decimal Entity_ID { get; set; } 
        public string Security_List_Code { get; set; } 
        public string Security_List_Name { get; set; }   
        public string Description { get; set; }
        public bool System_Locked { get; set; }
        //public Security_List_Detail SecurityListDetail{ get; set; }

        public IList<SecurityListVM> SecurityList { get; set; } 

    }

    public class SecurityListVM
    {
        //public decimal Entity_ID { get; set; }
        //public string  Security_List_Code { get; set; }

        public decimal Security_ID { get; set; } 
        public string Security_Ticker { get; set; }   

    }
}