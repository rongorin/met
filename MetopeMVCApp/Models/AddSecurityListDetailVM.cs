using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [StringLength(15, ErrorMessage =
        "Security List Code should be max 15 characters ")]
        public string Security_List_Code { get; set; }

        [Required]
        public string Security_List_Name { get; set; }
        [Required]
        public string Description { get; set; }
        public bool System_Locked { get; set; }
        //public Security_List_Detail SecurityListDetail{ get; set; }
    

        public List<CheckBoxListItem> Secs { get; set; }
    }
}