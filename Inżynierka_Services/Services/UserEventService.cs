using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.DTOs.User;
using Microsoft.Extensions.Logging;
using Inżynierka_Common.Helpers;
using Inżynierka.Shared.IRepositories;
using AutoMapper;
using Inżynierka.Shared.ViewModels.UserEvents;
using Org.BouncyCastle.Bcpg;
using Inżynierka_Common.Enums;
using Inżynierka.Shared.DTOs.Offers;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class UserEventService
    {
        private readonly IUserEventRepository _userEventRepository;
        private readonly ILogger<UserEventService> _logger;
        private readonly OfferService _offerService;
        private readonly IMapper _mapper;

        public UserEventService(IUserEventRepository userEventRepository, ILogger<UserEventService> logger, IMapper mapper, OfferService offerService)
        {
            _userEventRepository = userEventRepository;
            _logger = logger;
            _mapper = mapper;
            _offerService = offerService;
        }

        public UserEventUpdateViewModel? GetUserEventUpdateViewModel(int userId, int id)
        {
            string errorMessage = "";
            ReadEventDTO? userEvent = GetEvents(userId, out errorMessage)?.SingleOrDefault(e=>e.Id == id);
            if (userEvent == null) return null;

            UserEventUpdateViewModel result = new()
            {
                ClientName = userEvent.ClientName,
                ClientEmail = userEvent.ClientEmail,
                ClientPhoneNumber = userEvent.ClientPhoneNumber,
                EventName = userEvent.EventName,
                EventCompletionStatus = EnumHelper.GetEnumFromDescription<EventCompletionStatus>(userEvent.EventCompletionStatus),
                DeadlineDate = userEvent.DeadlineDate
            };

            return result;
        }

        public IEnumerable<ReadEventDTO>? GetEvents(int userId, out string _errorMessage)
        {
            var userEvents = _userEventRepository.GetAllUserEvents(userId);
            if(userEvents==null)
            {
                _errorMessage = MessageHelper.EmptyOfferList;
                return null;
            }

            IEnumerable<ReadEventDTO>? result = userEvents.Select(x => new ReadEventDTO
            {
                EventCompletionStatus = x.EventCompletionStatus,
                Id = x.Id,
                OfferId = x.OfferId,
                SellerId = x.SellerId,
                ClientName = x.ClientName,
                ClientEmail = x.ClientEmail,
                ClientPhoneNumber = x.ClientPhoneNumber,
                EventName = x.EventName,
                DeadlineDate = x.DeadlineDate,
                EndDate = x.DeadlineDate.AddHours(1)
            });

            _errorMessage = "";
            return result;
        }

        public ReadEventDTO? Get(int userId, int eventId, out string _errorMessage)
        {
            UserEvent? userEvent = _userEventRepository.GetById(eventId);
            if (userEvent == null || userEvent.DeletedDate != null)
            {
                _errorMessage = ErrorMessageHelper.NoEvent;
                return null;
            }
            if(userEvent.SellerId != userId)
            {
                _errorMessage = ErrorMessageHelper.NotYourEvent;
                return null;
            }

            ReadEventDTO result = _mapper.Map<ReadEventDTO>(userEvent);

            _errorMessage = "";
            return result;
        }

        public bool DeleteEvent(UserEventDeleteDTO dto, int userId, out string _errorMessage)
        {
            try
            {
                UserEvent? userEvent = _userEventRepository.GetById(dto.Id);

                if (userEvent == null || userEvent.DeletedDate != null)
                {
                    _errorMessage = ErrorMessageHelper.NoEvent;
                    return false;
                }
                else if(userEvent.SellerId != userId)
                {
                    _errorMessage = ErrorMessageHelper.NotYourEvent;
                    return false;
                }
                userEvent.DeletedDate = DateTime.Now;
                _userEventRepository.UpdateAndSaveChanges(userEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorDeletingOffer;
                return false;
            }
            _errorMessage = "";
            return true;
        }

        public bool Update(UserEventUpdateDTO userEventUpdateDTO, int eventId, out string _errorMessage)
        {
            UserEvent? userEvent = _userEventRepository.GetById(eventId);

            if (userEvent == null)
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return false;
            }

            if (userEventUpdateDTO.ClientName != null)
            { 
                userEvent.ClientName = userEventUpdateDTO.ClientName; 
            }
            if (userEventUpdateDTO.ClientEmail != null)
            {
                userEvent.ClientEmail = userEventUpdateDTO.ClientEmail;
            }
            if (userEventUpdateDTO.ClientPhoneNumber != null)
            {
                userEvent.ClientPhoneNumber = userEventUpdateDTO.ClientPhoneNumber;
            }
            if (userEventUpdateDTO.EventName != null)
            {
                userEvent.EventName = userEventUpdateDTO.EventName;
            }
            if (userEventUpdateDTO.EventCompletionStatus != null)
            {
                userEvent.EventCompletionStatus = userEventUpdateDTO.EventCompletionStatus;
            }
            if (userEventUpdateDTO.DeadlineDate != null)
            {
                userEvent.DeadlineDate = (DateTime)userEventUpdateDTO.DeadlineDate;
            }

            try
            {
                if (userEvent != null)
                {
                    _userEventRepository.UpdateAndSaveChanges(userEvent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorUpdatingEvent;
                return false;
            }

            _errorMessage = MessageHelper.UserEventUpdatedSuccessfully;
            return true;
        }

        public bool Create(UserEventCreateDTO userEventCreateDTO, out string _errorMessage)
        {
            GetTypeDTO? type = _offerService.GetTypeOfOffer(userEventCreateDTO.OfferId);
            if(type == null) 
            { 
                _errorMessage = ErrorMessageHelper.NoOffer;
                return false; 
            }

            Offer? offer = _offerService.GetOfferForUpdate(userEventCreateDTO.OfferId);
            if(offer ==null)
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return false;
            }
            UserEvent result = _mapper.Map<UserEvent>(userEventCreateDTO);

            try
            {
                if (result != null)
                {
                    _userEventRepository.AddAndSaveChanges(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorCreatingUserEvent;
                return false;
            }

            _errorMessage = MessageHelper.UserEventCreatedSuccessfully;
            return true;
        }

        public int GetTodayEventsCount(int userId, out string errorMessage)
        {
            IEnumerable<UserEvent>? resultCol;
            try
            {
                resultCol = _userEventRepository.GetAllUserEvents(userId);
            }
            catch (Exception ex)
            {
                errorMessage = ErrorMessageHelper.ErrorGettingUserEvents;
                return 0;
            }
            
            int result=0;
            if(resultCol != null)
            {
                result = resultCol.Where(e => e.DeadlineDate.Day == DateTime.Now.Day
                    && e.DeadlineDate.Month == DateTime.Now.Month
                    && e.DeadlineDate.Year == DateTime.Now.Year
                    && e.DeadlineDate > DateTime.Now).Count();
            }

            errorMessage = "";
            return result;
        }
    }
}