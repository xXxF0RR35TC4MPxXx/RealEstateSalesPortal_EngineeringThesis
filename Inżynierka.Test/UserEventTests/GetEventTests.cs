using Inżynierka.Shared.DTOs.User;
using Inżynierka.UnitTests.AgencyTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserEventTests
{
    public class GetEventTests : BaseUserEventServiceTests
    {
        [Fact]
        public void Get_ShouldReturnDTO_IfEventExistsAndIsntDeletedAndBelongsToUser()
        {
            UserEventRepositoryMock.Setup(x => x.GetById(1)).Returns(userEvent1);

            ReadEventDTO? result = testedUserEventService.Get(user1.Id, 1, out errorMessage);

            Assert.NotNull(result);
        }

        [Fact]
        public void Get_ShouldReturnNull_IfEventIsDeleted()
        {
            UserEventRepositoryMock.Setup(x => x.GetById(2)).Returns(deletedUserEvent);

            ReadEventDTO? result = testedUserEventService.Get(user1.Id, 2, out errorMessage);

            Assert.Null(result);
        }

        [Fact]
        public void Get_ShouldReturnNull_IfUserIsntOwnerOfEvent()
        {
            UserEventRepositoryMock.Setup(x => x.GetById(3)).Returns(notMyUserEvent);

            ReadEventDTO? result = testedUserEventService.Get(user1.Id, 3, out errorMessage);

            Assert.Null(result);
        }
    }
}
