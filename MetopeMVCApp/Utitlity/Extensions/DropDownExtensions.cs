using ASP.MetopeNspace.Models;
using MetopeMVCApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Utitlity.Extensions
{
    public static class DropDownExtensions
    {
        //public static SelectList ApplyDiscountForAccountStatus(this UserManager<ApplicationUser> price, 
        //                                                    decimal discountSize)
        //{
        //    return ;
        //}
         public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items,
                                                                Func<T, string> text, Func<T, string> value = null,
                                                                Func<T, Boolean> selected = null)
 
                { 
                    return items.Select(p => new SelectListItem 
                    { 
                        Text = text.Invoke(p), 
                        Value = (value == null ? text.Invoke(p) : value.Invoke(p)), 
                        Selected = selected != null && selected.Invoke(p) 
                    });
 
                }
 
    }
}