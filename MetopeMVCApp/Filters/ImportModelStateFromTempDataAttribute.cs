﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//This is Not used.  Attempted this: http://benfoster.io/blog/automatic-modelstate-validation-in-aspnet-mvc
//   Also see the Filters ModelStateTempDataTransfer and ImportModelStateFromTempDataAttribute 
namespace MetopeMVCApp.Filters
{
    /// <summary>
    /// An Action Filter for importing ModelState from TempData.
    /// You need to decorate your GET actions with this when using the <see cref="ValidateModelStateAttribute"/>.
    /// </summary>
    /// <remarks>
    /// Useful when following the PRG (Post, Redirect, Get) pattern.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ImportModelStateFromTempDataAttribute : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Only copy from TempData if we are rendering a View/Partial
            if (filterContext.Result is ViewResult)
            {
                ImportModelStateFromTempData(filterContext);
            }
            else
            {
                // remove it
                RemoveModelStateFromTempData(filterContext);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}