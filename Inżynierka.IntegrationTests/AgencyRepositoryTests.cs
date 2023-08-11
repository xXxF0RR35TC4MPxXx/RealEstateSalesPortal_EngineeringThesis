using Inżynierka.Shared;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.IntegrationTests
{
    public class AgencyRepositoryTests : BaseIntegrationTest
    {
        [Test, Isolated]
        public void Add_IfValidAgency_ShouldAddAgencyToDatabase()
        {
            User owner = userRepo.GetById(7);
            Agency agency = new Agency()
            {
                OwnerId = owner.Id,
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

            agencyRepo.AddAndSaveChanges(agency);

            var agenciesCount = context.Agencies.Count(x => x.Id == agency.Id && x.AgencyName == agency.AgencyName);
            Assert.That(agenciesCount, Is.EqualTo(1));
        }

        [Test, Isolated]
        public void Update_IfValidAgency_ShouldUpdateAgencyDatabase()
        {
            string newAgencyName = "UpdatedAgencyName";
            Agency agency1 = agencyRepo.GetById(1);
            string oldAgencyName = agency1.AgencyName;
            agency1.AgencyName = newAgencyName;
            agencyRepo.UpdateAndSaveChanges(agency1);
            var agenciesCount = context.Agencies.Count(x => x.Id == agency1.Id && x.AgencyName == newAgencyName);
            var agenciesCount2 = context.Agencies.Count(x => x.Id == agency1.Id && x.AgencyName == oldAgencyName);

            Assert.That(agenciesCount, Is.EqualTo(1));
            Assert.That(agenciesCount2, Is.EqualTo(0));
        }

        [Test, Isolated]
        public void RemoveById_IfValidIdAndNoAgents_ShouldRemoveAgencyFromDatabase()
        {
            User owner = userRepo.GetById(7);
            Agency agency = new Agency()
            {
                OwnerId = owner.Id,
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

            agencyRepo.AddAndSaveChanges(agency);
            Agency agency1 = agencyRepo.GetById(agency.Id);
            
            agencyRepo.RemoveByIdAndSaveChanges(agency1.Id);
            var agenciesCount = context.Agencies.Count(x => x.Id == agency1.Id);

            Assert.That(agenciesCount, Is.EqualTo(0));
        }

        [Test, Isolated]
        public void RemoveById_IfValidIdButHasAgents_ShouldThrowDbUpdateException()
        {
            var ex = Assert.Throws<DbUpdateException>(() => agencyRepo.RemoveByIdAndSaveChanges(1));
            Assert.That(ex, Is.Not.Null);
        }

        [Test, Isolated]
        public void GetById_IfValidId_ShouldReturnAgency()
        {
            var agency = agencyRepo.GetById(1);
            Assert.That(agency, Is.Not.Null);
        }

        [Theory, Isolated]
        [TestCase(-50, false)]
        [TestCase(999999, false)]
        [TestCase(1, true)]
        [TestCase(Int32.MaxValue, false)]
        [TestCase(Int32.MinValue, false)]
        [TestCase(0, false)]
        public void GetById_IfInvalidId_ShouldThrowException(int id, bool expectedResult)
        {
            var agency = agencyRepo.GetById(id);
            if (expectedResult)
            {
                Assert.That(agency, Is.Not.Null);
            }
            else
            {
                Assert.That(agency, Is.Null);
            }
            
        }

        [Test, Isolated]
        public void GetAll_IfAnyAgencies_ShouldReturnIQueryable()
        {
            var iqueryable = agencyRepo.GetAll();
            Assert.That(iqueryable.Count(), Is.Not.EqualTo(0));
        }
    }
}
