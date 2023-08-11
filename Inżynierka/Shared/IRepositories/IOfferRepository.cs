using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.Entities.OfferTypes.Apartment;
using Inżynierka.Shared.Entities.OfferTypes.BusinessPremises;
using Inżynierka.Shared.Entities.OfferTypes.Garage;
using Inżynierka.Shared.Entities.OfferTypes.Hall;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.Entities.OfferTypes.Plot;
using Inżynierka.Shared.Entities.OfferTypes.Room;

namespace Inżynierka.Shared.IRepositories
{
    [ScopedRegistrationWithInterface]
    public interface IOfferRepository : IBaseRepository<Offer>
    {

        public int GetUserOfferCount(int id);
        public string? GetTypeOfOffer(int paramId);
        public List<Offer>? GetHomepageOffers();
        public string? GetRoomFloor(int offerId);
        public List<Offer>? GetSimilarOffers(int id);
        public int GetUsersLastOfferId(int userId, string type);
        public Offer? GetOffer(int id, string? type);
        public IQueryable<Offer>? GetAllOfType(OfferFilteringDTO offerFilteringDTO);
        public Task<List<byte[]>?> GetPhotos(string imgPath);
        public Task<byte[]?> GetPhoto(string imgPath);
        int? GetRoomCount(int offerId, string type);

        public (IEnumerable<RoomRentingOffer>? roomOffers, IEnumerable<PlotOffer>? plotOffers, IEnumerable<HouseRentOffer>? houseRentOffers, IEnumerable<HouseSaleOffer>? houseSaleOffers
            , IEnumerable<HallRentOffer>? hallRentOffers, IEnumerable<HallSaleOffer>? hallSaleOffers, IEnumerable<BusinessPremisesRentOffer>? businessPremisesRentOffers, 
            IEnumerable<BusinessPremisesSaleOffer>? businessPremisesSaleOffers,
            IEnumerable<GarageRentOffer>? garageRentOffers, IEnumerable<GarageSaleOffer>? garageSaleOffers, IEnumerable<ApartmentRentOffer>? apartmentRentOffers,
            IEnumerable<ApartmentSaleOffer>? apartmentSaleOffers) GetAllUsersOffers(int? id, int? agencyId);
    }   
}
