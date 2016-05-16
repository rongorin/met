using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Utitlity.Extensions
{
    public static class SessionExtensions
    {
        public static T GetDataFromSession<T>(this HttpSessionStateBase session, string key)
        {
            return (T)session[key];
        } 
        public static void SetDataToSession<T>(this HttpSessionStateBase session, string key, object value) 
       { 
           session[key] = value; 
       } 
   } 
   }
 