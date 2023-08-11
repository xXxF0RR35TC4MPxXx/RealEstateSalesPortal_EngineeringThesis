using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.UnitTests.AgencyTests;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserEventTests
{
    public class CreateEventTests : BaseUserEventServiceTests
    {
        //expected behaviour
        //[Fact]
        //public void CreateEvent_ShouldReturnTrue_IfEventCreatedSuccessfully()
        //{
        //    GetTypeDTO typeDTO = new("house");
        //    _offerService.Setup(x => x.GetTypeOfOffer(It.IsAny<int>())).Returns<GetTypeDTO>(typeDTO);
            
        //    bool result = testedUserEventService.Create(user1.Id, userEventCreateDTO, out errorMessage);

        //    Assert.True(result);
        //}

        //offer not related to current user
        [Fact]
        public void CreateEvent_ShouldReturnFalse_IfEventNotRelatedToUser()
        {
            bool result = testedUserEventService.Create(userEventCreateDTO, out errorMessage);

            Assert.False(result);
        }
    }
}
