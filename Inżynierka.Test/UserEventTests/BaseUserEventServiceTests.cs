using AutoMapper;
using Inżynierka.Server.Profiles;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Inżynierka_Services.Services;
using Microsoft.Extensions.Logging;

namespace Inżynierka.UnitTests.UserEventTests
{
    public class BaseUserEventServiceTests
    {
        protected readonly Mock<IUserRepository> UserRepositoryMock = new();
        protected readonly Mock<IOfferRepository> OfferRepositoryMock = new();
        protected readonly Mock<IAgencyRepository> AgencyRepositoryMock = new();
        protected readonly Mock<IUserEventRepository> UserEventRepositoryMock = new();
        protected readonly Mock<IUserFavouriteRepository> UserFavouriteRepositoryMock = new();
        protected readonly Mock<ILogger<UserEventService>> LoggerMock = new();
        protected readonly Mock<ILogger<OfferService>> OfferLoggerMock = new();
        protected static IMapper _mapper;
        protected readonly UserEventService testedUserEventService;
        protected string errorMessage = "";
        public User user1;
        public User user2;
        public Offer offer1;
        public UserEvent userEvent1;
        public UserEvent userEvent2;
        public UserEvent userEvent3;
        public UserEvent userEvent4;
        public UserEvent deletedUserEvent;
        public UserEvent notMyUserEvent;
        public UserEventCreateDTO userEventCreateDTO;
        public UserEventUpdateDTO userEventUpdateDTO;
        public UserEventUpdateDTO userEventUpdateDTO2;
        public IEnumerable<UserEvent> userEvents;
        public IEnumerable<UserEvent> userEventsNotToday;
        public List<UserEvent> userEventsList;
        public List<UserEvent> userEventsListNotToday;
        public OfferService _offerService;
        protected BaseUserEventServiceTests()
        {
            _offerService = new OfferService(OfferLoggerMock.Object, OfferRepositoryMock.Object, AgencyRepositoryMock.Object, _mapper,
            UserFavouriteRepositoryMock.Object, UserRepositoryMock.Object);


            userEventCreateDTO = new UserEventCreateDTO(1, 1, "ClientName", "Client@mail.com", "123123123", "EventName", 
                EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW), DateTime.Now.AddDays(1));

            userEventUpdateDTO = new UserEventUpdateDTO("NewClientName", "new@mail.com", "321321321", "NewEventName", EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.DURING), DateTime.Now.AddDays(2));
            userEventUpdateDTO2 = new UserEventUpdateDTO() { ClientName = "NewClientName" };

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AgencyProfile());
                    mc.AddProfile(new UserProfile());
                    mc.AddProfile(new OfferProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            offer1 = new() { Id = 1};

            user1 = new()
            {
                Id = 1,
                Email = "admin@admin.com",
                UserStatus = "ACTIVE",
                RoleName = "ADMIN"
            };

            user2 = new()
            {
                Id = 2,
                Email = "admin2@admin2.com",
                UserStatus = "ACTIVE",
                RoleName = "ADMIN"
            };

            userEvent1 = new()
            {
                Id = 1,
                OfferId = offer1.Id,
                SellerId = user1.Id,
                Seller = user1,
                Offer = offer1,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName1",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now.AddDays(100)
            };

            userEvent4 = new()
            {
                Id = 1,
                OfferId = offer1.Id,
                SellerId = user1.Id,
                Seller = user1,
                Offer = offer1,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName1",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now.AddDays(100)
            };

            userEvent2 = new()
            {
                Id = 1,
                OfferId = offer1.Id,
                SellerId = user1.Id,
                Seller = user1,
                Offer = offer1,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName2",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now
            };

            userEvent3 = new()
            {
                Id = 1,
                OfferId = offer1.Id,
                SellerId = user1.Id,
                Seller = user1,
                Offer = offer1,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName3",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now
            };

            userEventsList = new() { userEvent1, userEvent2, userEvent3, };
            userEvents = userEventsList.AsEnumerable();

            userEventsListNotToday = new() { userEvent1, userEvent4 };
            userEventsNotToday = userEventsList.AsEnumerable();

            deletedUserEvent = new()
            {
                Id = 2,
                OfferId = offer1.Id,
                SellerId = user1.Id,
                Seller = user1,
                Offer = offer1,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now.AddDays(100),
                DeletedDate = DateTime.Now.AddDays(-1)
            };

            notMyUserEvent = new()
            {
                Id = 3,
                OfferId = offer1.Id,
                SellerId = user2.Id,
                Seller = user2,
                Offer = offer1,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now.AddDays(100)
            };

            testedUserEventService = new UserEventService(UserEventRepositoryMock.Object, LoggerMock.Object, _mapper, _offerService);
        }
    }
}
