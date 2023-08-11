using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.IntegrationTests
{
    public class UserEventRepositoryTests:BaseIntegrationTest
    {
        [Test, Isolated]
        public void Add_IfInvalidUserEvent_ShouldThrowDbUpdateException()
        {
            UserEvent UserEvent = new UserEvent()
            {
                OfferId = 22,
                SellerId = 7,
                ClientName = "TestName",
                //ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName1",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now.AddDays(100)
            };

            var ex = Assert.Throws<DbUpdateException>(() => userEventRepo.AddAndSaveChanges(UserEvent));
            Assert.That(ex, Is.Not.Null);
        }

        [Test, Isolated]
        public void GetAllUserEvents_IfValidUserId_ShouldReturnIEnumerableOfUserEvents()
        {
            var userEvents = userEventRepo.GetAllUserEvents(7);
            Assert.That(userEvents.Count, Is.Not.EqualTo(0));
        }

        [Test, Isolated]
        public void GetAllUserEvents_IfInvalidUserId_ShouldReturnNullOrEmpty()
        {
            var userEvents = userEventRepo.GetAllUserEvents(-1);
            Assert.That(userEvents.Count, Is.EqualTo(0));
        }

        //zrobić bazowe
        [Test, Isolated]
        public void Add_IfValidUserEvent_ShouldAddUserEventToDatabase()
        {
            UserEvent UserEvent = new UserEvent() 
            {
                OfferId = 22,
                SellerId = 7,
                ClientName = "TestName",
                ClientEmail = "test@mail.com",
                ClientPhoneNumber = "123123123",
                EventName = "TestEventName1",
                EventCompletionStatus = EnumHelper.GetDescriptionFromEnum(EventCompletionStatus.NEW),
                DeadlineDate = DateTime.Now.AddDays(100)
            };

            UserEvent = userEventRepo2.AddAndSaveChanges(UserEvent);

            var UserEventCount = context2.UserEvents.Count(x => x.Id == UserEvent.Id);
            Assert.That(UserEventCount, Is.EqualTo(1));
        }

        

        [Test, Isolated]
        public void Update_IfValidUserEvent_ShouldUpdateUserEventInDatabase()
        {
            string newName = "UpdatedEventName";
            UserEvent userEvent = userEventRepo2.GetById(3);
            string oldName = userEvent.EventName;
            userEvent.EventName = newName;
            userEventRepo2.UpdateAndSaveChanges(userEvent);
            var eventsCount = context2.UserEvents.Count(x => x.EventName == newName);
            var eventsCount2 = context2.UserEvents.Count(x => x.EventName == oldName);

            Assert.That(eventsCount, Is.EqualTo(1));
            Assert.That(eventsCount2, Is.EqualTo(0));
        }

        [Test, Isolated]
        public void RemoveById_IfValidId_ShouldRemoveUserEventFromDatabase()
        {
            userEventRepo2.RemoveByIdAndSaveChanges(1);
            var userCount = context2.UserEvents.Count(x => x.Id == 1);

            Assert.That(userCount, Is.EqualTo(0));
        }

        //[Test, Isolated]
        //public void RemoveById_IfInvalidId_ShouldThrowDbUpdateException()
        //{
        //    var ex = Assert.Throws<DbUpdateException>(() => userEventRepo2.RemoveByIdAndSaveChanges(-1));
        //    Assert.That(ex, Is.Not.Null);
        //}

        [Test, Isolated]
        public void GetById_IfValidId_ShouldReturnUserEvent()
        {
            var user = userEventRepo.GetById(1);
            Assert.That(user, Is.Not.Null);
        }

        [Theory, Isolated]
        [TestCase(-50, false)]
        [TestCase(999999, false)]
        [TestCase(3, true)]
        [TestCase(Int32.MaxValue, false)]
        [TestCase(Int32.MinValue, false)]
        [TestCase(0, false)]
        public void GetById_IfInvalidId_ShouldThrowException(int id, bool expectedResult)
        {
            var user = userEventRepo2.GetById(id);
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
        public void GetAll_IfAnyUserEvents_ShouldReturnIQueryable()
        {
            var iqueryable = userEventRepo2.GetAll();
            Assert.That(iqueryable.Count(), Is.Not.EqualTo(0));
        }
    }
}
