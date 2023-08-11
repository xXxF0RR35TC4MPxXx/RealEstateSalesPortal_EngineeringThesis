using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Inżynierka_Common.Listing;
using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka_Services.Listing;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using PagedList;
using Inżynierka.Shared.ViewModels.User;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IAgencyRepository _agencyRepository;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IAgencyRepository agencyRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _agencyRepository = agencyRepository;
        }

        public Guid SetUserRecoveryGuid(string email)
        {
            var user = _userRepository.GetUserByEmail(email);

            user.PasswordRecoveryGuid = Guid.NewGuid();

            try
            {
                _userRepository.UpdateAndSaveChanges(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return user.PasswordRecoveryGuid;
        }

        public User? GetUserByEmail(string email)
        {
            User? user = _userRepository.GetUserByEmail(email);
            return user;
            
        }

        public UserDTO Get(int userId)
        {
            User? user = _userRepository.GetById(userId);
            if (user == null)
            {
                return null;
            }

            UserDTO userDTO = new(user.Id, user.FirstName + " " + user.LastName, user.Email, user.UserStatus, user.RoleName, user.OwnerOfAgencyId);

            return userDTO;
        }

        public UserListing GetUsers(Paging paging, SortOrder? sortOrder, UserFilteringDTO userFiltringDTO)
        {
            IQueryable<User> users = _userRepository.GetAllUsers();
            users = users.Where(u => !u.DeletedDate.HasValue);

            if (!String.IsNullOrEmpty(userFiltringDTO.Email))
            {
                users = users.Where(s => s.Email.Contains(userFiltringDTO.Email));
            }
            if (!String.IsNullOrEmpty(userFiltringDTO.UserStatus))
            {
                users = users.Where(s => s.UserStatus.Equals(userFiltringDTO.UserStatus));
            }
            if (!String.IsNullOrEmpty(userFiltringDTO.RoleName))
            {
                users = users.Where(s => s.RoleName.Equals(userFiltringDTO.RoleName));
            }

            if (sortOrder != null && sortOrder.Sort != null)
            {
                users = Sorter<User>.Sort(users, sortOrder.Sort);
            }
            else
            {
                sortOrder = new SortOrder();
                sortOrder.Sort = new List<KeyValuePair<string, string>>();
                sortOrder.Sort.Add(new KeyValuePair<string, string>("Id", ""));

                users = Sorter<User>.Sort(users, sortOrder.Sort);
            }

            UserListing userListing = new();
            userListing.TotalCount = users.Count();
            userListing.UserFilteringDTO = userFiltringDTO;
            userListing.Paging = paging;
            userListing.SortOrder = sortOrder;
            userListing.UserDTOs = users
                .Select(x => new UserDTO(x.Id, $"{x.FirstName} {x.LastName}", x.Email, x.UserStatus, x.RoleName, x.OwnerOfAgencyId))
                .ToPagedList(paging.PageNumber, paging.PageSize);

            return userListing;
        }

        
        public bool Update(UserUpdateDTO userEdit, out string error)
        {
            User? user = _userRepository.GetById(userEdit.Id);

            if (user == null)
            {
                error = ErrorMessageHelper.NoUser;
                return false;
            }


            if ((userEdit.FirstName != null && !Regex.IsMatch(userEdit.FirstName, @"^[żźćńółęąśŻŹĆĄŚĘŁÓŃa-zA-Z-\s]+$")) || (userEdit.LastName != null && !Regex.IsMatch(userEdit.LastName, @"^[żźćńółęąśŻŹĆĄŚĘŁÓŃa-zA-Z-\s]+$")))
            {
                error = ErrorMessageHelper.ForbiddenSymbol;
                return false;
            }

            if(userEdit.FirstName!=null) 
                user.FirstName = userEdit.FirstName;

            if (userEdit.LastName != null) 
                user.LastName = userEdit.LastName;

            if (userEdit.Description != null)
                user.Description = userEdit.Description;

            if (userEdit.PhoneNumber != null)
                user.PhoneNumber = userEdit.PhoneNumber;

            user.Street = userEdit.Street;
            user.City = userEdit.City;
            user.PostalCode = userEdit.PostalCode;

            user.LastUpdatedDate = DateTime.UtcNow;

            try
            {
                _userRepository.UpdateAndSaveChanges(user);
            }
            catch (Exception ex)
            {
                error = ErrorMessageHelper.ErrorUpdatingUser;
                _logger.LogError(ex.Message);
                return false;
            }
            error = "";
            return true;
        }

        public UserUpdateViewModel? GetUserUpdateViewModel(int userId)
        {
            User? user = _userRepository.GetById(userId);
            if(user == null) { return null; }

            UserUpdateViewModel result = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street
            };
            return result;
        }

        public async Task<(bool UploadSuccessful, string errorMessage)> UploadUserAvatar(string? photo, int UserId, string imgPath)
        {
            //imgPath= Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Avatars")
            imgPath = Path.Combine(imgPath, UserId.ToString());

            //if there are no photos to be added
            if (photo == null)
            {
                return (true, "User information updated!");
            }

            //if the file is valid
            if (photo.Length > 0)
            {
                //create directory if there is none
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }

                //set the next name
                string filenameWithExtension = "avatar.jpg";
                var path = Path.Combine(imgPath, filenameWithExtension);

                //get list of files in offers directory
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                File.WriteAllBytes($"{path}", Convert.FromBase64String(photo));
                
            }
            else return (false, "User information updated successfully but the file was invalid or corrupted");

            return (true, @"User information updated successfully / Files uploaded successfully");
        }


        public bool Delete(int userId, int loginUserId, out string error)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
            {
                error = ErrorMessageHelper.NoUser;
                return false;
            }

            user.UserStatus = UserStatus.DELETED.ToString();
            user.DeletedById = loginUserId;
            user.DeletedDate = DateTime.UtcNow;

            try
            {
                _userRepository.UpdateAndSaveChanges(user);
            }
            catch (Exception)
            {
                error = ErrorMessageHelper.ErrorDeletingUser;
                _logger.LogError("Error updating user while deleting");
                return false;
            }
            error = "";
            return true;
        }

        public bool CheckIfUserExist(string email)
        {
            bool check = _userRepository.CheckIfUserExist(email);

            return check;
        }

        public async Task<(Guid?, string)> CreateUser(string? AgencyGuid, string firstname, string lastname, string password, string email, string phone, string? postalCode, string? City, string? Street)
        {
            Agency? agency = null;
            //check if any agency with given invitation guid exists, if not return null
            if (AgencyGuid != null && AgencyGuid != "") 
            {
                agency = _agencyRepository.GetAgencyByInvitationGuid(Guid.Parse(AgencyGuid));
                if (agency == null)
                {
                    return (null, ErrorMessageHelper.NoAgencyWithGivenCode);
                }
            }

            User newUser = new()
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                PhoneNumber = phone,
                Password = PasswordHashHelper.GetHash(password),
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                RoleName = agency==null?UserRoles.PRIVATE.ToString():UserRoles.AGENCY.ToString(),
                UserStatus = UserStatus.NOT_VERIFIED.ToString(),
                ConfirmationGuid = Guid.NewGuid(),
                PostalCode = postalCode,
                City = City,
                Street = Street,
                AgentInAgencyId = agency?.Id
            };

            try
            {
                _userRepository.AddAndSaveChanges(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return (newUser.ConfirmationGuid, "");
        }

        public async Task<bool> ChangeUserPassword(string email, string password)
        {
            User myUser = _userRepository.GetUserByEmail(email);
            string passwordAfterHash = PasswordHashHelper.GetHash(password);
            if (myUser.Password == passwordAfterHash) return false;

            _userRepository.ChangeUserPasswordByEmail(email, passwordAfterHash);
            return true;
        }
                
    }
}