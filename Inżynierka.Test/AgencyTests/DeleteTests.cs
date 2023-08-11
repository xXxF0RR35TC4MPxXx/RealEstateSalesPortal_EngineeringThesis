using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.AgencyTests
{
    //Arange, Act, Assert
    public class DeleteTests : BaseAgencyServiceTest
    {
        [Theory]
        [ClassData(typeof(Delete1TestDataGenerator))]
        public void Delete_ShouldReturnFalse_IfNoAgencyWithGivenIdOrDeletedOrNotMine(Agency? agency, AgencyDeleteDTO deleteDTO, int userId, bool expectedResult)
        {
            AgencyRepositoryMock.Setup(x => x.GetById(deleteDTO.Id)).Returns(agency);
            bool result = testedAgencyService.Delete(userId, deleteDTO, out errorMessage);
            Assert.True(result == expectedResult);
        }
    }


    public class Delete1TestDataGenerator : IEnumerable<object[]>
    {
        static User user1 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN"
        };


        private readonly List<object[]> _data = new List<object[]>
        {
            //good
            new object[] {new Agency()
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
                    OwnerId = user1.Id,
                    Owner = user1,
                }, new AgencyDeleteDTO(1) { LastUpdatedDate = DateTime.Now, DeletedDate = DateTime.Now}, 1, true
            },
            //no agency with Id
            new object[] {(Agency?)null, new AgencyDeleteDTO(1) { LastUpdatedDate = DateTime.Now, DeletedDate = DateTime.Now}, 1, false},

            //deleted already
            new object[] {new Agency()
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
                    OwnerId = user1.Id,
                    Owner = user1,
                    DeletedDate = DateTime.Now.AddDays(-1)
                }, new AgencyDeleteDTO(1) { LastUpdatedDate = DateTime.Now, DeletedDate = DateTime.Now}, 1, false
            },

            //not the owner of agency
            new object[] {new Agency()
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
                    OwnerId = user1.Id,
                    Owner = user1,
                }, new AgencyDeleteDTO(1) { LastUpdatedDate = DateTime.Now, DeletedDate = DateTime.Now}, 2, false
            },
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
