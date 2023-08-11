using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.DTOs.Offers.Update;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.OfferTests
{
    public class UpdateOfferTests : BaseOfferServiceTests
    {
        [Fact]
        public void Update_ShouldReturnFalse_IfNoOfferWithGivenId()
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            OfferUpdateDTO dto = new OfferUpdateDTO() { };

            OfferRepositoryMock.Setup(x => x.GetOffer(1, It.IsAny<string>())).Returns((Offer?)null);

            bool result = testedOfferService.Update(dto, type, 1, user1.Id, out errorMessage);
            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldReturnFalse_IfUserIdIsntEqualToOfferSellerId()
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            HouseSaleOffer offer = new HouseSaleOffer() { Id = 1, SellerId = 2, OfferTitle = "test" };
            OfferUpdateDTO dto = new OfferUpdateDTO() { };

            OfferRepositoryMock.Setup(x => x.GetOffer(offer.Id, It.IsAny<string>())).Returns(offer);

            bool result = testedOfferService.Update(dto, type, offer.Id, user1.Id, out errorMessage);
            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldReturnTrue_IfEverythingOK()
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            HouseSaleOffer offer = new HouseSaleOffer() { Id = 1, SellerId = 1, OfferTitle = "test" };
            OfferUpdateDTO dto = new OfferUpdateDTO() { OfferTitle = "NewTitle" };

            OfferRepositoryMock.Setup(x => x.GetOffer(offer.Id, It.IsAny<string>())).Returns(offer);

            bool result = testedOfferService.Update(dto, type, offer.Id, user1.Id, out errorMessage);
            Assert.True(result);
        }
    }
}
