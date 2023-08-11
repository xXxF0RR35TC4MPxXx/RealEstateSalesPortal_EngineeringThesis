using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using Inżynierka.Shared.ViewModels.Offer.Filtering;
using Inżynierka.Shared.ViewModels;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka_Common.Listing;
using Inżynierka_Services.Listing;
using AutoMapper;
using System.Text.Json;
using Inżynierka.Server.Attributes;
using Inżynierka.Shared.ViewModels.User;
using Inżynierka.Shared.DTOs.User;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("UserController")]
    public class UserController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private UserActionService _userActionService;
        private OfferService _offerService;
        private UserService _userService;
        private IMapper _mapper;
        private string? _errorMessage;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(ILogger<HomeController> logger, UserActionService userActionService, OfferService offerService, IWebHostEnvironment webHostEnvironment, 
            IMapper mapper, UserService userService)
        {
            _logger = logger;
            _userActionService = userActionService;
            _offerService = offerService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Returns a profile of user - contains: list of offers, his information and info about his agency
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Offers, user information and info about agency in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting user account</response>
        /// <response code="200">List of plot offers</response>
        [HttpPost]
        [Route("{userId}")]
        [ProducesResponseType(typeof(UserOfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult? GetDetails(int userId, UserPageFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("UserController", "GetDetails", userId.ToString() + " " + JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            UserOffersFilteringDTO offerFilteringDTO = _mapper.Map<UserOffersFilteringDTO>(offerListFilterViewModel);
            offerFilteringDTO.availableOfferTypes = offerListFilterViewModel.availableOfferTypes;
            offerFilteringDTO.availableEstateType = offerListFilterViewModel.availableEstateType;

            UserOfferListing? offers = (UserOfferListing?)_offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), userId, null);
                       
            if(offers!=null) offers.UserId = userId;
            return Ok(offers);
        }


        /// <summary> Update user information </summary>
        /// <param name="updatedUser">Object of the UserUpdateViewModel class containing new information about the user</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "User updated successfully"</response>
        /// <response code="400">string "Error updating user (check parameters)<br />
        /// string "Error updating user<br />
        /// string "Error updating user in database"
        /// </response>
        [HttpPost]
        [Route("Update")]
        [RequireUserRole("AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateUserProfile(UserUpdateViewModel updatedUser)
        {
            LogUserAction("UserController", "Update", $"{JsonSerializer.Serialize(updatedUser)}", _userActionService);

            int currentUserId = GetUserId();
            if(currentUserId == 0)
            {
                return Forbid("You can't access the user edit page while not being logged in!");
            }

            string message="", uploadResultMessage;

            UserUpdateDTO userUpdateDTO = _mapper.Map<UserUpdateDTO>(updatedUser);
            userUpdateDTO.Id = currentUserId;

            //update the user
            bool result = _userService.Update(userUpdateDTO, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            if(updatedUser.Avatar!=null && updatedUser.Avatar!="")
            {
                (result, uploadResultMessage) = _userService.UploadUserAvatar(updatedUser.Avatar, currentUserId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Avatars")).Result;
                message = Translate(uploadResultMessage);
            }

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }

        [HttpGet("CheckIfAgent/{userId}")]
        public IActionResult CheckIfAgent(int userId)
        {
            UserDTO? user = _userService.Get(userId);
            if (user == null) return BadRequest(false);
            if (user.RoleName != "AGENCY") return BadRequest(false);
            else return Ok(true);
        }

        [HttpGet("CheckIfOwner/{userId}")]
        public IActionResult CheckIfOwner(int userId)
        {
            UserDTO? user = _userService.Get(userId);
            if (user == null) return BadRequest(false);
            if (user.OwnerOfAgencyId != null) return Ok(true);
            else return BadRequest(false);
        }

        /// <summary>
        /// Gets an update view model of currently logged in user
        /// </summary>
        /// <returns>JSON string representing an object of the UserUpdateViewModel class</returns>
        /// <response code="200">Object of UserUpdateViewModel</response>
        /// <response code="400">null</response>
        [HttpGet("GetUpdateViewModel/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        public UserUpdateViewModel? GetUpdateViewModel()
        {
            LogUserAction("UserController", "GetUpdateViewModel", "", _userActionService);
            int currentUserId = GetUserId();
            if (currentUserId == 0)
            {
                return null;
            }

            UserUpdateViewModel? user = _userService.GetUserUpdateViewModel(currentUserId);
            return user;
        }
    }
}