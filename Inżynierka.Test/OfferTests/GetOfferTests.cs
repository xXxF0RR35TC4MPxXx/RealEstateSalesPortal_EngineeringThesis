using Inżynierka.Shared.DTOs.Offers.Read;
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
    public class GetOfferTests:BaseOfferServiceTests
    {
        [Fact]
        public void GetOffer_ShouldReturnNull_WhenNoOfferWithGivenId()
        {
            int offerId = 999;

            OfferRepositoryMock.Setup(x => x.GetTypeOfOffer(offerId)).Returns((string?)null);
            OfferRepositoryMock.Setup(x => x.GetOffer(offerId, null)).Returns((Offer?)null);
            ReadOfferDTO? result = testedOfferService.Get(offerId, "", out errorMessage);

            Assert.Null(result);
        }

        [Fact]
        public void GetOffer_ShouldReturnNull_WhenOfferDoesntHaveAStrongType()
        {
            byte[]? bytes = new byte[] { 0x1 };

            OfferRepositoryMock.Setup(x => x.GetTypeOfOffer(offer1.Id)).Returns((string?)null);
            OfferRepositoryMock.Setup(x => x.GetOffer(offer1.Id, null)).Returns(offer1);
            UserRepositoryMock.Setup(x => x.GetById(user1.Id)).Returns(user1);
            OfferRepositoryMock.Setup(x => x.GetRoomCount(offer1.Id, It.IsAny<string>())).Returns(3);
            OfferRepositoryMock.Setup(x => x.GetRoomFloor(offer1.Id)).Returns("1");
            OfferRepositoryMock.Setup(x => x.GetPhoto(It.IsAny<string>())).Returns(Task.FromResult(bytes));

            ReadOfferDTO? result = testedOfferService.Get(offer1.Id, "", out errorMessage);

            Assert.Null(result);
        }

        [Fact]
        public void GetOffer_ShouldReturnHouseSaleOffer_WhenSellerPrivateAndOK()
        {
            string type= EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            byte[]? bytes = new byte[] { 0x1 };
            
            OfferRepositoryMock.Setup(x => x.GetTypeOfOffer(houseSaleOffer.Id)).Returns(type);
            OfferRepositoryMock.Setup(x => x.GetOffer(houseSaleOffer.Id, type)).Returns(houseSaleOffer);
            UserRepositoryMock.Setup(x => x.GetById(user1.Id)).Returns(user1);
            OfferRepositoryMock.Setup(x => x.GetRoomCount(houseSaleOffer.Id, type)).Returns(3);
            OfferRepositoryMock.Setup(x => x.GetRoomFloor(houseSaleOffer.Id)).Returns("1");
            OfferRepositoryMock.Setup(x => x.GetPhoto(It.IsAny<string>())).Returns(Task.FromResult(bytes));

            ReadOfferDTO? result = testedOfferService.Get(houseSaleOffer.Id, "", out errorMessage);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetOffer_ShouldReturnHouseSaleOffer_WhenSellerIsAnAgentAndOK()
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            byte[]? bytes = new byte[] { 0x1 };

            OfferRepositoryMock.Setup(x => x.GetTypeOfOffer(houseSaleOffer2.Id)).Returns(type);
            OfferRepositoryMock.Setup(x => x.GetOffer(houseSaleOffer2.Id, type)).Returns(houseSaleOffer2);
            UserRepositoryMock.Setup(x => x.GetById(user2.Id)).Returns(user2);
            OfferRepositoryMock.Setup(x => x.GetRoomCount(houseSaleOffer2.Id, type)).Returns(3);
            OfferRepositoryMock.Setup(x => x.GetRoomFloor(houseSaleOffer2.Id)).Returns("1");
            OfferRepositoryMock.Setup(x => x.GetPhoto(It.IsAny<string>())).Returns(Task.FromResult(bytes));

            ReadOfferDTO? result = testedOfferService.Get(houseSaleOffer2.Id, "", out errorMessage);

            Assert.NotNull(result);
        }

    }
}
