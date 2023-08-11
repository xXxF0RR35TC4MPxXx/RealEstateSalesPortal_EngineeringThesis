using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.Entities.OfferTypes.Room;
using Inżynierka.Shared.Entities.OfferTypes.Plot;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.Entities.OfferTypes.Hall;
using Inżynierka.Shared.Entities.OfferTypes.BusinessPremises;
using Inżynierka.Shared.Entities.OfferTypes.Garage;
using Inżynierka.Shared.Entities.OfferTypes.Apartment;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class OfferRepository : BaseRepository<Offer>, IOfferRepository
    {
        public IConfiguration Configuration { get; }
        private DataContext _dataContext;

        public OfferRepository(DataContext context, IConfiguration configuration) : base(context)
        {

            _dataContext = context;
            Configuration = configuration;
            
        }

        public int GetUserOfferCount(int id)
        {
            var offers = GetAllUsersOffers(id, null);
            int count = 0;
            if (offers.roomOffers != null) count += offers.roomOffers.Count();
            if (offers.plotOffers != null) count += offers.plotOffers.Count();
            if (offers.houseRentOffers != null) count += offers.houseRentOffers.Count();
            if (offers.houseSaleOffers != null) count += offers.houseSaleOffers.Count();
            if (offers.hallRentOffers != null) count += offers.hallRentOffers.Count();
            if (offers.hallSaleOffers != null) count += offers.hallSaleOffers.Count();
            if (offers.apartmentRentOffers != null) count += offers.apartmentRentOffers.Count();
            if (offers.apartmentSaleOffers != null) count += offers.apartmentSaleOffers.Count();
            if (offers.businessPremisesRentOffers != null) count += offers.businessPremisesRentOffers.Count();
            if (offers.businessPremisesSaleOffers != null) count += offers.businessPremisesSaleOffers.Count();
            if (offers.garageRentOffers != null) count += offers.garageRentOffers.Count();
            if (offers.garageSaleOffers != null) count += offers.garageSaleOffers.Count();

            return count;
        }


        public List<Offer>? GetSimilarOffers(int id)
        {
            string? type = GetTypeOfOffer(id);
            if (type == null) return null;

            Offer? offer = GetOffer(id, type);
            if (offer != null)
            {
                List<Offer> result = _dataContext.RoomRentingOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>();
                result.AddRange(_dataContext.HouseRentOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.HouseSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.ApartmentRentOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.ApartmentSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.GarageSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.GarageSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.HallSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.HallSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.BusinessPremisesSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.BusinessPremisesSaleOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());
                result.AddRange(_dataContext.PlotOffers.Where(o => o.DeletedDate == null && o.SellerId == offer.SellerId && o.Id != id).OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>());

                return result.OrderByDescending(o => o.AddedDate).Take(4).ToList<Offer>();
            }
            else return null;
        }

        public List<Offer>? GetHomepageOffers()
        {

            List<Offer> result = _dataContext.RoomRentingOffers.Where(o=> o.DeletedDate == null).OrderByDescending(o => o.AddedDate).Take(8).ToList<Offer>();

            result.AddRange(_dataContext.HouseRentOffers.Where(o => o.DeletedDate == null).OrderByDescending(o => o.AddedDate).Take(8).ToList<Offer>());
            result.AddRange(_dataContext.HouseSaleOffers.Where(o => o.DeletedDate == null).OrderByDescending(o => o.AddedDate).Take(8).ToList<Offer>());
            result.AddRange(_dataContext.ApartmentRentOffers.Where(o => o.DeletedDate == null).OrderByDescending(o => o.AddedDate).Take(8).ToList<Offer>());
            result.AddRange(_dataContext.ApartmentSaleOffers.Where(o => o.DeletedDate == null).OrderByDescending(o => o.AddedDate).Take(8).ToList<Offer>());

            var endResult = result.OrderByDescending(o => o.AddedDate).Take(8).ToList<Offer>();

            return endResult;
        }

        public IQueryable<Offer>? GetAllOfType(OfferFilteringDTO offerFilteringDTO)
        {
            IQueryable<Offer>? result;

            if (offerFilteringDTO is RoomRentFilteringDTO)
            {
                result = _dataContext.RoomRentingOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is PlotOfferFilteringDTO)
            {
                result = _dataContext.PlotOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is ApartmentRentFilteringDTO)
            {
                result = _dataContext.ApartmentRentOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is ApartmentSaleFilteringDTO)
            {
                result = _dataContext.ApartmentSaleOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is HallRentFilteringDTO)
            {
                result = _dataContext.HallRentOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is HallSaleFilteringDTO)
            {
                result = _dataContext.HallSaleOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is HouseRentFilteringDTO)
            {
                result = _dataContext.HouseRentOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is HouseSaleFilteringDTO)
            {
                result = _dataContext.HouseSaleOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is PremisesRentFilteringDTO)
            {
                result = _dataContext.BusinessPremisesRentOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is PremisesSaleFilteringDTO)
            {
                result = _dataContext.BusinessPremisesSaleOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is GarageRentFilteringDTO)
            {
                result = _dataContext.GarageRentOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else if (offerFilteringDTO is GarageSaleFilteringDTO)
            {
                result = _dataContext.GarageSaleOffers.Where(o => !o.DeletedDate.HasValue);
            }
            else result = null;

            return result;
        }

        public (IEnumerable<RoomRentingOffer>? roomOffers, IEnumerable<PlotOffer>? plotOffers, IEnumerable<HouseRentOffer>? houseRentOffers, IEnumerable<HouseSaleOffer>? houseSaleOffers
            , IEnumerable<HallRentOffer>? hallRentOffers, IEnumerable<HallSaleOffer>? hallSaleOffers, IEnumerable<BusinessPremisesRentOffer>? businessPremisesRentOffers,
            IEnumerable<BusinessPremisesSaleOffer>? businessPremisesSaleOffers,
            IEnumerable<GarageRentOffer>? garageRentOffers, IEnumerable<GarageSaleOffer>? garageSaleOffers, IEnumerable<ApartmentRentOffer>? apartmentRentOffers,
            IEnumerable<ApartmentSaleOffer>? apartmentSaleOffers) GetAllUsersOffers(int? id, int? agencyId)
        {
            if (id != null)
            {
                IEnumerable<RoomRentingOffer>? roomResult = _dataContext.RoomRentingOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<PlotOffer>? plotResult = _dataContext.PlotOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HouseRentOffer>? houseRentResult = _dataContext.HouseRentOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HouseSaleOffer>? houseSaleResult = _dataContext.HouseSaleOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HallRentOffer>? hallRentResult = _dataContext.HallRentOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HallSaleOffer>? hallSaleResult = _dataContext.HallSaleOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<BusinessPremisesRentOffer>? businessRentResult = _dataContext.BusinessPremisesRentOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<BusinessPremisesSaleOffer>? businessSaleResult = _dataContext.BusinessPremisesSaleOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<GarageRentOffer>? garageRentResult = _dataContext.GarageRentOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<GarageSaleOffer>? garageSaleResult = _dataContext.GarageSaleOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<ApartmentRentOffer>? apartmentRentResult = _dataContext.ApartmentRentOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<ApartmentSaleOffer>? apartmentSaleResult = _dataContext.ApartmentSaleOffers.Where(i => i.SellerId == id && i.DeletedDate == null).Include(i => i.Seller);


                return (roomResult, plotResult, houseRentResult, houseSaleResult, hallRentResult, hallSaleResult,
                    businessRentResult, businessSaleResult, garageRentResult, garageSaleResult, apartmentRentResult, apartmentSaleResult);
            }
            else if (agencyId != null)
            {
                IEnumerable<RoomRentingOffer>? roomResult = _dataContext.RoomRentingOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<PlotOffer>? plotResult = _dataContext.PlotOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HouseRentOffer>? houseRentResult = _dataContext.HouseRentOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HouseSaleOffer>? houseSaleResult = _dataContext.HouseSaleOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HallRentOffer>? hallRentResult = _dataContext.HallRentOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<HallSaleOffer>? hallSaleResult = _dataContext.HallSaleOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<BusinessPremisesRentOffer>? businessRentResult = _dataContext.BusinessPremisesRentOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<BusinessPremisesSaleOffer>? businessSaleResult = _dataContext.BusinessPremisesSaleOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<GarageRentOffer>? garageRentResult = _dataContext.GarageRentOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<GarageSaleOffer>? garageSaleResult = _dataContext.GarageSaleOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<ApartmentRentOffer>? apartmentRentResult = _dataContext.ApartmentRentOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);
                IEnumerable<ApartmentSaleOffer>? apartmentSaleResult = _dataContext.ApartmentSaleOffers.Where(i => (i.Seller.AgentInAgencyId == agencyId || i.Seller.OwnerOfAgencyId == agencyId) && i.DeletedDate == null).Include(i => i.Seller);


                return (roomResult, plotResult, houseRentResult, houseSaleResult, hallRentResult, hallSaleResult,
                    businessRentResult, businessSaleResult, garageRentResult, garageSaleResult, apartmentRentResult, apartmentSaleResult);
            }
            else return (null, null, null, null, null, null, null, null, null, null, null, null);
        }

        public int GetUsersLastOfferId(int UserId, string type)
        {
            int result;
            if (type == EnumHelper.GetDescriptionFromEnum(EstateType.ROOM))
            {
                result = _dataContext.RoomRentingOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PLOT))
            {
                result = _dataContext.PlotOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE)+ EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.HouseRentOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            { 
                result = _dataContext.HouseSaleOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.HallRentOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.HallSaleOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.BusinessPremisesRentOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.BusinessPremisesSaleOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.GarageRentOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.GarageSaleOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.ApartmentRentOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.ApartmentSaleOffers.Where(o => o.SellerId == UserId).OrderBy(o => o.Id).Last().Id;
            }
            else result = 0;           

            return result;
            
        }

        public string? GetRoomFloor(int offerId)
        {
            return _dataContext.RoomRentingOffers.Where(o => o.Id == offerId).OrderBy(o => o.Id).Last().Floor;
        }
        public int? GetRoomCount(int OfferId, string type)
        {
            int? result;
            if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.HouseRentOffers.Where(o => o.Id == OfferId).OrderBy(o => o.Id).Last().RoomCount;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.HouseSaleOffers.Where(o => o.Id == OfferId).OrderBy(o => o.Id).Last().RoomCount;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.ApartmentRentOffers.Where(o => o.Id == OfferId).OrderBy(o=>o.Id).Last().RoomCount;
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.ApartmentSaleOffers.Where(o => o.Id == OfferId).OrderBy(o => o.Id).Last().RoomCount;
            }
            else result = 0;

            return result;
        }

        //returns type of offer in format: "ESTATEOFFERTYPE", for example. "HOUSESALE", "APARTMENTRENT"
        public string? GetTypeOfOffer(int paramId)
        {
            string? result = null;
            string queryString = "SELECT OfferType, EstateType FROM [dbo].[Offer] WHERE Id = @paramId AND DeletedDate IS NULL";
            using (SqlConnection sqlConnection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.Parameters.AddWithValue("@paramId", paramId);
                sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        result = string.Format("{0}{1}", reader[1], reader[0]);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }

            return result;
        }

        public Offer? GetOffer(int id, string? type)
        {
            Offer? result;

            if (type.Contains(EnumHelper.GetDescriptionFromEnum(EstateType.ROOM)))
            {
                result = _dataContext.RoomRentingOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type.Contains(EnumHelper.GetDescriptionFromEnum(EstateType.PLOT)))
            {
                result = _dataContext.PlotOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.HouseRentOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.HouseSaleOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.HallRentOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.HallSaleOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.BusinessPremisesRentOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.BusinessPremisesSaleOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.GarageRentOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.GarageSaleOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                result = _dataContext.ApartmentRentOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                result = _dataContext.ApartmentSaleOffers.Where(i => i.Id == id && i.DeletedDate == null)
                .Include(i => i.Seller)
                .FirstOrDefault();
            }
            else result = null;

            return result;
        }

        public async Task<List<byte[]>?> GetPhotos(string folderPath)
        {
            List<byte[]>? result = new();
            try
            {
                var files = Directory.GetFiles(folderPath);
                if (files.Count() > 0)
                {
                    foreach (string file in files)
                    {
                        var fileRead = await File.ReadAllBytesAsync(file);
                        if (fileRead != null)
                        {
                            result.Add(fileRead);
                        }
                    }
                    return result;
                }
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<byte[]?> GetPhoto(string imgPath)
        {
            List<byte[]>? result = new();
            string filePath;
            try
            {
                filePath = Directory.GetFiles(imgPath).First();
            }
            catch(Exception ex)
            {
                return null;
            }

            var fileRead = await File.ReadAllBytesAsync(filePath);
            if (fileRead != null)
            {
                result.Add(fileRead);
            }

            return result.First();
        }
    }

}
