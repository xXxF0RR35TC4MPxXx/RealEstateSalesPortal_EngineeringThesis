using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserTests
{
    public class UpdateTests:BaseUserServiceTests
    {
        [Fact]
        public void Update_ShouldReturnUserId_UserExists()
        {
            int userId = 10;
            string name = "Hugh";
            string surname = "Mungus";
            string phoneNumber = "123123123";
            string description = "testDesc";
            string city = "testCity";
            string postalCode = "testCode";
            string street = "testStreet";

            UserUpdateDTO dto = new(userId, phoneNumber, name, surname, description, null, city, postalCode, street);

            User userToUpdate = new()
            {
                Id = userId,
                UserStatus = "ACTIVE",
                RoleName = "PRIVATE",
            };

            UserRepositoryMock.Setup(x => x.GetById(userId)).Returns(userToUpdate);
            UserRepositoryMock.Setup(x => x.UpdateAndSaveChanges(userToUpdate)).Verifiable();

            bool expected = testedUserService.Update(dto, out errorMessage);

            UserRepositoryMock.Verify(x => x.UpdateAndSaveChanges(userToUpdate), Times.Once);
            UserRepositoryMock.Verify(x => x.GetById(userId), Times.Once);

            Assert.True(expected);
        }

        [Fact]
        public void Update_ShouldReturn0_UserNotInDb()
        {
            int userId = 10;
            string name = "John";
            string surname = "Teslaw";
            string phoneNumber = "123123123";
            string description = "testDesc";
            string city = "testCity";
            string postalCode = "testCode";
            string street = "testStreet";

            UserUpdateDTO dto = new(userId, phoneNumber, name, surname, description, null, city, postalCode, street);


            User userToUpdate = null;

            UserRepositoryMock.Setup(x => x.GetById(userId)).Returns(userToUpdate);

            bool result = testedUserService.Update(dto, out errorMessage);

            UserRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
            Assert.False(result);
        }
    }
}
