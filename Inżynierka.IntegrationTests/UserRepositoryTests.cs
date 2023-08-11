using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Repositories;
using Inżynierka_Common.Enums;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.IntegrationTests
{
    public class UserRepositoryTests : BaseIntegrationTest
    {
        [Theory, Isolated]
        [TestCase(7, "PRIVATE")]
        [TestCase(8, "AGENCY")]
        [TestCase(999, null)]
        [TestCase(0, null)]
        [TestCase(-1, null)]
        public void GetUserRoleById_IfValidId_ShouldReturnRole(int id, string expectedRole)
        {
            var role = userRepo.GetUserRoleById(id);
            Assert.That(role, Is.EqualTo(expectedRole));
        }

        [Theory, Isolated]
        [TestCase("xd@gmail.com", 2)]
        [TestCase("no@user.com", null)]
        [TestCase("", null)]
        public void GetUserByEmail_IfValidId_ShouldReturnUser(string email, int? expectedUserId)
        {
            var user = userRepo.GetUserByEmail(email);
            if (expectedUserId == null)
            {
                Assert.That(user, Is.Null);
            }
            else
            {
                Assert.That(user.Id, Is.EqualTo(expectedUserId));
            }
        }

        [Test, Isolated]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            var users = userRepo.GetAllUsers();

            Assert.That(users.Count(), Is.Not.EqualTo(0));
        }

        [Theory, Isolated]
        [TestCase(2, true)]
        [TestCase(7, true)]
        [TestCase(8, true)]
        [TestCase(-5, false)]
        [TestCase(Int32.MaxValue, false)]
        [TestCase(Int32.MinValue, false)]
        public void GetUserEmail_IfValidId_ShouldReturnAllUserEmail(int id, bool expectedValidEmail)
        {
            var userEmail = userRepo.GetUserEmail(id);
            if (expectedValidEmail)
            {
                Assert.That(userEmail, Is.Not.Null);
            }
            else
            {
                Assert.That(userEmail, Is.Null);
            }

        }

        [Theory, Isolated]
        [TestCase("xd@gmail.com", true)]
        [TestCase("7", false)]
        [TestCase("", false)]
        [TestCase("⭕", false)]
        public void CheckIfUserExist_IfValidEmail_ShouldReturnTrue(string email, bool expectedResult)
        {
            var exists = userRepo.CheckIfUserExist(email);
            Assert.That(exists, Is.EqualTo(expectedResult));

        }


        [Theory, Isolated]
        [TestCase(1, true)]
        [TestCase(-1, false)]
        public void GetAllAgents_IfValidAgencyId_ShouldReturnAllAgents(int agencyId, bool expectedResult)
        {
            var agents = userRepo.GetAllAgents(agencyId);

            if (expectedResult)
            {
                Assert.That(agents.Count(), Is.Not.EqualTo(0));
            }
            else
            {
                Assert.That(agents.Count(), Is.EqualTo(0));
            }
        }

        //base methods

        [Test, Isolated]
        public void Add_IfValidUser_ShouldAddUserToDatabase()
        {
            User user22 = new User()
            {
                Email = "test2@mail.com",
                Password = "pass2",
                LastName = "pleasework2",
                RoleName = "PRIVATE2",
                PhoneNumber = "123123123",
                FirstName = "TestName2",
                CreatedDate = DateTime.Now,
                UserStatus = "ACTIVE",
                PasswordRecoveryGuid = new(),
                ConfirmationGuid = new()
            };

            user22 = userRepo2.AddAndSaveChanges(user22);

            var usersCount = context2.Users.Count(x => x.Id == user22.Id);
            Assert.That(usersCount, Is.EqualTo(1));
        }

        [Test, Isolated]
        public void Add_IfInvalidUser_ShouldThrowDbUpdateException()
        {
            User user = new User()
            {
                Email = "test@mail.com",
                Password = "pass",
                RoleName = "PRIVATE",
                PhoneNumber = "123123123",
                FirstName = "TestName1",
                //LastName = "TestName2",
                CreatedDate = DateTime.Now,
                UserStatus = "ACTIVE",
                PasswordRecoveryGuid = new(),
                ConfirmationGuid = new()
            };

            var ex = Assert.Throws<DbUpdateException>(() => userRepo.AddAndSaveChanges(user));
            Assert.That(ex, Is.Not.Null);
        }

        [Test, Isolated]
        public void Update_IfValidUser_ShouldUpdateUserInDatabase()
        {
            string newName = "UpdatedName";
            User user = userRepo2.GetById(7);
            string oldLastName = user.LastName;
            user.LastName = newName;
            userRepo2.UpdateAndSaveChanges(user);
            var usersCount = context2.Users.Count(x => x.Id == user.Id && x.LastName == newName);
            var usersCount2 = context2.Users.Count(x => x.Id == user.Id && x.LastName == oldLastName);

            Assert.That(usersCount, Is.EqualTo(1));
            Assert.That(usersCount2, Is.EqualTo(0));
        }

        [Test, Isolated]
        public void RemoveById_IfValidIdAndNoOffers_ShouldRemoveUserFromDatabase()
        {
            User user = new User()
            {
                Email = "test@mail.com",
                Password = "pass",
                RoleName = "PRIVATE",
                PhoneNumber = "123123123",
                FirstName = "TestName1",
                LastName = "TestName2",
                CreatedDate = DateTime.Now,
                UserStatus = "ACTIVE",
                PasswordRecoveryGuid = new(),
                ConfirmationGuid = new()
            };

            user = userRepo2.AddAndSaveChanges(user);
            
            userRepo2.RemoveByIdAndSaveChanges(user.Id);
            var userCount = context2.Users.Count(x => x.Id == user.Id);

            Assert.That(userCount, Is.EqualTo(0));
        }

        [Test, Isolated]
        public void RemoveById_IfValidIdButHasOffers_ShouldThrowDbUpdateException()
        {
            var ex = Assert.Throws<DbUpdateException>(() => userRepo.RemoveByIdAndSaveChanges(7));
            Assert.That(ex, Is.Not.Null);
        }

        [Test, Isolated]
        public void GetById_IfValidId_ShouldReturnUser()
        {
            var user = userRepo.GetById(7);
            Assert.That(user, Is.Not.Null);
        }

        [Theory, Isolated]
        [TestCase(-50, false)]
        [TestCase(999999, false)]
        [TestCase(7, true)]
        [TestCase(Int32.MaxValue, false)]
        [TestCase(Int32.MinValue, false)]
        [TestCase(0, false)]
        public void GetById_IfInvalidId_ShouldThrowException(int id, bool expectedResult)
        {
            var user = userRepo.GetById(id);
            if (expectedResult)
            {
                Assert.That(user, Is.Not.Null);
            }
            else
            {
                Assert.That(user, Is.Null);
            }

        }

        [Test, Isolated]
        public void GetAll_IfAnyUsers_ShouldReturnIQueryable()
        {
            var iqueryable = userRepo.GetAll();
            Assert.That(iqueryable.Count(), Is.Not.EqualTo(0));
        }
    }
}
