using Inżynierka.Shared.ViewModels;
using Inżynierka_Common.Helpers;
using Inżynierka_Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using Inżynierka.Shared.ViewModels.User;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("AuthController")]
    public class AuthController : BaseController
    {

        private readonly AuthService _authServices;
        private readonly EmailService _emailService;
        private readonly UserService _userService;
        private readonly UserActionService _userActionService;

        public AuthController(AuthService authServices, EmailService emailService, UserService userService, UserActionService userActionService)
        {
            _authServices = authServices;
            _emailService = emailService;
            _userService = userService;
            _userActionService = userActionService;
        }

        //Returns info about currently signed-in user
        [Authorize]
        [HttpGet]
        [Route("user-profile")]
        public async Task<IActionResult> UserProfileAsync()
        {
            int userId = GetUserId();

            UserDTO user = _userService.Get(userId);

            return Ok(user);
        }


        /// <summary>
        /// Sign in user method
        /// </summary>
        /// <param name="loginViewModel">E-mail of the user</param>
        /// <returns>Signed in user</returns>
        /// <response code="400">string "Cannot log in. Check your credentials."</response>
        /// <response code="200">Returns user email</response>
        [HttpPost]
        [Route("SignIn")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn(SignInViewModel loginViewModel)
        {
            LogUserAction("AuthController", "SignIn", JsonSerializer.Serialize(loginViewModel.Email), _userActionService);
            User? user = _userService.GetUserByEmail(loginViewModel.Email);
            if (user == null) return BadRequest(new ResponseViewModel("Nie ma takiego użytkownika"));
            if (user != null && user.UserStatus != EnumHelper.GetDescriptionFromEnum(UserStatus.ACTIVE))
            {
                return BadRequest(new ResponseViewModel("Potwierdź swoje konto zanim się zalogujesz!"));
            }

            ClaimsIdentity? claimsIdentity = await _authServices.ValidateAndCreateClaim(loginViewModel.Password, loginViewModel.Email);

            if (claimsIdentity != null)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new ResponseViewModel(loginViewModel.Email));
            }

            string message = Translate(ErrorMessageHelper.WrongCredentials);

            return BadRequest(new ResponseViewModel(message));
        }


        /// <summary>
        /// Sign out user method
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns user Logs out</response>
        [HttpGet]
        [Route("LogOut")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> LogOut()
        {
            LogUserAction("AuthController", "LogOut", "", _userActionService);
            await HttpContext.SignOutAsync("Cookies");

            return Ok();
        }


        /// <summary>
        /// User registration method
        /// </summary>
        /// <returns></returns>
        /// <param name="newUser">Object containing information about a new user</param>
        /// <response code="200">User created successfully</response>
        /// <response code="400">Invalid email or password or user already exist</response>
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(NewUserViewModel newUser)
        {
            bool check = _userService.CheckIfUserExist(newUser.Email);
            string message;

            if (check)
            {
                message = Translate(ErrorMessageHelper.UserExists);

                return BadRequest(new ResponseViewModel(message));
            }

            if (!Regex.IsMatch(newUser.FirstName, @"^[żźćńółęąśŻŹĆĄŚĘŁÓŃa-zA-Z-\s]+$") || !Regex.IsMatch(newUser.LastName, @"^[żźćńółęąśŻŹĆĄŚĘŁÓŃa-zA-Z-\s]+$"))
            {
                message = Translate(ErrorMessageHelper.ForbiddenSymbol);

                return BadRequest(new ResponseViewModel(message));
            }

            if (!Regex.IsMatch(newUser.PhoneNumber, @"^([0-9]{9})$"))
            {
                message = Translate(ErrorMessageHelper.WrongPhoneNumber);
                return BadRequest(new ResponseViewModel(message));
            }


            (Guid?, string) confirmationGuid = await _userService.CreateUser(newUser.AgencyInvitationGuid, newUser.FirstName, newUser.LastName, newUser.Password, newUser.Email, newUser.PhoneNumber, newUser.PostalCode, newUser.City, newUser.Street);
            if (confirmationGuid.Item1 == null) 
            { 
                return BadRequest(new ResponseViewModel(confirmationGuid.Item2));
            }

            string url = $"{this.Request.Scheme}://{this.Request.Host}/Auth/ConfirmEmailPage?guid={confirmationGuid.Item1}";
            _emailService.SendConfirmationEmail(newUser.Email, url);

            message = Translate(MessageHelper.UserCreated);

            NewUserViewModel toSerialize = newUser;
            toSerialize.Password = "";
            toSerialize.SecondPassword = "";
            toSerialize.AgencyInvitationGuid = null;
            string jsonSerializedUser = JsonSerializer.Serialize(toSerialize);
            LogUserAction("AuthController", "Register", jsonSerializedUser, _userActionService);

            return Ok(new ResponseViewModel(message));
        }



        /// <summary>
        /// If user exist, sends recovery to mail parameter adress.
        /// </summary>
        /// <returns></returns>
        /// <param name="confirmationGuid" example="9fb49f98-f169-4316-9737-23b656058c5c"></param>
        /// <response code="200">Account confirmed</response>
        /// <response code="400">Confirmation Failed</response>
        [HttpPost]
        [AllowAnonymous]
        [Route("ConfirmAccount")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmAccount([FromBody] Guid guid)
        {
            bool check = _authServices.ConfirmUser(guid).Result;
            string message;

            if (check)
            {
                message = Translate(MessageHelper.AccountConfirmed);

                return Ok(new ResponseViewModel(message));
            }

            message = Translate(ErrorMessageHelper.ConfirmationFailed);

            return BadRequest(new ResponseViewModel(message));
        }



        /// <summary>
        /// If user exist, sends recovery to mail parameter adress.
        /// </summary>
        /// <returns></returns>
        /// <param name="email" example="test@gmail.com"></param>
        /// <response code="200">Recovery e-mail sent</response>
        /// <response code="400">Account doesn't exist</response>
        [HttpPost]
        [Route("PasswordRecoveryMail")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PasswordRecoveryMail([FromBody] string email)
        {
            bool changedPassword = _userService.CheckIfUserExist(email);
            string message;

            if (!changedPassword)
            {
                message = Translate(ErrorMessageHelper.AccountDoesntExist);

                return BadRequest(new ResponseViewModel(message));
            }
            var recoveryGuid = _userService.SetUserRecoveryGuid(email);

            //var fullUrl = this.Url.Action("RecoverPassword", "Auth", new { guid = recoveryGuid }, protocol: "https");

            string url = $"{Request.Scheme}://{Request.Host}/Auth/RecoverPassword?guid={recoveryGuid}";

            _emailService.SendPasswordRecoveryEmail(email, url);

            message = Translate(MessageHelper.RecoveryEmailSent);

            return Ok(new ResponseViewModel(message));
        }


        //CheckIfRecoveryGuidExists
        /// <summary>
        /// Checks if a user with given RecoveryGuid exists
        /// </summary>
        /// <returns></returns>
        /// <param name="guid">Object of Guid class</param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        [HttpPost]
        [Route("CheckIfRecoveryGuidExists")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckIfRecoveryGuidExists([FromBody] Guid guid)
        {
            User? user = _authServices.GetUserWithRecoveryGuid(guid);

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }



        /// <summary>
        /// If Guid and user email are assign to same entity, change user password
        /// </summary>
        /// <returns></returns>
        /// <param name="user">object of the UserPasswordRecoveryViewModel class</param>
        /// <response code="200">Password changed</response>
        /// <response code="400">Email and Guid values are assigned to different users, try again</response>
        [HttpPost]
        [Route("RecoverPassword")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordForm2ViewModel model)
        {
            
            User? user = _authServices.GetUserWithRecoveryGuid(model.Guid);
            string message;

            if (user == null)
            {
                message = Translate(ErrorMessageHelper.NoUserWithGivenRecoveryGuid);

                return BadRequest(message);
            }
            LogUserAction("AuthController", "RecoverPassword", JsonSerializer.Serialize(new { user.Email, model.Guid}), _userActionService);
            var result = await _userService.ChangeUserPassword(user.Email, model.Password);
            if(!result)
            {
                message = Translate(MessageHelper.PasswordDuplicate);
                return Ok(new ResponseViewModel(message));
            }
            else
            {
                message = Translate(MessageHelper.PasswordChanged);
                return Ok(new ResponseViewModel(message));
            }
        }
    }
}
