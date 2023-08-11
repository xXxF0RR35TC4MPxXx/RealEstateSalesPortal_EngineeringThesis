using Inżynierka.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using Inżynierka.Shared.ViewModels;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.DTOs.Offers;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("VisibilityController")]
    public class VisibilityController : BaseController
    {
        private string? _errorMessage;
        private readonly OfferService _offerService;
        private readonly UserService _userService;
        private readonly VisibilityService _visibilityService;
        public VisibilityController(OfferService offerService, UserService userService, VisibilityService visibilityService)
        {
            _offerService = offerService;
            _userService = userService;
            _visibilityService = visibilityService;
        }


        [HttpGet]
        [Route("CheckVisibility/{offerId}")]
        public IActionResult CheckVisibility(int offerId)
        {
            bool result = _visibilityService.CheckVisibility(offerId, out _errorMessage);
            if (result)
            {
                return Ok(new ResponseViewModel("Visible"));
            }
            else { return BadRequest(new ResponseViewModel("Not visible")); }
        }

        [HttpGet]
        [Route("ChangeVisibility/{offerId}/{restrictText}")]
        public IActionResult ChangeVisibility(int offerId, string restrictText)     //checkbox
        {
            bool restrict = restrictText == "True";
            int userId = GetUserId();
            bool result = _visibilityService.ChangeVisibility(userId, offerId, restrict, out _errorMessage);
            if (result)
            {
                return Ok(new ResponseViewModel("Success"));
            }
            else { return BadRequest(new ResponseViewModel(_errorMessage)); }
        }
        
        [HttpGet]
        [Route("AcceptedUsers/{offerId}")]
        public IActionResult AcceptedUsers(int offerId)
        {
            int userId = GetUserId();
            IList<OfferVisibleForUserDTO>? results = _visibilityService.GetAcceptedUsers(userId, offerId, out _errorMessage);
            if (results!=null)
            {
                return Ok(results);
            }
            else { return BadRequest(new ResponseViewModel(_errorMessage)); }
        }

        [HttpGet]
        [Route("AddUserToDetails/{offerId}/{userEmail}")]
        public IActionResult AddUserToDetails(int offerId, string userEmail)          //create
        {
            int ownerId = GetUserId();
            bool result = _visibilityService.AddUserToDetails(ownerId, offerId, userEmail, out _errorMessage);
            if (result)
            {
                return Ok(new ResponseViewModel("Success"));
            }
            else { return BadRequest(new ResponseViewModel(_errorMessage)); }
        }


        [HttpGet]
        [Route("RemoveDetailsFromUser/{offerId}/{userId}")]
        public IActionResult RemoveUserFromDetails(int offerId, int userId)         //delete
        {
            int ownerId = GetUserId();
            bool result = _visibilityService.RemoveUserFromDetails(ownerId, offerId, userId, out _errorMessage);
            if (result)
            {
                return Ok(new ResponseViewModel("Success"));
            }
            else { return BadRequest(new ResponseViewModel(_errorMessage)); }

        }

        [HttpGet]
        [Route("CheckIfUserAbleToSeeDetails/{offerId}/{userId}")]
        public IActionResult CheckIfUserAbleToSeeDetails(int? offerId, int? userId)       //read
        {
            if(offerId == null)
            {
                return BadRequest(false);
            }

            GetTypeDTO? typeDTO = _offerService.GetTypeOfOffer(offerId.Value);
            if (typeDTO == null) return BadRequest(false);

            Offer? offer = _offerService.GetOfferForUpdate(offerId.Value);
            if (offer == null) return BadRequest(false);
            
           

            //dla właściciela ma być zawsze dostępne
            if (offer.SellerId == GetUserId()) return Ok(true);

            //niezalogowany i dostęp swobodny
            if (userId == null && offer.VisibilityRestricted == true)
            {
                return BadRequest(false);
            }

            //jeśli widoczność nieograniczona to zwróć true
            if (offer.VisibilityRestricted == false) return Ok(true);


            //jeżeli takiego usera nie ma
            UserDTO? user = _userService.Get(userId.Value);
            if (user == null) return BadRequest(false);

            //jeżeli widoczność jest ograniczona, ale jestem userem, który ma to widzieć
            else if (offer.VisibilityRestricted == true && _visibilityService.CheckIfEligible(offerId.Value, userId.Value))
            {
                return Ok(true);
            }

            //jeżeli widoczność ograniczona i mam tego nie widzieć
            else return BadRequest(false);
        }
        
        [HttpGet]
        [Route("CheckIfUserAbleToSeeDetails/{offerId}/")]
        public IActionResult CheckIfUserAbleToSeeDetails(int? offerId)       //read
        {
            if(offerId == null)
            {
                return BadRequest(false);
            }

            GetTypeDTO? typeDTO = _offerService.GetTypeOfOffer(offerId.Value);
            if (typeDTO == null) return BadRequest(false);

            Offer? offer = _offerService.GetOfferForUpdate(offerId.Value);
            if (offer == null) return BadRequest(false);
            

            //niezalogowany i dostęp swobodny
            if (offer.VisibilityRestricted == true)
            {
                return BadRequest(false);
            }

            //jeżeli widoczność ograniczona i mam tego nie widzieć
            else return Ok(true);
        }
    }
}