
using AutoMapper;
using Inżynierka.Server.Profiles;
using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Services.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Inżynierka.UnitTests.AgencyTests
{
    public class BaseAgencyServiceTest
    {
        protected readonly Mock<IUserRepository> UserRepositoryMock = new();
        protected readonly Mock<IOfferRepository> OfferRepositoryMock = new();
        protected readonly Mock<IAgencyRepository> AgencyRepositoryMock = new();
        protected readonly Mock<IUserEventRepository> UserEventRepositoryMock = new();
        protected readonly Mock<ILogger<AgencyService>> LoggerMock = new();
        protected static IMapper _mapper;
        protected readonly AgencyService testedAgencyService;
        protected string errorMessage = "";
        protected Agency agency;
        protected User user;
        protected AgencyUpdateDTO updateDTO;
        protected BaseAgencyServiceTest()
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

            testedAgencyService = new AgencyService(LoggerMock.Object, OfferRepositoryMock.Object, AgencyRepositoryMock.Object, 
                UserRepositoryMock.Object, _mapper, UserEventRepositoryMock.Object);
                        

            updateDTO = new AgencyUpdateDTO()
            {
                AgencyName = "UpdatedName",
                Email = "email@updated.com",
                PhoneNumber = "999999999",
                City = "UpdatedCity",
                Address = "UpdatedAddress",
                PostalCode = "99-999",
                Description = "UpdatedDesc",
                NIP = "9999999999",
                REGON = "999999999",
                LicenceNumber = "999999999",
                UpdatedDate = DateTime.Now
            };

            user = new()
            {
                Id = 1,
                Email = "admin@admin.com",
                UserStatus = "ACTIVE",
                RoleName = "ADMIN"
            };

            agency = new()
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                LicenceNumber = "111111111111111",
                REGON = "111111111",
                NIP = "1111111111",
                Description = "desc",
                PostalCode = "15-333",
                City = "Białystok",
                Address = "Zwierzyniecka 6",
                PhoneNumber = "111111111",
                Email = "test@test.com",
                AgencyName = "TestAgency",
                OwnerId = user.Id,
                Owner = user,
            };
            user.OwnerOfAgencyId = agency.Id;
        }
    }
}
