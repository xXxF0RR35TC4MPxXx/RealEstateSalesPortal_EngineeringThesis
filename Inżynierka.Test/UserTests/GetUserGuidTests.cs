using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserTests
{
    public class GetUserGuidTests:BaseUserServiceTests
    {
        [Fact]
        public void SetUserRecoveryGuid_ShouldReturnUserRecoveryGuid_ShouldWork()
        {
            string email = "test@gmail.com";

            Guid expected = new();

            User user = new()
            {
                Email = email,
            };

            UserRepositoryMock.Setup(x => x.GetUserByEmail(email)).Returns(user);
            UserRepositoryMock.Setup(x => x.UpdateAndSaveChanges(user)).Verifiable();

            Guid result = testedUserService.SetUserRecoveryGuid(email);

            UserRepositoryMock.Verify(x => x.GetUserByEmail(email), Times.Once());
            UserRepositoryMock.Verify(x => x.UpdateAndSaveChanges(It.IsAny<User>()), Times.Once);
            Assert.NotEqual(expected, result);
        }
    }
}
