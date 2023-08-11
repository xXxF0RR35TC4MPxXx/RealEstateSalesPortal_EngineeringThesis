using AutoMapper;
using Inżynierka.Server.Profiles;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Inżynierka_Services.Services;
using Microsoft.Extensions.Logging;

namespace Inżynierka.UnitTests.OfferTests
{
    public class BaseOfferServiceTests
    {
        protected readonly Mock<IUserRepository> UserRepositoryMock = new();
        protected readonly Mock<IOfferRepository> OfferRepositoryMock = new();
        protected readonly Mock<IAgencyRepository> AgencyRepositoryMock = new();
        protected readonly Mock<IUserEventRepository> UserEventRepositoryMock = new();
        protected readonly Mock<IUserFavouriteRepository> UserFavouriteRepositoryMock = new();
        protected readonly Mock<ILogger<OfferService>> LoggerMock = new();
        protected static IMapper _mapper;
        protected readonly OfferService testedOfferService;
        protected string errorMessage = "";
        public User user1;
        public User user2;
        public Offer offer1;
        public Agency agency;
        public HouseSaleOffer houseSaleOffer;
        public HouseSaleOffer houseSaleOffer2;
        
        protected BaseOfferServiceTests()
        {
            
           
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

            
            
            
            agency = new Agency() {Id=1, AgencyName="test" };
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
                RoleName = "ADMIN",
                AgentInAgencyId = 1,
            };
            offer1 = new() { Id = 1, Area = 100, Price = 1000, SellerId = 1, Seller=user1};
            houseSaleOffer = new() { Id = 1, Area = 100, Price = 1000, SellerId = 1, Seller=user1};
            houseSaleOffer2 = new() { Id = 1, Area = 100, Price = 1000, SellerId = 2, Seller = user2 };

            testedOfferService = new OfferService(LoggerMock.Object, OfferRepositoryMock.Object, 
                AgencyRepositoryMock.Object, _mapper, UserFavouriteRepositoryMock.Object, UserRepositoryMock.Object);
        }
    }
}
