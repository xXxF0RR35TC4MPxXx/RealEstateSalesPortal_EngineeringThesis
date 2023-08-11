using AutoMapper;
using Inżynierka.Server.Profiles;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Inżynierka_Services.Services;
using Microsoft.Extensions.Logging;

namespace Inżynierka.UnitTests.UserFavouriteTests
{
    public class BaseUserFavouriteServiceTests
    {
        protected readonly Mock<IUserRepository> UserRepositoryMock = new();
        protected readonly Mock<IUserFavouriteRepository> UserFavouriteRepositoryMock = new();
        protected readonly Mock<IOfferRepository> OfferRepositoryMock = new();
        protected readonly Mock<IAgencyRepository> AgencyRepositoryMock = new();
        protected readonly Mock<IUserEventRepository> UserEventRepositoryMock = new();
        protected readonly Mock<ILogger<OfferService>> LoggerMock = new();
        protected static IMapper _mapper;
        protected readonly OfferService testedOfferService;
        protected string errorMessage = "";
        public User user1;
        public User user2;
        public Offer offer1;
        public Offer offer2;
        public UserFavourite fav1, fav2, fav3;
        protected BaseUserFavouriteServiceTests()
        {
            fav1 = new()
            {
                OfferId = 1,
                UserId = 1,
                LikeDate = DateTime.Now.AddDays(-1)
            };

            fav2 = new()
            {
                OfferId = 2,
                UserId = 1,
                LikeDate = DateTime.Now.AddDays(-2)
            }; 
            
            fav3 = new()
            {
                OfferId = 1,
                UserId = 2,
                LikeDate = DateTime.Now.AddDays(-3)
            };

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
            offer2 = new() { Id = 2};

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

            testedOfferService = new OfferService(LoggerMock.Object, OfferRepositoryMock.Object,
                AgencyRepositoryMock.Object, _mapper, UserFavouriteRepositoryMock.Object, UserRepositoryMock.Object);
        }
    }
}
