using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Entities.OfferTypes.House;
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
    public class OfferRepositoryTests : BaseIntegrationTest
    {
        //GetTypeOfOffer() - to może być problematyczne 

        public static object?[] GetAllOffersOfTypeDTOParamsTestCases =
        {
             new object?[] { new HouseSaleFilteringDTO(), true },
             new object?[] {new HouseRentFilteringDTO(), true },
             new object?[] {new ApartmentRentFilteringDTO(), true },
             new object?[] {new RoomRentFilteringDTO(), true },
             new object?[] {new PlotOfferFilteringDTO(), true },
             new object?[] {new HallRentFilteringDTO(), true },
             new object?[] {new HallSaleFilteringDTO(), true },
             new object?[] {new PremisesSaleFilteringDTO(), true },
             new object?[] {new PremisesRentFilteringDTO(), true },
             new object?[] {new GarageSaleFilteringDTO(), true },
             new object?[] {new GarageRentFilteringDTO(), true },
             new object?[] {new OfferFilteringDTO(), false },
        };

        public static object?[] GetUsersLastOfferTestCases =
        {
             new object?[] { 7, "Pokoje", true },
             new object?[] { 7, "Działki", true },
             new object?[] { 7, "DomyWynajem", true },
             new object?[] { 7, "DomySprzedaż", true },
             new object?[] { 7, "MieszkaniaWynajem", true },
             new object?[] { 7, "MieszkaniaSprzedaż", true },
             new object?[] { 7, "Lokale użytkoweWynajem", true },
             new object?[] { 7, "Lokale użytkoweSprzedaż", true },
             new object?[] { 7, "Hale i magazynyWynajem", true },
             new object?[] { 7, "Hale i magazynySprzedaż", true },
             new object?[] { 7, "GarażeWynajem", true },
             new object?[] { 7, "GarażeSprzedaż", true },
             new object?[] { -1, "Pokoje", false },
             new object?[] { -1, "Działki", false },
             new object?[] { -1, "DomyWynajem", false },
             new object?[] { -1, "DomySprzedaż", false },
             new object?[] { -1, "MieszkaniaWynajem", false },
             new object?[] { -1, "MieszkaniaSprzedaż", false },
             new object?[] { -1, "Lokale użytkoweWynajem", false },
             new object?[] { -1, "Lokale użytkoweSprzedaż", false },
             new object?[] { -1, "Hale i magazynyWynajem", false },
             new object?[] { -1, "Hale i magazynySprzedaż", false },
             new object?[] { -1, "GarażeWynajem", false },
             new object?[] { -1, "GarażeSprzedaż", false },
        };

        public static object?[] GetRoomCountTestCases =
        {
             new object?[] { 31, "Pokoje", false },
             new object?[] { 19, "Działki", false },
             new object?[] { 29, "DomyWynajem", true },
             new object?[] { 18, "DomySprzedaż", true },
             new object?[] { 23, "MieszkaniaWynajem", true },
             new object?[] { 21, "MieszkaniaSprzedaż", true },
             new object?[] { 33, "Lokale użytkoweWynajem", false },
             new object?[] { 32, "Lokale użytkoweSprzedaż", false },
             new object?[] { 27, "Hale i magazynyWynajem", false },
             new object?[] { 28, "Hale i magazynySprzedaż", false },
             new object?[] { 25, "GarażeWynajem", false },
             new object?[] { 26, "GarażeSprzedaż", false }
        };

        public static object?[] GetOfferTestCases =
        {
             new object?[] { 31, "Pokoje"},
             new object?[] { 19, "Działki"},
             new object?[] { 29, "DomyWynajem"},
             new object?[] { 18, "DomySprzedaż"},
             new object?[] { 23, "MieszkaniaWynajem"},
             new object?[] { 21, "MieszkaniaSprzedaż"},
             new object?[] { 33, "Lokale użytkoweWynajem"},
             new object?[] { 32, "Lokale użytkoweSprzedaż"},
             new object?[] { 27, "Hale i magazynyWynajem"},
             new object?[] { 28, "Hale i magazynySprzedaż"},
             new object?[] { 25, "GarażeWynajem"},
             new object?[] { 26, "GarażeSprzedaż" }
        };
               
        [Theory, Isolated]
        [TestCaseSource(nameof(GetOfferTestCases))]
        public void GetOffer_IfValidParams_ShouldReturnOffer(int offerId, string type)
        {
            Offer? offer = offerRepo.GetOffer(offerId, type);
            Assert.That(offer, Is.Not.Null);
        }

        [Theory, Isolated]
        [TestCaseSource(nameof(GetRoomCountTestCases))]
        public void GetRoomCount_IfValidParams_ShouldReturnRoomCount(int offerId, string type, bool expectedResult)
        {
            int? roomCount = offerRepo.GetRoomCount(offerId, type);
            if (!expectedResult)
            {
                Assert.That(roomCount, Is.EqualTo(0));
            }
            else
            {
                
                Assert.That(roomCount, Is.Not.EqualTo(0));
            }
        }

        [Test, Isolated]
        [TestCase(-1, false)]
        [TestCase(31, true)]
        [TestCase(35, true)]
        public void GetRoomFloor_IfValidId_ShouldReturnFloor(int offerId, bool expectedResult)
        {
            if (expectedResult)
            {
                var floor = offerRepo.GetRoomFloor(offerId);
                Assert.That(floor, Is.Not.Null);
            }
            else
            {
                Assert.Catch(() => offerRepo.GetRoomFloor(offerId));
            }
        }

        [Theory, Isolated]
        [TestCaseSource(nameof(GetAllOffersOfTypeDTOParamsTestCases))]
        public void GetAllOfType_IfAnyOffersOfType_ShouldReturnThem(OfferFilteringDTO dto, bool expectedResult)
        {
            var result = offerRepo.GetAllOfType(dto);

            if (expectedResult)
            {
                Assert.That(result, Is.Not.Null);
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }

        [Theory, Isolated]
        [TestCaseSource(nameof(GetUsersLastOfferTestCases))]
        public void GetUsersLastOfferId_IfAnyOffersOfType_ShouldReturnThem(int userId, string type, bool expectedResult)
        {
            if (!expectedResult)
            {
                Assert.Catch(()=> offerRepo.GetUsersLastOfferId(userId, type));
            }
            else
            {
                int? lastId = offerRepo.GetUsersLastOfferId(userId, type);
                Assert.That(lastId, Is.Not.EqualTo(0));
            }
         }

        [Test, Isolated]
        public void GetHomepageOffers_IfAnyOffers_ShouldReturn8OfThem()
        {
            List<Offer>? offers = offerRepo.GetHomepageOffers();
            var offerCount = offers?.Count;

            if (context.HouseRentOffers.Count() != 0 || context.HouseSaleOffers.Count() != 0 || context.ApartmentRentOffers.Count() != 0 || context.ApartmentSaleOffers.Count() != 0)
            {
                Assert.That(offerCount, Is.Not.EqualTo(0));
            }
            else
            {
                Assert.That(offerCount, Is.EqualTo(0));
            }
        }


        //base methods
        [Test, Isolated]
        public void Add_IfValidHouseSaleOffer_ShouldAddHouseSaleOfferToDatabase()
        {
            var offer = (HouseSaleOffer)offerRepo.AddAndSaveChanges(houseSaleOffer);

            var offerCount = context.HouseSaleOffers.Count(x => x.Id == offer.Id);
            Assert.That(offerCount, Is.EqualTo(1));
        }

        [Test, Isolated]
        public void Add_IfInvalidHouseSaleOffer_ShouldThrowDbUpdateException()
        {
            var ex = Assert.Throws<DbUpdateException>(() => offerRepo2.AddAndSaveChanges(invalidHouseSaleOffer));
            Assert.That(ex, Is.Not.Null);
        }


        [Test, Isolated]
        public void Update_IfValidHouseSaleOffer_ShouldUpdateHouseSaleOfferInDatabase()
        {
            string newTitle = "UpdatedTitle";
            HouseSaleOffer houseSaleOffer = (HouseSaleOffer)offerRepo.GetById(30);
            string oldTitle = houseSaleOffer.OfferTitle;
            houseSaleOffer.OfferTitle = newTitle;

            offerRepo.UpdateAndSaveChanges(houseSaleOffer);
            var offerCount = context.HouseSaleOffers.Count(x => x.OfferTitle == newTitle);
            var offerCount2 = context.HouseSaleOffers.Count(x => x.OfferTitle == oldTitle);

            Assert.That(offerCount, Is.EqualTo(1));
            Assert.That(offerCount2, Is.EqualTo(0));
        }

        [Test, Isolated]
        public void RemoveById_IfValidId_ShouldRemoveUserEventFromDatabase()
        {
            offerRepo.RemoveByIdAndSaveChanges(1);
            var offerCount = context.HouseSaleOffers.Count(x => x.Id == 1);

            Assert.That(offerCount, Is.EqualTo(0));
        }


        [Test, Isolated]
        public void GetById_IfValidId_ShouldReturnHouseSaleOffer()
        {
            var offer = offerRepo2.GetById(1);
            Assert.That(offer, Is.Not.Null);
        }
           
        [Theory, Isolated]
        [TestCase(1, "DomySprzedaż", true)]
        [TestCase(18, "DomySprzedaż", true)]
        [TestCase(30, "DomySprzedaż", true)]
        [TestCase(Int32.MaxValue, "DomySprzedaż", false)]
        [TestCase(Int32.MinValue, "DomySprzedaż", false)]
        [TestCase(0, "DomySprzedaż", false)]
        [TestCase(-1, "DomySprzedaż", false)]
        [TestCase(29, "DomySprzedaż", false)]
        [TestCase(42, "invalidxd", false)]
        public void GetOffer_IfValidIdAndType_ShouldReturnOffer(int id, string type, bool expectedResult)
        {
            var offer = offerRepo2.GetOffer(id, type);
            if (expectedResult)
            {
                Assert.That(offer, Is.Not.Null);
            }
            else
            {
                Assert.That(offer, Is.Null);
            }
        }        
    }
}
