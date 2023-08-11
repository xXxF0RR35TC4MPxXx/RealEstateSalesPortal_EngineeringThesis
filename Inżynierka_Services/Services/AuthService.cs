using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Inżynierka_Common.ServiceRegistrationAttributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class AuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;

        public AuthService(ILogger<AuthService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        private bool ValidateUser(string password, string email)
        {
            string? passwordInDb = _userRepository.GetUserPassword(email);
            string passwordAfterHash = PasswordHashHelper.GetHash(password);

            if (passwordAfterHash == passwordInDb)
                return true;

            return false;
        }

        public User? GetUserWithRecoveryGuid(Guid guid)
        {
            User? user = _userRepository.GetUserByRecoveryGuid(guid);
            return user;
        }

        public async Task<ClaimsIdentity?> ValidateAndCreateClaim(string password, string email)
        {
            if (ValidateUser(password, email))
            {
                User user = _userRepository.GetUserByEmail(email);

                var claims = new List<Claim>()
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", email),
                    new Claim("RoleName", user.RoleName),
                    new Claim("UserStatus", user.UserStatus)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return claimsIdentity;
            }

            return null;
        }

        public async Task<bool> CheckPasswordRecoveryGuid(Guid guid, string email)
        {
            Guid? userGuid = _userRepository.GetUserGuidByEmail(email);

            if (guid == userGuid)
                return true;

            return false;
        }

        public async Task<bool> ConfirmUser(Guid guid)
        {
            try
            {
                User? user = _userRepository.GetUserByConfirmationGuid(guid);
                
                if (user != null && user.ConfirmationGuid == guid)
                {
                    user.UserStatus = UserStatus.ACTIVE.ToString();
                    _userRepository.UpdateAndSaveChanges(user);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}