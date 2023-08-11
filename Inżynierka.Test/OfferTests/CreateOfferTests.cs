using Inżynierka.Shared.DTOs.Offers.Create;
using Inżynierka.Shared.DTOs.Offers.Update;
using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.OfferTests
{
    public class CreateOfferTests : BaseOfferServiceTests
    {
        [Fact]
        public void Create_ShouldReturnFalse_IfSavingChangesThrowsException()
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            HouseSaleOfferCreateDTO dto = new HouseSaleOfferCreateDTO() { OfferTitle = "Created" };
            Exception ex = new();

            OfferRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<Offer?>())).Throws(ex);

            bool result = testedOfferService.Create(dto, user1.Id, out errorMessage);
            Assert.False(result);
        }

        [Fact]
        public void Create_ShouldReturnTrue_IfEverythingOK()
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            HouseSaleOfferCreateDTO dto = new HouseSaleOfferCreateDTO() { OfferTitle = "Created" };

            bool result = testedOfferService.Create(dto, user1.Id, out errorMessage);
            Assert.True(result);
        }
    }
}
