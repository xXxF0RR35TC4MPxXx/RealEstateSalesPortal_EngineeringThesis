using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.Entities;
using Inżynierka_Services.Services;
using iText.StyledXmlParser.Node;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.AgencyTests
{
    public class AddAgentTests : BaseAgencyServiceTest
    {

        [Theory]
        [ClassData(typeof(AddAgent1TestDataGenerator))]
        public void AddAgent_ShouldReturnFalse_IfUserDoesntExist(int agentId, User owner, User? returnedAgent, bool expectedResult)
        {
            Agency agency = new Agency()
            {
                Id = 1,
                OwnerId = 2,
                Owner = owner,
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
                CreatedDate = DateTime.Now,
            };
            owner.OwnerOfAgency = agency;

            UserRepositoryMock.Setup(x => x.GetById(agentId)).Returns(returnedAgent);
            UserRepositoryMock.Setup(x => x.GetById(owner.Id)).Returns(owner);
            AgencyRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<Agency>())).Returns(agency);
            AgencyRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(agency);
            
            bool result = testedAgencyService.AddAgent(agentId, owner.Id, out errorMessage);

            if(expectedResult == true)
            {
                Assert.True(result);
            }
            else
            {
                Assert.False(result);
            }
        }
    }

    public class AddAgent1TestDataGenerator : IEnumerable<object[]>
    {
        static List<Offer> offers = new List<Offer>() { new Offer() , new Offer() };

        static User user1 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN"
        };
        static User user2 = new()
        {
            Id = 2,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            OwnerOfAgencyId = 1
        };
        static User user3 = new()
        {
            Id = 3,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            AgentInAgencyId = 1
        };
        static User user4 = new()
        {
            Id = 4,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            OwnerOfAgencyId = 2
        };
        static User user5 = new()
        {
            Id = 5,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            UserOffers = offers
        };

        private readonly List<object[]> _data = new List<object[]>
        {

            new object[] {1, user2, user1, true},  //owner dodaje wolnego agenta
            new object[] {3, user2, user3, false}, //owner nie może dodać wolnego agenta bo już jest w innej
            new object[] {1, user3, user1, false}, //nie owner dodaje wolnego agenta
            new object[] {2, user2, null, false},  //owner dodaje usera który nie istnieje
            new object[] {4, user2, user4, false}, //owner dodaje usera który jest ownerem innej agencji
            new object[] {5, user2, user5, false}, //owner dodaje usera który jest wolny ale ma już oferty
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
