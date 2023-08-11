using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserTests
{
    public class CreateUserTests: BaseUserServiceTests
    {
        [Fact]
        public async Task CreateUser_ShouldReturnGuid_ShouldWork()
        {
            string name = "Hugh";
            string surname = "Mungus";
            string password = "password";
            string email = "test@gmail.com";
            Guid expected = Guid.Empty;
            UserRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<User>())).Verifiable();
            AgencyRepositoryMock.Setup(x => x.GetAgencyByInvitationGuid(It.IsAny<Guid>())).Returns(testAgency);
            string agencyGuid = Guid.NewGuid().ToString();

            (Guid?, string) actual = await testedUserService.CreateUser(agencyGuid, name, surname, password, email, "123123123", "", "", "");

            UserRepositoryMock.Verify(x => x.AddAndSaveChanges(It.IsAny<User>()), Times.Once);
            Assert.False(actual.Item1 == expected);
        }

        [Theory]
        [InlineData("12312312", false)]
        [InlineData("123123123", true)]
        [InlineData("xdxdxdxdd", false)]
        [InlineData("        ", false)]
        [InlineData("646464646", true)]
        [InlineData("x99999999", false)]
        [InlineData("😋😋😋😋😋😋😋😋😋", false)]
        [InlineData("123😋2😋123", false)]
        public void CreateUser_ShouldReturnFalse_WhenPhoneNumberDoesntMatchRegex(string number, bool expectedResult)
        {
            bool actual;

            if (Regex.IsMatch(number, @"^([0-9]{9})$"))
            {
                actual = true;
            }
            else actual = false;

            Assert.True(actual == expectedResult);
        }
    }
}
