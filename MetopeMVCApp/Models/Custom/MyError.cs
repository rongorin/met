using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models.Custom
{
    public class MyError
    {
        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }
    }
}