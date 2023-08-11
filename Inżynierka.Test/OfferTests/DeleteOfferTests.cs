using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.Entities;
using Inżynierka.UnitTests.AgencyTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.OfferTests
{
    public class DeleteOfferTests : BaseOfferServiceTests
    {
        [Fact]
        public void Delete_ShouldReturnFalse_IfNoOfferWithGivenId()
        {
            DeleteOfferDTO dto = new DeleteOfferDTO(1);

            OfferRepositoryMock.Setup(x => x.GetById(dto.Id)).Returns((Offer?)null);

            bool result = testedOfferService.DeleteOffer(user1.Id, dto, out errorMessage);
            Assert.False(result);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_IfUserIdIsntEqualToOfferSellerId()
        {
            Offer offer = new Offer() { Id = 1, SellerId = 2, OfferTitle = "test" };
            DeleteOfferDTO dto = new DeleteOfferDTO(1);

            OfferRepositoryMock.Setup(x => x.GetById(dto.Id)).Returns((Offer?)null);

            bool result = testedOfferService.DeleteOffer(user1.Id, dto, out errorMessage);
            Assert.False(result);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_IfEverythingWorks()
        {
            Offer offer = new Offer() { Id = 1, SellerId = 1, OfferTitle = "test" };
            DeleteOfferDTO dto = new DeleteOfferDTO(1);

            OfferRepositoryMock.Setup(x => x.GetById(dto.Id)).Returns(offer);

            bool result = testedOfferService.DeleteOffer(user1.Id, dto, out errorMessage);
            Assert.True(result);
        }
    }
}
