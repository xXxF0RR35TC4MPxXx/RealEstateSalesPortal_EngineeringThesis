using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Inżynierka_Services.Services;
using Microsoft.Extensions.Logging;


namespace Inżynierka.UnitTests.UserTests
{
    public class BaseUserServiceTests
    {
        protected UserService testedUserService;
        protected Mock<IUserRepository> UserRepositoryMock = new();
        protected Mock<IAgencyRepository> AgencyRepositoryMock = new();
        protected Mock<ILogger<UserService>> LoggerMock = new();
        protected string errorMessage;
        public Agency testAgency;
        public User testUser;
        protected BaseUserServiceTests()
        {
            testedUserService = new UserService(LoggerMock.Object, UserRepositoryMock.Object, AgencyRepositoryMock.Object);

            testUser = new()
            {
                Id = 1,
                Email = "test@agency.com",
                UserStatus = "ACTIVE",
                RoleName = "AGENCY",
                OwnerOfAgencyId = 1
            };

            testAgency = new()
            {
                Id = 1,
                OwnerId = testUser.Id,
                AgencyName = "Name",
                Email = "test@mail.com",
                PhoneNumber = "111111111",
                City = "City",
                Address = "Address",
                InvitationGuid = Guid.NewGuid(),
                PostalCode = "11-111",
                Description = "test",
                NIP = "1231231231",
                REGON = "123123123",
                CreatedDate = DateTime.Now.AddDays(-1)
            };
        }
    }
}
