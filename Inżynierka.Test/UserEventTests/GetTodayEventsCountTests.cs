using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserEventTests
{
    public class GetTodayEventsCountTests : BaseUserEventServiceTests
    {
        //empty list
        [Fact]
        public void GetTodayEventsCount_ShouldReturn0_WhenNoEventsFoundAtAll()
        {
            UserEventDeleteDTO dto = new(1);

            UserEventRepositoryMock.Setup(x => x.GetAllUserEvents(1)).Returns((IEnumerable<UserEvent>?)null);

            int? result = testedUserEventService.GetTodayEventsCount(1, out errorMessage);

            Assert.Equal(0, result);
        }

        //events today found
        [Fact]
        public void GetTodayEventsCount_ShouldReturnInt_WhenTodaysEventsFound()
        {
            UserEventDeleteDTO dto = new(1);

            UserEventRepositoryMock.Setup(x => x.GetAllUserEvents(1)).Returns(userEvents);

            int? result = testedUserEventService.GetTodayEventsCount(1, out errorMessage);

            Assert.Equal(0, result);
        }

        //not empty list but no events today
        [Fact]
        public void GetTodayEventsCount_ShouldReturn0_WhenNoEventsToday()
        {
            UserEventDeleteDTO dto = new(1);

            UserEventRepositoryMock.Setup(x => x.GetAllUserEvents(1)).Returns(userEventsNotToday);

            int? result = testedUserEventService.GetTodayEventsCount(1, out errorMessage);

            Assert.Equal(0, result);
        }
    }
}
