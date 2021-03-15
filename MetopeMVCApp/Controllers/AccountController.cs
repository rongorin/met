using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ASP.MetopeNspace.Models;
using System.Collections;
using System.Web.Caching;
using StructureMap;
using System.Configuration; 

namespace ASP.MetopeNspace.Controllers
{
    [Authorize]
   // [SslRequest]
    public class AccountController : Controller
    { 
        //private readonly UserManager<IdentityUser> _userManager;
        private static readonly int PasswordExpireDays = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordExpireDays"]);


        public AccountController()
              : this( new UserStore<ApplicationUser>(new ApplicationDbContext()))
            // : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }
        public AccountController(UserStore<ApplicationUser> userManager)
        {
            store = userManager;
            UserManager = new UserManager<ApplicationUser>(store);
           // UserManager.UserLockoutEnabledByDefault = true; 
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public UserStore<ApplicationUser> store { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

   
                var user = await UserManager.FindByNameAsync(model.UserName);
                //throw new ArgumentNullException("Model"); 
                if (user != null)
                {
                    //// var checkIfUsernamePasswordValid = await UserManager.FindAsync(model.Email,model.Password);
 
                    //var checkIfUsernamePasswordValid = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password);
                    var validCredentials = await UserManager.FindAsync(model.UserName, model.Password);
                
                    //*******************************************************
                    // Check if user is locked out due to invalid login attempts,
                    // If they are inform them
                    //*******************************************************

                    if (await UserManager.IsLockedOutAsync(user.Id))
                    {
                        ModelState.AddModelError("", string.Format("Your account has been locked out for {0} minutes due to {1} invalid login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"], ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]));
                    }
                     //*******************************************************
                    // Count number of failed login attempts and display to user
                    // Before locking them out
                    //*******************************************************

                    //else if (await UserManager.GetLockoutEnabledAsync(user.Id) && Convert.ToBoolean(checkIfUsernamePasswordValid) != true)
                    else if (await UserManager.GetLockoutEnabledAsync(user.Id) && validCredentials == null)
                    { 
                       // await UserManager.AccessFailedAsync(user.Id);

                        string message; 
                        if (await UserManager.IsLockedOutAsync(user.Id))
                        {
                            message = string.Format("Your account has been locked out for {0} minutes due to {1} invalid login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"], ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]);
                        }
                        else
                        {
                            user.AccessFailedCount++;   
                            //var ctx = store.Context;  
                            //await ctx.SaveChangesAsync();
                            int accessFailedCount = await UserManager.GetAccessFailedCountAsync(user.Id);
                
                            int attemptsLeft = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]) - accessFailedCount;
                           
                            if (attemptsLeft < 1)
                            {
                                user.LockoutEndDateUtc = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"]));
                                user.AccessFailedCount = 0;
                                message = string.Format("Invalid user/password. You are locked out for {1} minutes.", attemptsLeft, ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"]);
                            }
                            else
                                message = string.Format("Invalid user/password. You have {0} more attempt(s) before your account is locked out.", attemptsLeft);


                            IdentityResult result = await UserManager.UpdateAsync(user); 
                        }

                        ModelState.AddModelError("", message);
                    }
                    else if (validCredentials == null)
                    {
                        ModelState.AddModelError("", "Invalid credentials. Please try again.");
                    } 
                    else
                    {
                        //await UserManager.AccessFailedAsync(user.Id);
                        await SignInAsync(user, model.RememberMe);

                        // If user get login correct before lock out reset failed count
                        await UserManager.ResetAccessFailedCountAsync(user.Id);

                        if (user.LastPasswordChangedDate.AddDays(PasswordExpireDays) < DateTime.UtcNow)
                        {
                            return RedirectToAction("Manage", new ManageUserViewModel { });
                        }
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", string.Format("Sorry we cannot find your email address."));
                    return View();
                }
            }
            return View(model);
        }


//        UserManager<User> userManager = new UserManager<User>(new UserStore());

//if (userManager.SupportsUserLockout && userManager.IsLockedOut(userId))
//    return;

//var user = userManager.FindById(userId);
//if (userManager.CheckPassword(user, password))
//{
//    if (userManager.SupportsUserLockout && userManager.GetAccessFailedCount(userId) > 0)
//    {
//        userManager.ResetAccessFailedCount(userId);
//    }

//    // Authenticate user
//}
//else
//{
//    if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(userId))
//    {
//        userManager.AccessFailed(userId);
//    }
//}

        //
        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Loginxx(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    { 
        //        return View(model);
        //    }

        //   //  RemoveContextItems(); //remove item that are cached in the context. ie entityid etc

        //    // find user by username first
        //    var user = await UserManager.FindByNameAsync(model.UserName);
 

          
        //    if (user != null)
        //    {
        //        //adapt for password expiry: stackoverflow.com/questions/29039537/how-to-setup-password-expiration-using-asp-net-identity-framework
        //        //var user = await UserManager.FindAsync(model.UserName, model.Password);
        //        var validCredentials = await UserManager.FindAsync(model.UserName, model.Password);

        //        if (await UserManager.IsLockedOutAsync(user.Id))
        //        {
        //            ModelState.AddModelError("", string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));
        //        }
        //        else if (await UserManager.GetLockoutEnabledAsync(user.Id) && validCredentials == null)
        //        {
        //            // Record the failure which also may cause the user to be locked out
        //            await UserManager.AccessFailedAsync(user.Id);
                     

        //            string message;

        //            if (await UserManager.IsLockedOutAsync(user.Id))
        //            {
        //                message = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString());
        //            }
        //            else
        //            {

        //                int accessFailedCount = await UserManager.GetAccessFailedCountAsync(user.Id);

                 

        //                int attemptsLeft =
        //                    Convert.ToInt32(
        //                        ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"].ToString()) -
        //                    accessFailedCount;

        //                message = string.Format(
        //                    "Invalid credentials. You have {0} more attempt(s) before your account gets locked out.", attemptsLeft);
        //            } 
        //            ModelState.AddModelError("", message);
        //        }
        //        else if (validCredentials == null)
        //        {
        //            ModelState.AddModelError("", "Invalid credentials. Please try again.");
        //        } 
        //        else
        //        {
        //            await SignInAsync(user, model.RememberMe);
        //            await UserManager.ResetAccessFailedCountAsync(user.Id); //clear out token

        //            if (user.LastPasswordChangedDate.AddDays(PasswordExpireDays) < DateTime.UtcNow)
        //            {
        //                return RedirectToAction("Manage", new ManageUserViewModel { });
        //            }

        //            return RedirectToLocal(returnUrl); 
        //        }   
        //    } 

           

        //    return View(model);
           
        //}

        //
        // GET: /Account/Register
       // [AllowAnonymous]
        [Authorize(Roles = "Admin")] 
        public ActionResult Register()
        { 
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous] Registering new users will be only by the administrators
        [Authorize(Roles = "Admin")] 
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //RG: IMPORTANT remove the model.EntityIdScope below from being posted  
                var user = new ApplicationUser() { UserName = model.UserName, EntityIdScope = model.EntityIdScope, IsEnabled = true};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInAsync(user, isPersistent: false);
                     
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
  
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    { 
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            user.LastPasswordChangedDate = DateTime.UtcNow;
                            await UserManager.UpdateAsync(user);
                             
                            await SignInAsync(user, isPersistent: false );
                        }

                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff( )
        {
            /*----------------------------------------------
            on signing out , i remove all the cached dropdown items.
            HttpContext oc = System.Web.HttpContext.Current;
            ----------------------------------------------*/
            RemoveContextItems();

            AuthenticationManager.SignOut(); 
            return RedirectToAction("Index", "Home");
        }
    
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private void RemoveContextItems()
        {
            HttpContext oc = System.Web.HttpContext.Current;
            foreach (var c in oc.Cache)
            {
                oc.Cache.Remove(((DictionaryEntry)c).Key.ToString());
            } 
        }
        private bool HasPassword() 
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}