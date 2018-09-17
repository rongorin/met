using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
// Filter to centralise the check that the user is accessing either their Entity's data or the Generic Entity ONLY.
// reference : https://blog.falafel.com/custom-filter-asp-net-mvc-5/
//---------------------------------------------------------------------------------------------------------------
namespace MetopeMVCApp.Filters
{

        //public class CustomEntityAuthoriseFilter : ActionFilterAttribute
        //{
        //    private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    string userId = HttpContext.Current.User.Identity.GetUserId();


        //    public override void OnActionExecuted(ActionExecutedContext filterContext)
        //    { 

        //        decimal refGenericEntity = (decimal)filterContext.Controller.ViewBag.genericEntity;
        //        decimal refEntityIdScope =  (decimal)filterContext.Controller.ViewBag.EntityIdScope;

        //        var usersEntity = filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute");

        //        if (usersEntity != filterContext.Controller.ViewBag.genericEntity &&
        //            usersEntity != filterContext.Controller.ViewBag.thisEntityID) 
             
        //        {
        //            //var result = _authorizeService.CanManageUser(userId);
                   
        //            filterContext.Result = new RedirectToRouteResult(
        //                new RouteValueDictionary{{ "controller", "Account" },
        //                                        { "action", "Login" }
 
        //                                        });
        //            //filterContext.Result = new RedirectToRouteResult(
        //            //                new RouteValueDictionary { 
        //            //                            { "action", "Index" }, 
        //            //                            { "controller", "Unauthorised" } });

                 
        //        }
        //        base.OnActionExecuted(filterContext);
        //    }
             
        //}
      

}