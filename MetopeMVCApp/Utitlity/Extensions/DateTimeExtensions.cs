using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MetopeMVCApp.Utitlity.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToCustomDateString(this DateTime? dateTime)
        {
            //return dateTime.ToString("MM/dd/yyyy hh:mm tt");
            return dateTime != null ? dateTime.Value.ToString("yyyy-MM-dd") : "n/a";
        }
    }
}