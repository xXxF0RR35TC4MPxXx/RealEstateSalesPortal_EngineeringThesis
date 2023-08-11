using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Inżynierka.UnitTests.AgencyTests
{
    public class CreateAgencyTests : BaseAgencyServiceTest
    {
        [Theory]
        [ClassData(typeof(Create1TestDataGenerator))]
        public void CreateAgency_ShouldReturnFalse_IfCreatorIsAlreadyAnAgent(User user, bool equal)
        {
            AgencyCreateDTO createDTO = new AgencyCreateDTO()
            {
                AgencyName = "TestName",
                Email = "test@mail.com",
                PhoneNumber = "123123123",
                City = "Test",
                Address = "TestAddress",
                PostalCode = "99-999",
                Description = "Desc",
                NIP = "9999999999",
                REGON = "132123123",
                LicenceNumber = "num",
                CreatedDate = DateTime.Now
            };

            Agency agency2 = new Agency()
            {
                Id = 1,
                OwnerId = user.Id,
                Owner = user,
                AgencyName = createDTO.AgencyName,
                Email = createDTO.Email,
                PhoneNumber = createDTO.PhoneNumber,
                City = createDTO.City,
                Address = createDTO.Address,
                PostalCode = createDTO.PostalCode,
                Description = createDTO.Description,
                NIP = createDTO.NIP,
                REGON = createDTO.REGON,
                LicenceNumber = createDTO.LicenceNumber,
                CreatedDate = DateTime.Now,
            };


            UserRepositoryMock.Setup(x => x.GetById(user.Id)).Returns(user);
            AgencyRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<Agency>())).Returns(agency2);
            int? result = testedAgencyService.CreateAgency(createDTO, user.Id, out errorMessage);

            if (equal)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
            }
            
        }
    }

    public class Create1TestDataGenerator : IEnumerable<object[]>
    {
        static User user1 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN"
        };

        static User user2 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            OwnerOfAgencyId = 1
        };

        static User user3 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            AgentInAgencyId = 1
        };

        private readonly List<object[]> _data = new List<object[]>
        {

            new object[] {user1, false},
            new object[] {user2, true},
            new object[] {user3, true},
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
