using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserTests
{
    public class CheckIfUserExistTests:BaseUserServiceTests
    {
        [Fact]
        public void CheckIfUserExist_ShouldReturnTrue_UserExists()
        {
            string email = "test@gmail.com";

            UserRepositoryMock.Setup(x => x.CheckIfUserExist(email)).Returns(true);

            bool actual = testedUserService.CheckIfUserExist(email);

            UserRepositoryMock.Verify(x => x.CheckIfUserExist(email), Times.Once);
            Assert.True(actual);
        }

        [Fact]
        public void CheckIfUserExists_ShouldReturnFalse_UserDontExists()
        {
            string email = "test@gmail.com";

            UserRepositoryMock.Setup(x => x.CheckIfUserExist(email)).Returns(false);

            bool actual = testedUserService.CheckIfUserExist(email);

            UserRepositoryMock.Verify(x => x.CheckIfUserExist(email), Times.Once);
            Assert.False(actual);
        }
    }
}
