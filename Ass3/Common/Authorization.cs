using Ass3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SignalRAssignment.Common
{
    public class Authorization { }

    public class AuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = VaSession.Get<AppUser>(context.HttpContext.Session, "Account");
            if (account == null)
            {
                var query = context.HttpContext.Request.QueryString;
                context.Result = new RedirectToPageResult("/Login", new { returnUrl = context.HttpContext.Request.Path + query });
                
            }
        }
    }
}
