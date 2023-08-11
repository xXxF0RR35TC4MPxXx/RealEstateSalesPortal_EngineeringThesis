using Inżynierka_Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using Inżynierka.Shared.ViewModels;
using Inżynierka_Services.Listing;
using AutoMapper;
using System.Text.Json;
using Inżynierka.Server.Attributes;
using Inżynierka.Shared.ViewModels.UserPreferenceForm;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using Microsoft.AspNetCore.Http.Extensions;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("UserPreferenceController")]
    public class UserPreferenceFormController : BaseController
    {
        private readonly UserActionService _userActionService;
        private readonly UserPreferenceFormService _userPreferenceFormService;
        private readonly IMapper _mapper;
        private string? _errorMessage;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWebHostBuilder _webHostBuilder;
        public UserPreferenceFormController(UserActionService userActionService, IMapper mapper, UserPreferenceFormService userPreferenceService, IWebHostEnvironment env, IWebHostBuilder builder)
        {
            _userActionService = userActionService;
            _userPreferenceFormService = userPreferenceService;
            _mapper = mapper;
            _webHostEnvironment = env;
            _webHostBuilder = builder;
        }

        /// <summary> Create a new user preference form </summary>
        /// <param name="newFormViewModel">Object of the UserPreferenceFormCreateViewModel class containing information about the new user preference form</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "User preference form created successfully"</response>
        /// <response code="400">string "Error creating user preference form (check parameters)<br />
        /// string "Error creating user preference form<br />
        /// string "Error saving user preference form in database"
        /// </response>
        [HttpPost]
        [Route("Create/")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult Create(UserPreferenceFormCreateViewModel newFormViewModel)
        {
            LogUserAction("UserPreferenceFormController", "Create", JsonSerializer.Serialize(newFormViewModel), _userActionService);
                       
            //create a new form
            bool result = _userPreferenceFormService.Create(newFormViewModel, out _errorMessage);

            return (result == false) ? NotFound(new ResponseViewModel(_errorMessage)) : Ok(new ResponseViewModel(_errorMessage));
        }

        /// <summary>
        /// Returns a list of agents user preference forms
        /// </summary>
        /// <returns>Object of the JsonResult class representing the list of user preference forms in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting user preference forms</response>
        /// <response code="200">List of user preference forms</response>
        [HttpGet]
        [Route("MyForms/")]
        [RequireUserRole("AGENCY")]
        [ProducesResponseType(typeof(List<UserPreferenceFormThumbnailDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetMyForms()
        {
            int agentId = GetUserId();
            LogUserAction("UserPreferenceFormController", "MyForms", agentId.ToString(), _userActionService);

            List<UserPreferenceFormThumbnailDTO>? forms = _userPreferenceFormService.GetMyForms(agentId, out _errorMessage);

            if (forms == null)
            {
                string message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(forms);
        }

        //single form
        /// <summary>
        /// Returns a single form - contains: list of suggested offers
        /// </summary>
        /// <param name="formId">Id of form</param>
        /// <returns>Object of the JsonResult class representing the list of Offers and form details in JSON format</returns>
        /// <response code="400">Error getting the form</response>
        /// <response code="200">List of offers and user preference details</response>
        [HttpGet]
        [RequireUserRole("AGENCY")]
        [Route("MyForms/{formId}")]
        [ProducesResponseType(typeof(FormPageListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Get(int formId)
        {
            LogUserAction("UserPreferenceFormController", "MyForms/", formId.ToString(), _userActionService);

            var userId = GetUserId();

            FormPageListing? form = _userPreferenceFormService.GetForm(false, formId, userId, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), out _errorMessage);

            if (form == null)
            {
                return BadRequest(new ResponseViewModel(Translate(_errorMessage)));
            }

            return Ok(form);
        }

        /// <summary>
        /// Returns a single form - contains: list of suggested offers
        /// </summary>
        /// <param name="formId">Id of form</param>
        /// <returns>Object of the JsonResult class representing the list of Offers and form details in JSON format</returns>
        /// <response code="400">Error getting the form</response>
        /// <response code="200">List of offers and user preference details</response>
        [HttpGet]
        [Route("GetReplyData/{formId}")]
        [ProducesResponseType(typeof(FormPageListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetForReply(int formId)
        {
            LogUserAction("UserPreferenceFormController", "MyForms/", formId.ToString(), _userActionService);
            
            FormPageListing? form = _userPreferenceFormService.GetForm(true, formId, 0, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), out _errorMessage);
            
            if (form == null)
            {
                return BadRequest(new ResponseViewModel(Translate(_errorMessage)));
            }

            return Ok(form);
        }

        /// <summary>
        /// Adds an offer as a suggestion to user preference form
        /// </summary>
        /// <param name="formId">Id of form</param>
        /// <param name="offerId">Id of offer</param>
        /// <returns>Object of the JsonResult class representing the list of Offers and form details in JSON format</returns>
        /// <response code="400">Error adding the suggestion to form</response>
        /// <response code="200">Suggestion added successfully</response>
        [HttpGet]
        [RequireUserRole("AGENCY")]
        [Route("AddSuggestion/{formId}/{offerId}")]
        [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult AddAsSuggestion(int formId, int offerId)
        {
            LogUserAction("UserPreferenceFormController", "AddAsSuggestion", formId.ToString() + "/" + offerId.ToString(), _userActionService);
            int userId = GetUserId();
            bool form = _userPreferenceFormService.AddAsSuggestion(userId, formId, offerId, out _errorMessage);

            if (form == false)
            {
                return BadRequest(new ResponseViewModel(Translate(_errorMessage)));
            }

            return Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary>
        /// Remove an offer from suggestions to user preference form
        /// </summary>
        /// <param name="formId">Id of form</param>
        /// <param name="offerId">Id of offer</param>
        /// <returns>String response</returns>
        /// <response code="400">Error removing suggestion from the form</response>
        /// <response code="200">Suggestion removed</response>
        [HttpGet]
        [RequireUserRole("AGENCY")]
        [Route("RemoveSuggestion/{formId}/{offerId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult RemoveSuggestion(int formId, int offerId)
        {
            LogUserAction("UserPreferenceFormController", "RemoveSuggestion", formId.ToString() + "/" + offerId.ToString(), _userActionService);

            int userId = GetUserId();

            bool form = _userPreferenceFormService.RemoveSuggestion(formId, offerId, userId, out _errorMessage);

            if (form == false)
            {
                return BadRequest(new ResponseViewModel(Translate(_errorMessage)));
            }

            return Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary>
        /// Sends an email with a list of suggestions and a reply form
        /// </summary>
        /// <param name="formId">Id of form</param>
        /// <returns>String response</returns>
        /// <response code="400">Error sending reply to user</response>
        /// <response code="200">Reply sent successfully</response>
        [HttpGet]
        [RequireUserRole("AGENCY")]
        [Route("SendSuggestionEmail/{formId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SendSuggestion(int formId)
        {
            LogUserAction("UserPreferenceFormController", "SendSuggestion", formId.ToString(), _userActionService);

            int userId = GetUserId();
            string baseUrl = _webHostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey).Split(';')[0];
            bool form = _userPreferenceFormService.SendEmail(baseUrl, formId, userId, Url, out _errorMessage);

            if (form == false)
            {
                return BadRequest(new ResponseViewModel(Translate(_errorMessage)));
            }

            return Ok(new ResponseViewModel(Translate(_errorMessage)));
        }
                
        /// <summary>
        /// Sends an email with a list of suggestions and a reply form
        /// </summary>
        /// <param name="formId">Id of form</param>
        /// <param name="guid">Guid used to confirm the right user replies</param>
        /// <returns>String response</returns>
        /// <response code="400">Error sending reply to user</response>
        /// <response code="200">Reply sent successfully</response>
        [HttpPost]
        [Route("ReplyToSuggestions/{formId}/{guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult ReplyToSuggestions(ReplyToSuggestionsViewModel viewModel, int formId, string guid)
        {
            LogUserAction("UserPreferenceFormController", "ReplyToSuggestions", formId.ToString() + " " + guid.ToString(), _userActionService);
            Guid actualGuid = Guid.Parse(guid);
            bool form = _userPreferenceFormService.ReplyToSuggestions(viewModel.ClientComment, formId, actualGuid, out _errorMessage);

            if (form == false)
            {
                return BadRequest(new ResponseViewModel(Translate(_errorMessage)));
            }

            return Ok(new ResponseViewModel(Translate(_errorMessage)));
        }
    }
}