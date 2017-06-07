//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web; 
//using Microsoft.AspNet.Identity.EntityFramework;
//using ASP.MetopeNspace.Models;
//using Microsoft.AspNet.Identity;
////using System.Web.Http;
//using System.Web.Http;
//using System.Web.Mvc;
//using System.Security.Principal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
namespace MetopeMVCApp.Filters
{
    public class CustomAuthoriseEntityFilter : AuthorizeAttribute  
    {
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        string userId = HttpContext.Current.User.Identity.GetUserId();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }


            //if ((this._rolesSplit.Length > 0) && !this._rolesSplit.Any<string>(new Func<string, bool>(user.IsInRole)))
            //{
            //    return false;
            //}
      


            return true;
        }
        //private readonly decimal[] allowedEnts;

        //public CustomAuthoriseEntityFilter(params decimal[] roles)  
        //{
        //    this.allowedEnts = roles;  
        //}
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId());
   


        //    //bool authorize = false;
        //    //foreach (var role in allowedEnts)
        //    //{ 

        //    //    manager.FindById( User.Identity.GetUserId());
        //    //    var user = context.AppUser.Where(m => m.UserID ==  HttpContext.Current.User.Identity.GetUserId()/* getting user form current context */ && m.Role == role &&
        //    //    m.IsActive == true); // checking active users with allowed roles.  
        //    //    if (user.Count() > 0)
        //    //    {
        //    //        authorize = true; /* return true if Entity has current user(active) with specific role */
        //    //    }
        //    //}
        //    //return authorize;
        //}  

    }
}