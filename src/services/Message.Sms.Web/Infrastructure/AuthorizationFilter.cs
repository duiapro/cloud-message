﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Message.Sms.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public bool IsCheck { get; set; } = true;

        public bool IsAdmin { get; set; } = true;

        public AuthFilterAttribute() { }

        public AuthFilterAttribute(bool isCheck)
        {
            IsCheck = isCheck;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (IsCheck)
            {
                var user = context.HttpContext.Session.GetString(AppUsers.SESSION_KEY);
                if (user == null)
                {
                    //ajax request
                    if (context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                    {
                        context.Result = new UnauthorizedObjectResult("User identity is invalid, please log in again.");
                    }
                    else
                    {
                        context.Result = new RedirectResult("/users/login", false);
                    }
                }
                else if (IsAdmin)
                {
                    var appUsers = context.HttpContext.RequestServices.GetService<AppUsers>();
                    if (appUsers?.IsAdmin == false)
                    {
                        //throw new Exception("sb, do you have permission?");
                        context.HttpContext.Response.Redirect("/error.html?message=sb, do you have permission?", false);
                    }
                    //context.HttpContext.Response.Redirect("/user", false);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
