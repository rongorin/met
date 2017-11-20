using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class SecurityListDetailIndexVM
    {
        public decimal Entity_ID { get; set; }
        public string Security_List_Code { get; set; }
        public string Security_List_Name { get; set; }
        public string Description { get; set; }
        //public Security_List_Detail SecurityListDetail{ get; set; }

        public IEnumerable<SecurityListVM> SecurityList { get; set; }

    }

    public class SecurityListVM
    {
        public decimal Entity_ID { get; set; }
        public string  Security_List_Code { get; set; }

        public decimal Security_ID { get; set; }   

    }
}