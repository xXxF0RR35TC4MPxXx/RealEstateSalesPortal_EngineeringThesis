using Inżynierka.Shared.DTOs.User;
using Inżynierka.UnitTests.AgencyTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserEventTests
{
    public class DeleteEventTests : BaseUserEventServiceTests
    {
        [Fact]
        public void DeleteEvent_ShouldReturnTrue_WhenEverythingIsOK()
        {
            UserEventDeleteDTO dto = new(1);

            UserEventRepositoryMock.Setup(x => x.GetById(1)).Returns(userEvent1);

            bool result = testedUserEventService.DeleteEvent(dto, 1, out errorMessage);

            Assert.True(result);
        }

        [Fact]
        public void DeleteEvent_ShouldReturnFalse_WhenEventIsDeleted()
        {
            UserEventDeleteDTO dto = new(2);

            UserEventRepositoryMock.Setup(x => x.GetById(2)).Returns(deletedUserEvent);

            bool result = testedUserEventService.DeleteEvent(dto, 1, out errorMessage);

            Assert.False(result);
        }

        [Fact]
        public void DeleteEvent_ShouldReturnFalse_WhenEventIsntMine()
        {
            UserEventDeleteDTO dto = new(3);

            UserEventRepositoryMock.Setup(x => x.GetById(3)).Returns(notMyUserEvent);

            bool result = testedUserEventService.DeleteEvent(dto, 1, out errorMessage);

            Assert.False(result);
        }

    }
}
