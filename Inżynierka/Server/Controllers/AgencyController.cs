using Inżynierka_Common.Helpers;
using Inżynierka.Shared.Entities;
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
using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.ViewModels.Agency;
using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.ViewModels.User;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("AgencyController")]
    public class AgencyController : BaseController
    {
        private readonly UserActionService _userActionService;
        private readonly AgencyService _agencyService;
        private readonly IMapper _mapper;
        private string? _errorMessage;
        private OfferService _offerService;
        private UserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AgencyController(UserActionService userActionService, IMapper mapper, AgencyService agencyService, IWebHostEnvironment webHostEnvironment, OfferService offerService, UserService userService)
        {
            _userActionService = userActionService;
            _agencyService = agencyService;
            _mapper = mapper;
            _offerService = offerService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }

        [HttpGet("CheckIfOwner/{viewerId}/{agencyId}")]
        public IActionResult CheckIfOwner(string? viewerId, string? agencyId)
        {
            if(viewerId == null) { return BadRequest(new ResponseViewModel("Użytkownik nie jest zalogowany")); }
            if(agencyId == null) { return BadRequest(new ResponseViewModel("Nie podano identyfikatora agencji")); }
            Agency? agency = _agencyService.GetAgency(Int32.Parse(agencyId));
            if (agency == null) return BadRequest(new ResponseViewModel("Agencja o podanym identyfikatorze nie istnieje"));
            if (agency.DeletedDate != null) return BadRequest(new ResponseViewModel("Wybrana agencja nie istnieje"));
            if (agency.OwnerId != Int32.Parse(viewerId)) return BadRequest(new ResponseViewModel("NIE"));
            else return Ok(new ResponseViewModel("TAK"));
        }

        [HttpGet("CheckIfAgent/{viewerId}/{agencyId}")]
        public IActionResult CheckIfAgent(string? viewerId, string? agencyId)
        {
            //if user is not logged in
            if (viewerId == null) { return BadRequest(new ResponseViewModel("Użytkownik nie jest zalogowany")); }

            //if agency doesn't exist
            Agency? agency = _agencyService.GetAgency(Int32.Parse(agencyId));
            if (agency == null) return BadRequest(new ResponseViewModel("Agencja o podanym identyfikatorze nie istnieje"));
            if (agency.DeletedDate != null) return BadRequest(new ResponseViewModel("Wybrana agencja nie istnieje"));

            //if user doesn't exist
            UserDTO? userDTO = _userService.Get(Int32.Parse(viewerId));
            if(userDTO==null) return BadRequest(new ResponseViewModel("Błąd podczas określenia użytkownika"));

            User? user = _userService.GetUserByEmail(userDTO.Email);
            if (user == null) return BadRequest(new ResponseViewModel("Podany użytkownik nie istnieje"));

            //if user is not an agent in given agency
            if (user.AgentInAgencyId != Int32.Parse(agencyId)) return BadRequest(new ResponseViewModel("NIE"));
            else return Ok(new ResponseViewModel("TAK"));
        }

        [HttpGet("JoinAgencyByCode/{guid}/")]
        public IActionResult JoinAgencyByCode(string? guid)
        {
            int userId = GetUserId();
            Guid outGuid = Guid.Empty;
            bool parseResult = Guid.TryParse(guid, out outGuid);
            if (parseResult == false) { return BadRequest(false); }

            bool inviteResult = _agencyService.JoinAgencyByCode(outGuid, userId);
            if (inviteResult == false) return BadRequest(false);

            return Ok(true);
        }

        //read
        /// <summary>
        /// Returns a profile of agency - contains: list of offers and their information
        /// </summary>
        /// <param name="agencyId">Id of agency</param>
        /// <param name="agencyPageFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Offers and agency information JSON format</returns>
        /// <response code="400">Error getting agency</response>
        /// <response code="200">List of offers and agency info</response>
        [HttpPost]
        [Route("{agencyId}")]
        [ProducesResponseType(typeof(AgencyOfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Get(int agencyId, AgencyPageFilteringViewModel agencyPageFilterViewModel)
        {
            LogUserAction("AgencyController", "Get", agencyId.ToString() + " " + JsonSerializer.Serialize(agencyPageFilterViewModel), _userActionService);

            Paging paging = agencyPageFilterViewModel.Paging;
            SortOrder? sortOrder = agencyPageFilterViewModel.SortOrder;
            AgencyOffersFilteringDTO offerFilteringDTO = _mapper.Map<AgencyOffersFilteringDTO>(agencyPageFilterViewModel);


            AgencyOfferListing? offers = (AgencyOfferListing?)_offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, agencyId);
            if(offers == null) return BadRequest(new ResponseViewModel("Nie znaleziono agencji"));
            return Ok(offers);
        }

        //create

        /// <summary> Create a new agency </summary>
        /// <param name="newAgency">Object of the AgencyCreateViewModel class containing information about the new agency</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Agency created successfully"</response>
        /// <response code="400">string "Error creating agency (check parameters)<br />
        /// string "Error creating agency<br />
        /// string "Error saving agency in database"
        /// </response>
        [HttpPost]
        [Route("Create")]
        [RequireUserRole("PRIVATE", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateAgency(AgencyCreateViewModel newAgency)
        {
            LogUserAction("AgencyController", "Create", JsonSerializer.Serialize(newAgency), _userActionService);

            int ownerId = GetUserId();
            string message, uploadResultMessage;
            bool result;

            AgencyCreateDTO agencyCreateDTO = _mapper.Map<AgencyCreateDTO>(newAgency);
            agencyCreateDTO.CreatedDate = DateTime.Now;
            agencyCreateDTO.InvitationGuid = Guid.NewGuid();

            //create a new Agency
            int? agencyId = _agencyService.CreateAgency(agencyCreateDTO, ownerId, out _errorMessage);

            if (agencyId == null || agencyId == 0)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the logo for the agency we just created
            (result, uploadResultMessage) = _agencyService.UploadAgencyLogo(newAgency.Logo, agencyId.Value, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "AgencyLogo")).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }

        //update
        /// <summary> Update agency information </summary>
        /// <param name="updatedAgency">Object of the AgencyUpdateViewModel class containing new information about the agency</param>
        /// <param name="agencyId">ID of updated agency</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Agency updated successfully"</response>
        /// <response code="400">string "Error updating agency (check parameters)<br />
        /// string "Error updating agency<br />
        /// string "Error updating agency in database"
        /// </response>
        [HttpPost]
        [Route("Update/{agencyId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateAgency(AgencyUpdateViewModel updatedAgency, int agencyId)
        {
            LogUserAction("AgencyController", "Update", $"{JsonSerializer.Serialize(updatedAgency)}", _userActionService);

            int currentUserId = GetUserId();
            if (currentUserId == 0)
            {
                return Forbid("You can't access the agency edit page while not being logged in!");
            }

            string message="", uploadResultMessage;

            AgencyUpdateDTO agencyUpdateDTO = _mapper.Map<AgencyUpdateDTO>(updatedAgency);
            agencyUpdateDTO.UpdatedDate = DateTime.Now;
            agencyUpdateDTO.PostalCode = updatedAgency.PostalCode;

            //update the user
            bool result = _agencyService.Update(agencyId, currentUserId, agencyUpdateDTO, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            if(updatedAgency.Logo!=null && updatedAgency.Logo!="")
            {
                (result, uploadResultMessage) = _agencyService.UploadAgencyLogo(updatedAgency.Logo, agencyId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "AgencyLogo")).Result;
                message = Translate(uploadResultMessage);
            }
            
            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }


        //delete
        /// <summary>
        /// Delete an agency specified by given ID
        /// </summary>
        /// <param name="agencyId">ID of the offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Agency deleted successfully</response>
        /// <response code="400">Error deleting agency or there is no agency with such Id</response>
        [HttpGet]
        [Route("Delete/{agencyId}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int agencyId)
        {
            LogUserAction("AgencyController", "Delete", agencyId.ToString(), _userActionService);
            string message;
            int userId = GetUserId();

            AgencyDeleteDTO dto = new(agencyId)
            {
                LastUpdatedDate = DateTime.Now,
                DeletedDate = DateTime.Now
            };

            bool result = _agencyService.Delete(userId, dto, out _errorMessage);
            if (result == false)
            {
                message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            message = Translate(MessageHelper.AgencyDeletedSuccessfully);

            return Ok(new ResponseViewModel(message));
        }

        /// <summary>
        /// Returns a list of agents
        /// </summary>
        /// <param name="agencyId">Id of agency</param>
        /// <returns>Object of the JsonResult class representing the list of Agents in JSON format</returns>
        /// <response code="400">Error getting list of agents</response>
        /// <response code="200">List of agents</response>
        [HttpGet]
        [Route("{agencyId}/Agents/")]
        [ProducesResponseType(typeof(AgentsListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetAgents(int? agencyId)
        {
            LogUserAction("AgencyController", $"{agencyId}/Agents", (agencyId!=null) ? agencyId.Value.ToString() : "null", _userActionService);
            
            if(agencyId == null)
            {
                return BadRequest(new ResponseViewModel("Nie podano numeru agencji."));
            }

            Agency? agency = _agencyService.GetAgency(agencyId.Value);
            int currentUser = GetUserId();

            UserDTO? userDTO = _userService.Get(currentUser);
            if(userDTO == null) 
            {
                return BadRequest(new ResponseViewModel("Wstęp tylko dla zalogowanych użytkowników serwisu"));
            }

            User? user = _userService.GetUserByEmail(userDTO.Email);
            if (user == null)
            {
                return BadRequest(new ResponseViewModel("Wstęp tylko dla zalogowanych użytkowników serwisu"));
            }

            if (agency!=null && agency.OwnerId != currentUser && agency.Id != user.AgentInAgencyId)
            {
                return BadRequest(new ResponseViewModel(ErrorMessageHelper.CantViewAgentListOfAgency));
            }

            AgentsListing? agents = _agencyService.GetAgents(agencyId, Path.Combine(_webHostEnvironment.ContentRootPath, "img"));

            if (agents == null)
            {
                return BadRequest(new ResponseViewModel(MessageHelper.NoAgentsInThisAgency));
            }

            return Ok(agents);
        }


        /// <summary>
        /// Allows user to leave the current agency IF HE'S AN AGENT - doesn't work for owners
        /// </summary>
        /// <returns>Confirmation</returns>
        /// <response code="400">Error while leaving current agency</response>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("Leave")]
        [ProducesResponseType(typeof(UserOfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult LeaveAgency()
        {
            int userId = GetUserId();
            LogUserAction("AgencyController", "LeaveAgency", userId.ToString(), _userActionService);

            bool result = _agencyService.LeaveAgency(userId, null, out string _errorMessage);

            if (result == false)
            {
                string message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(result);
        }

        /// <summary>
        /// Allows the owner of an agency to remove given agent from the agency
        /// </summary>
        /// <param name="agentId">Id of agent</param>
        /// <returns>Confirmation</returns>
        /// <response code="400">Error while removing the agent from agency</response>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("RemoveAgent/{agentId}")]
        [RequireUserRole("AGENCY")]
        [ProducesResponseType(typeof(UserOfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult RemoveAgent(int agentId)
        {
            LogUserAction("AgencyController", "RemoveAgent", agentId.ToString(), _userActionService);

            bool result = _agencyService.LeaveAgency(agentId, GetUserId(), out string _errorMessage);

            if (result == false)
            {
                string message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(result);
        }


        /// <summary>
        /// Allows the owner of an agency to add given agent to the agency
        /// </summary>
        /// <param name="agentId">Id of agent</param>
        /// <returns>Confirmation</returns>
        /// <response code="400">Error while adding the agent to agency</response>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("AddAgent/{userId}")]
        [RequireUserRole("AGENCY")]
        [ProducesResponseType(typeof(UserOfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult AddAgent(int userId)
        {
            LogUserAction("AgencyController", "AddAgent", userId.ToString(), _userActionService);

            bool result = _agencyService.AddAgent(userId, GetUserId(), out string _errorMessage);

            if (result == false)
            {
                string message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(result);
        }


        /// <summary>
        /// Gets an update view model of selected agency
        /// </summary>
        /// <returns>JSON string representing an object of the AgencyUpdateViewModel class</returns>
        /// <response code="200">Object of AgencyUpdateViewModel</response>
        /// <response code="400">null</response>
        [HttpGet("GetUpdateViewModel")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        public AgencyUpdateViewModel? GetUpdateViewModel()
        {
            LogUserAction("AgencyController", "GetUpdateViewModel", "", _userActionService);
            int currentUserId = GetUserId();
            if (currentUserId == 0)
            {
                return null;
            }

            AgencyUpdateViewModel? user = _agencyService.GetUpdateViewModel(currentUserId);
            return user;
        }
    }
}