using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.AuthTests
{
    public class ConfirmUserTests : BaseAuthServiceTest
    {
        [Fact]
        public void ConfirmUser_ShouldReturnTrue_DbGuidAndInputGuidAreTheSame()
        {
            //Arrange
            Guid inputGuid = Guid.NewGuid();
            User user = new()
            {
                ConfirmationGuid = inputGuid
            };
            UserRepositoryMock.Setup(x => x.GetUserByConfirmationGuid(inputGuid)).Returns(user);
            UserRepositoryMock.Setup(x => x.UpdateAndSaveChanges(user)).Verifiable();

            //Act
            bool check = testedAuthService.ConfirmUser(inputGuid).Result;

            //Assert
            Assert.True(check);
        }

        [Fact]
        public void ConfirmUser_ShouldReturnFalse_DbGuidAndInputGuidAreNotTheSame()
        {
            string inputEmail = "test@gmail.com";
            Guid inputGuid = Guid.NewGuid();

            User user = new()
            {
                ConfirmationGuid = Guid.NewGuid(),
            };

            UserRepositoryMock.Setup(x => x.GetUserByEmail(inputEmail)).Returns(user);

            bool check = testedAuthService.ConfirmUser(inputGuid).Result;

            UserRepositoryMock.Verify(x => x.GetUserByConfirmationGuid(inputGuid), Times.AtLeastOnce);
            Assert.False(check);
        }

        [Fact]
        public void ConfirmUser_ShouldReturnFalse_UserNotInDb()
        {
            string inputEmail = "test@gmail.com";
            Guid inputGuid = Guid.NewGuid();

            User? user = null;

            UserRepositoryMock.Setup(x => x.GetUserByEmail(inputEmail)).Returns(user);

            bool check = testedAuthService.ConfirmUser(inputGuid).Result;

            UserRepositoryMock.Verify(x => x.GetUserByConfirmationGuid(inputGuid), Times.AtLeastOnce);
            Assert.False(check);
        }
    }
}
