using Inżynierka.Shared.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.AgencyTests
{
    public class LeaveAgencyTests : BaseAgencyServiceTest
    {

        [Theory]
        [ClassData(typeof(LeaveAgency1TestDataGenerator))]
        public void LeaveAgency_ShouldReturnFalse_IfUserDoesntExist(int agentId, User owner, User? returnedAgent, bool expectedResult)
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
            if (returnedAgent != null && returnedAgent.AgentInAgencyId != null) returnedAgent.AgentInAgency = agency;
            List<Offer> offers = new List<Offer>();

            UserRepositoryMock.Setup(x => x.GetById(agentId)).Returns(returnedAgent);
            UserRepositoryMock.Setup(x => x.GetById(owner.Id)).Returns(owner);
            AgencyRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<Agency>())).Returns(agency);
            AgencyRepositoryMock.Setup(x => x.GetById(agency.Id)).Returns(agency);
            OfferRepositoryMock.Setup(x => x.GetAll()).Returns(offers.AsQueryable());
            UserEventRepositoryMock.Setup(x => x.GetAllUserEvents(agentId)).Returns((IEnumerable<UserEvent>?)null);

            bool result = testedAgencyService.LeaveAgency(agentId, owner.Id, out errorMessage);

            if (expectedResult == true)
            {
                Assert.True(result);
            }
            else
            {
                Assert.False(result);
            }
        }
    }

    public class LeaveAgency1TestDataGenerator : IEnumerable<object[]>
    {
        static User user1 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
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
        static User user6 = new()
        {
            Id = 6,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN",
            AgentInAgencyId = 2
        };

        private readonly List<object[]> _data = new List<object[]>
        {

            new object[] {3, user2, user3, true},  //owner usuwa swojego agenta
            new object[] {1, user2, user1, false},  //owner usuwa kogoś kto agentem nie jest
            new object[] {6, user2, user6, false},  //owner usuwa czyjegoś agenta
            new object[] {2, user3, user2, false},  //nie owner usuwa agenta ze swojej agencji
            new object[] {6, user3, user6, false},  //nie owner usuwa nie swojej agencji
            new object[] {1, user2, null, false},  //nie ma takiego użytkownika
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
