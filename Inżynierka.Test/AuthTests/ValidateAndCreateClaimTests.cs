using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.AuthTests
{
    public class ValidateAndCreateClaimTests : BaseAuthServiceTest
    {
        //SignInTest
        [Fact]
        public async Task ValidateAndCreateClaim_ShouldReturnClaimsIdentity_WhenUserIsValid()
        {
            //arrange
            string userEmail = "user@gmail.com";
            string userPassword = "password";
            string passwordInDb = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

            User user = new()
            {
                Id = 1,
                Email = userEmail,
                UserStatus = "ACTIVE",
                RoleName = "ADMIN"
            };

            List<Claim>? claims = new()
            {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("RoleName", user.RoleName),
                    new Claim("UserStatus", user.UserStatus)
            };

            UserRepositoryMock.Setup(x => x.GetUserPassword(userEmail)).Returns(passwordInDb);
            UserRepositoryMock.Setup(x => x.GetUserByEmail(userEmail)).Returns(user);
            //act
            ClaimsIdentity? claimsIdentity = await testedAuthService.ValidateAndCreateClaim(userPassword, userEmail);

            //assert
            UserRepositoryMock.Verify(x => x.GetUserPassword(userEmail), Times.Once);
            UserRepositoryMock.Verify(x => x.GetUserByEmail(userEmail), Times.Once);
            Assert.True(claimsIdentity.Claims.Count() == claims.Count);
        }

        [Fact]
        public async Task ValidateAndCreateClaim_ShouldReturnNull_WhenUserEmailIsNotInDB()
        {
            //Arrange
            string userEmail = "user@gmail.com";
            string userPassword = "password";
            string? passwordIsNull = null;

            UserRepositoryMock.Setup(x => x.GetUserPassword(userEmail)).Returns(passwordIsNull);
            //Act
            ClaimsIdentity? claimsIdentity = await testedAuthService.ValidateAndCreateClaim(userPassword, userEmail);

            //Assert
            UserRepositoryMock.Verify(x => x.GetUserPassword(userEmail), Times.AtLeastOnce);
            Assert.Null(claimsIdentity);
        }

        [Fact]
        public async Task ValidateAndCreateClaim_ShouldReturnNull_WhenUserPasswordlIsNotValid()
        {
            //Arrange
            string userEmail = "user@gmail.com";
            string userPassword = "wrongPassword";
            string passwordInDb = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

            UserRepositoryMock.Setup(x => x.GetUserPassword(userEmail)).Returns(passwordInDb);
            //Act
            ClaimsIdentity? claimsIdentity = await testedAuthService.ValidateAndCreateClaim(userPassword, userEmail);

            //Assert
            UserRepositoryMock.Verify(x => x.GetUserPassword(userEmail), Times.AtLeastOnce);
            Assert.Null(claimsIdentity);
        }

    }
}
