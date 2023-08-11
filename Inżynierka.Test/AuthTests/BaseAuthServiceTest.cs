
using Inżynierka.Shared.IRepositories;
using Inżynierka_Services.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.AuthTests
{
    public class BaseAuthServiceTest
    {
        protected static readonly Mock<IUserRepository> UserRepositoryMock = new();
        protected static readonly Mock<IAgencyRepository> AgencyRepositoryMock = new();
        protected readonly UserService UserService = new(UserServiceLoggerMock.Object, UserRepositoryMock.Object, AgencyRepositoryMock.Object);
        protected readonly Mock<ILogger<AuthService>> LoggerMock = new();
        protected static readonly Mock<ILogger<UserService>> UserServiceLoggerMock = new();
        protected readonly AuthService testedAuthService;

        protected BaseAuthServiceTest()
        {
            testedAuthService = new AuthService(LoggerMock.Object, UserRepositoryMock.Object);
        }
    }
}
