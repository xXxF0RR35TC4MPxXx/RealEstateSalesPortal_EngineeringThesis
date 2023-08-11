namespace Inżynierka.UnitTests.AuthTests
{
    public class CheckPasswordRecoveryGuidTests : BaseAuthServiceTest
    {

        [Fact]
        public async Task CheckPasswordRecoveryGuid_ShouldReturnTrue_WhenUserInDBAndGuidAreTheSame()
        {
            //Arrange
            Guid inputGuid = Guid.NewGuid();
            string inputEmail = "test@gmail.com";
            Guid? userGuidInDb = inputGuid;

            UserRepositoryMock.Setup(x => x.GetUserGuidByEmail(inputEmail)).Returns(userGuidInDb);

            //Act
            bool check = await testedAuthService.CheckPasswordRecoveryGuid(inputGuid, inputEmail);
            //Assert
            UserRepositoryMock.Verify(x => x.GetUserGuidByEmail(inputEmail), Times.AtLeastOnce);
            Assert.True(check);
        }

        [Fact]
        public async Task CheckPasswordRecoveryGuid_ShouldReturnFalse_WhenUserGuidInDBAndGuidAreNotTheSame()
        {
            //Arrange
            Guid inputGuid = Guid.NewGuid();
            string inputEmail = "test@gmail.com";
            Guid? userGuidInDb = Guid.NewGuid();

            UserRepositoryMock.Setup(x => x.GetUserGuidByEmail(inputEmail)).Returns(userGuidInDb);

            //Act
            bool check = await testedAuthService.CheckPasswordRecoveryGuid(inputGuid, inputEmail);
            //Assert
            UserRepositoryMock.Verify(x => x.GetUserGuidByEmail(inputEmail), Times.AtLeastOnce);
            Assert.False(check);
        }

        [Fact]
        public async Task CheckPasswordRecoveryGuid_ShouldReturnFalse_WhenUserIsNotInDB()
        {
            //Arrange
            Guid inputGuid = Guid.NewGuid();
            string inputEmail = "test@gmail.com";
            Guid? guidInDbIsNull = null;

            UserRepositoryMock.Setup(x => x.GetUserGuidByEmail(inputEmail)).Returns(guidInDbIsNull);

            //Act
            bool check = await testedAuthService.CheckPasswordRecoveryGuid(inputGuid, inputEmail);
            //Assert
            UserRepositoryMock.Verify(x => x.GetUserGuidByEmail(inputEmail), Times.Exactly(3));
            Assert.False(check);
        }
    }
}
