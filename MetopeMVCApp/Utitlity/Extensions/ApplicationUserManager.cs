using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Utitlity.Extensions
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
         public ApplicationUserManager(): base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
            PasswordValidator = new MinimumLengthValidator (10);
        }
    }
}
 