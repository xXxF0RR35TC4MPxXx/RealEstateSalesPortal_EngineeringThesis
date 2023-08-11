using Inżynierka.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Resources;
using Inżynierka_Common.Enums;

namespace Inżynierka.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ResourceManager _resourceManager;
        protected CultureInfo culture;

        public BaseController()
        {
            culture = new CultureInfo("en-GB");
            _resourceManager = new ResourceManager("HeRoBackEnd.LanguageResources.LangResource", Assembly.GetExecutingAssembly());
        }

        protected void LogUserAction(string controller, string controllerAction, string actionParameters, UserActionService userActionService)
        {
            int userId = GetUserId();
            UserAction userAction = new()
            {
                UserId = userId,
                Controller = controller,
                ControllerAction = controllerAction,
                ActionParameters = actionParameters,
                Date = DateTime.Now
            };

            userActionService.CreateUserAction(userAction);
        }

        protected int GetUserId()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            Claim? idClaim = claims.FirstOrDefault(e => e.Type == "Id");

            int id;
            if (idClaim == null)
            {
                id = 0;
            }
            else
            {
                int.TryParse(idClaim.Value, out id);
            }

            return id;
        }

        protected string GetUserRole()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            Claim? roleClaim = claims.FirstOrDefault(e => e.Type == "RoleName");
            string role;
            if (roleClaim == null)
            {
                role = UserRoles.ANONYMOUS.ToString();
            }
            else 
            { 
                role = roleClaim.Value; 
            }
                
            return role;
        }

        protected string Translate(string message)
        {
            if (HttpContext.Session.GetString("Language") == null || HttpContext.Session.GetString("Language") == "en-GB")
            {
                return message;
            }

            string language = HttpContext.Session.GetString("Language");
            CultureInfo culture = new CultureInfo(language);

            string result = _resourceManager.GetString(message, culture);

            return result;
        }
    }
}
