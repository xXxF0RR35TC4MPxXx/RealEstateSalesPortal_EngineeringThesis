using Inżynierka_Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using Inżynierka.Shared.ViewModels;
using Inżynierka_Services.Listing;
using AutoMapper;
using System.Text.Json;
using Inżynierka.Server.Attributes;
using Inżynierka_Common.Enums;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.ViewModels.UserEvents;
using Inżynierka.Shared.ViewModels.User;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("UserEventsController")]
    public class UserEventController : BaseController
    {
        private readonly UserActionService _userActionService;
        private readonly UserEventService _userEventService;
        private readonly IMapper _mapper;
        private string? _errorMessage;
        public UserEventController(UserActionService userActionService, IMapper mapper, UserEventService userEventService)
        {
            _userActionService = userActionService;
            _userEventService = userEventService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of users events
        /// </summary>
        /// <param name="eventListViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of user events in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting user events</response>
        /// <response code="200">List of user events</response>
        [HttpGet]
        [Route("GetAll")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(UserOfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetMyEvents()
        {
            int userId = GetUserId();
            LogUserAction("UserEventController", "GetMyEvents", userId.ToString(), _userActionService);

            IEnumerable<ReadEventDTO>? events = _userEventService.GetEvents(userId, out _errorMessage);
            return Ok(events);
        }

        /// <summary>
        /// Returns a single user event
        /// </summary>
        /// <param name="eventId">Id of the event</param>
        /// <returns>Object of the JsonResult class representing the single user event in JSON format</returns>
        /// <response code="200">User event information</response>
        /// <response code="400">Error getting user event</response>
        [HttpPost]
        [Route("User/Events/{eventId}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(ReadEventDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetEvent(int eventId)
        {
            LogUserAction("UserEventController", "GetEvent", eventId.ToString(), _userActionService);

            int userId = GetUserId();
            ReadEventDTO? @event = _userEventService.Get(userId, eventId, out _errorMessage);

            if (@event == null)
            {
                string message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            return new JsonResult(@event);
        }

        [HttpGet]
        [Route("GetUpdateViewModel/{id}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(UserEventUpdateViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public UserEventUpdateViewModel? GetUpdateViewModel(int id)
        {
            LogUserAction("UserEventController", "GetUpdateViewModel", "", _userActionService);
            int currentUserId = GetUserId();
            if (currentUserId == 0)
            {
                return null;
            }

            UserEventUpdateViewModel? model = _userEventService.GetUserEventUpdateViewModel(currentUserId, id);
            return model;
        }


        /// <summary>
        /// Delete an event specified by given ID
        /// </summary>
        /// <param name="eventId">ID of the event</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Event deleted successfully</response>
        /// <response code="400">Error deleting event or there is no event with such Id</response>
        [HttpGet]
        [Route("Delete/{eventId}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult DeleteEvent(int eventId)
        {
            LogUserAction("UserEventController", "Delete", eventId.ToString(), _userActionService);
            string message; 
            int userId = GetUserId();
            ReadEventDTO? @event = _userEventService.Get(userId, eventId, out _errorMessage);

            if (@event == null)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            UserEventDeleteDTO dto = new(eventId)
            {
                DeletedDate = DateTime.Now
            };

            bool result = _userEventService.DeleteEvent(dto, userId, out _errorMessage);
            if (result == false)
            {
                message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            message = Translate(MessageHelper.UserEventDeletedSuccessfully);

            return Ok(new ResponseViewModel(message));
        }

        /// <summary> Create a new user event</summary>
        /// <param name="newEvent">Object of the UserEventCreateViewModel class containing information about the new user event</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "User event created successfully"</response>
        /// <response code="400">string "Error creating user event (check parameters)<br />
        /// string "Error creating user event<br />
        /// string "Error saving user event in database"
        /// </response>
        [HttpPost]
        [Route("Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateUserEvent(UserEventCreateViewModel newEvent)
        {
            LogUserAction("UserEventController", "Create", JsonSerializer.Serialize(newEvent), _userActionService);

            int userId = GetUserId();
            UserEventCreateDTO userEventCreateDTO = _mapper.Map<UserEventCreateDTO>(newEvent);
            userEventCreateDTO.EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW);
            userEventCreateDTO.SellerId = userId;

            //create a new event
            bool result = _userEventService.Create(userEventCreateDTO, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }


        /// <summary> Update a user event </summary>
        /// <param name="updatedEvent">Object of the UserEventUpdateViewModel class containing information about the updated Event</param>
        /// <param name="eventId">Id of updated event</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Event updated successfully"</response>
        /// <response code="400">string "Error updating Event (check parameters)<br />
        /// string "Error updating Event<br />
        /// string "Error updating Event in database"
        /// </response>
        [HttpPost]
        [Route("Update/{eventId}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateUserEvent(UserEventUpdateViewModel updatedEvent, int eventId)
        {
            LogUserAction("UserEventController", "Update", $"{eventId}, {JsonSerializer.Serialize(updatedEvent)}", _userActionService);

            int userId = GetUserId();
            ReadEventDTO? @event = _userEventService.Get(userId, eventId, out _errorMessage);
           
            if (@event==null)
            {
                string message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            UserEventUpdateDTO userEventUpdateDTO = _mapper.Map<UserEventUpdateDTO>(updatedEvent);
            userEventUpdateDTO.EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(updatedEvent.EventCompletionStatus);
            //update the event
            bool result = _userEventService.Update(userEventUpdateDTO, eventId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Checks if there are any user events available today </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Amount of events today"</response>
        /// <response code="400">string "Error getting list of events"
        /// </response>
        [HttpGet]
        [Route("TodayEventsCount")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public int CheckIfAnyEventsToday()
        {
            LogUserAction("UserEventController", "CheckIfAnyEventsToday", "", _userActionService);

            int userId = GetUserId();
            int eventCount = _userEventService.GetTodayEventsCount(userId, out _errorMessage);

            return eventCount;
        }
    }
}