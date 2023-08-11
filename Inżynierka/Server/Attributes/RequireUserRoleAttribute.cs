using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inżynierka.Server.Attributes
{
    public class RequireUserRoleAttribute : Attribute, IAuthorizationFilter
    {
        public string[] usrRoles { get; set; }

        public RequireUserRoleAttribute(params string[] userRoles)
        {
            usrRoles = userRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAdmin = context.HttpContext.User.Claims
                .Any(x => x.Type == "RoleName" && x.Value == UserRoles.ADMIN.ToString());
            if (isAdmin)
                return;

            foreach (var role in usrRoles)
            {
                var hasClaim = context.HttpContext.User.Claims.Any(x => x.Type == "RoleName" && x.Value == role);

                if (hasClaim)
                    return;
            }

            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
