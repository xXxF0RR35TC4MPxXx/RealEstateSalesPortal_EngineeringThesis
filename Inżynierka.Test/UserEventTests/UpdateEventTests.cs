using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.UnitTests.AgencyTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserEventTests
{
    public class UpdateEventTests : BaseUserEventServiceTests
    {
        //jeżeli nie ma takiego eventu
        [Fact]
        public void Update_ShouldReturnFalse_IfEventDoesntExist()
        {
            UserEventRepositoryMock.Setup(x => x.GetById(1)).Returns((UserEvent?)null);

            bool result = testedUserEventService.Update(userEventUpdateDTO, 1, out errorMessage);

            Assert.False(result);
        }

        //działa dobrze edytując wszystkie dane
        [Fact]
        public void Update_ShouldReturnTrue_IfOKUpdateAll()
        {
            UserEventRepositoryMock.Setup(x => x.GetById(1)).Returns(userEvent1);

            bool result = testedUserEventService.Update(userEventUpdateDTO, 1, out errorMessage);

            Assert.True(result);
        }

        //działa dobrze na częściowo pustych danych
        [Fact]
        public void Update_ShouldReturnTrue_IfOKUpdateSome()
        {
            UserEventRepositoryMock.Setup(x => x.GetById(1)).Returns(userEvent1);

            bool result = testedUserEventService.Update(userEventUpdateDTO2, 1, out errorMessage);

            Assert.True(result);
        }
    }
}
