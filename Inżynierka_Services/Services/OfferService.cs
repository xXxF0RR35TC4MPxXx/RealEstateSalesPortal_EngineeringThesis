using Inżynierka_Common.Helpers;
using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.Extensions.Logging;
using Inżynierka_Common.Enums;
using Inżynierka.Shared.DTOs.Offers.Create;
using Inżynierka.Shared.Entities.OfferTypes.Plot;
using Inżynierka.Shared.Entities.OfferTypes.Room;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.Entities.OfferTypes.Hall;
using Inżynierka.Shared.Entities.OfferTypes.BusinessPremises;
using Inżynierka.Shared.Entities.OfferTypes.Garage;
using Inżynierka.Shared.Entities.OfferTypes.Apartment;
using AutoMapper;
using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.DTOs.Offers.Update;
using Inżynierka_Common.Listing;
using Inżynierka_Services.Listing;
using PagedList;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Microsoft.IdentityModel.Tokens;
using iText.Kernel.Pdf;
using LiczbyNaSlowaNET;
using Path = System.IO.Path;
using iText.Forms.Fields;
using iText.Forms;
using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.Map;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class OfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserFavouriteRepository _userFavouriteRepository;
        private readonly ILogger<OfferService> _logger;
        private readonly IMapper _mapper;
        private readonly IAgencyRepository _agencyRepository;

        public OfferService(ILogger<OfferService> logger, IOfferRepository offerRepository, IAgencyRepository agencyRepository, IMapper mapper, 
            IUserFavouriteRepository userFavouriteRepository, IUserRepository userRepository)
        {
            _offerRepository = offerRepository;
            _logger = logger;
            _mapper = mapper;
            _userFavouriteRepository = userFavouriteRepository;
            _userRepository = userRepository;
            _agencyRepository = agencyRepository;
        }

        public IEnumerable<HomepageOffersDTO>? GetSimilar(int id, string path)
        {
            List<Offer>? offers = _offerRepository.GetSimilarOffers(id);

            IEnumerable<HomepageOffersDTO> homepageOffersDTOcollection = offers.Select(offer => new HomepageOffersDTO
            {
                Id = offer.Id,
                OfferTitle = offer.OfferTitle,
                OfferCategory = offer.EstateType,
                ForSaleOrForRent = offer.OfferType,
                Voivodeship = offer.Voivodeship,
                City = offer.City,
                Area = offer.Area,
                RoomCount = offer.RoomCount,
                Price = offer.Price,
                Photo = File.Exists(Path.Combine(path, offer.Id.ToString(), "1.jpg")) ? File.ReadAllBytes(Path.Combine(path, offer.Id.ToString(), "1.jpg")) : null
            }) ; ;


            return homepageOffersDTOcollection;
        }

        public Offer? GetOfferForUpdate(int offerId)
        {
            string? type = _offerRepository.GetTypeOfOffer(offerId);
            if(type == null || type == "")
            {
                return null;
            }

            Offer offer = _offerRepository.GetOffer(offerId, type);
            if (offer == null)
            {
                return null;
            }

            return offer;
        }

        public GetTypeDTO? GetTypeOfOffer(int offerId)
        {
            string? type= _offerRepository.GetTypeOfOffer(offerId);
            if (type != null)
            {
                GetTypeDTO dto = new(type);
                return dto;
            }
            else return null;
        }

        public async Task<Coordinates?> GetCoordinates(string address)
        {
            string key = "dwbfps8KP1QDPg9vK6OI~ZYZIcujm5aaUH-mgu5De2w~ApYEWr9kE5IaeLvDHz78uoDDvAPiFF0xF56Z2h8VUAOy6mS6EvgLWY9v7E2MyLRV";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://dev.virtualearth.net/REST/v1/Locations");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            //client.DefaultRequestHeaders.
            string uri = string.Format("?q={0}&maxResults=1&key={1}", address, key);
            var response = await client.GetAsync(uri);

            string resultContent = await response.Content.ReadAsStringAsync();

            Root root = JsonConvert.DeserializeObject<Root>(resultContent);
            double cord1 = root.resourceSets[0].resources[0].geocodePoints[0].coordinates[0];
            double cord2 = root.resourceSets[0].resources[0].geocodePoints[0].coordinates[1];

            return new Coordinates() { Longitude = cord1, Latitude = cord2 };
        }

        public ReadOfferDTO? Get(int offerId, string imgPath, out string _errorMessage)
        {
            int? RoomCount = 0;
            string agencyLogoImgPath = System.IO.Path.Combine(imgPath, "AgencyLogo");
            string offerImgPath = System.IO.Path.Combine(imgPath, "Offers", offerId.ToString());
            

            string? type = _offerRepository.GetTypeOfOffer(offerId);

            Offer? offer = _offerRepository.GetOffer(offerId, type);
            if (offer == null)
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return null;
            }
            string userImgPath = System.IO.Path.Combine(imgPath, "Avatars", offer.Seller.Id.ToString());

            IEnumerable<byte[]>? photos = _offerRepository.GetPhotos(offerImgPath).Result;

            int PriceForOneSquareMeter;
            if(offer is not null && offer.Area != 0 && offer.Area is not null)
            {
                PriceForOneSquareMeter = (int)(offer.Price / offer.Area);
            }
            else
            {
                PriceForOneSquareMeter = 0;
            }

            User? user = _userRepository.GetById(offer.SellerId);
            int? id_agencji;
            if (user.OwnerOfAgencyId != null)
            {
                id_agencji = user.OwnerOfAgencyId;
            }
            else if (user.AgentInAgencyId != null)
            {
                id_agencji = user.AgentInAgencyId;
            }
            else
            {
                id_agencji = null;
            }

            Agency? agency = (id_agencji != null) ? _agencyRepository.GetById(id_agencji.Value) : null;

            string estateType = offer.EstateType;
            if (estateType == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) || estateType == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT))
            {
                //pobierz ilość pokoi z oferty o podanym ID
                RoomCount = _offerRepository.GetRoomCount(offerId, type);
            }

            //get agency here
            // AgencyInOfferDTO agencyDTO = getAgencyOfUser();
            _errorMessage = "";
            if(offer is RoomRentingOffer)
            {
                RoomRentingReadOfferDTO readDTO = _mapper.Map<RoomRentingReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.Floor = _offerRepository.GetRoomFloor(offer.Id);
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency==null?(offer.Seller.OwnerOfAgency==null?null:offer.Seller.OwnerOfAgency.PhoneNumber):offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if(offer is PlotOffer)
            {
                PlotReadOfferDTO readDTO = _mapper.Map<PlotReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if(offer is HouseRentOffer)
            {
                HouseRentReadOfferDTO readDTO = _mapper.Map<HouseRentReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is HouseSaleOffer)
            {
                HouseSaleReadOfferDTO readDTO = _mapper.Map<HouseSaleReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is HallRentOffer)
            {
                HallRentReadOfferDTO readDTO = _mapper.Map<HallRentReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is HallSaleOffer)
            {
                HallSaleReadOfferDTO readDTO = _mapper.Map<HallSaleReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is GarageRentOffer)
            {
                GarageRentReadOfferDTO readDTO = _mapper.Map<GarageRentReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is GarageSaleOffer)
            {
                GarageSaleReadOfferDTO readDTO = _mapper.Map<GarageSaleReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is BusinessPremisesRentOffer)
            {
                BusinessPremisesRentReadOfferDTO readDTO = _mapper.Map<BusinessPremisesRentReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is BusinessPremisesSaleOffer)
            {
                BusinessPremisesSaleReadOfferDTO readDTO = _mapper.Map<BusinessPremisesSaleReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is ApartmentRentOffer)
            {
                ApartmentRentReadOfferDTO readDTO = _mapper.Map<ApartmentRentReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.RoomCount = RoomCount;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.EstateType = offer.EstateType;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else if (offer is ApartmentSaleOffer)
            {
                ApartmentSaleReadOfferDTO readDTO = _mapper.Map<ApartmentSaleReadOfferDTO>(offer);
                readDTO.Photos = photos;
                readDTO.PriceForOneSquareMeter = PriceForOneSquareMeter;
                //readDTO.ApartmentFinishCondition = offer.ApartmentFinishCondition;
                readDTO.OfferType = offer.OfferType;
                readDTO.SellerType = offer.SellerType;
                readDTO.SellerId = offer.Seller.Id;
                readDTO.SellerAvatar = _offerRepository.GetPhoto(userImgPath).Result;
                readDTO.SellerName = offer.Seller.FirstName + " " + offer.Seller.LastName;
                readDTO.SellerPhone = offer.Seller.PhoneNumber;
                readDTO.AgencyPhone = offer.Seller.AgentInAgency == null ? (offer.Seller.OwnerOfAgency == null ? null : offer.Seller.OwnerOfAgency.PhoneNumber) : offer.Seller.AgentInAgency.PhoneNumber;
                readDTO.AddingDate = offer.AddedDate;
                readDTO.AgencyId = (agency != null) ? agency.Id : null;
                readDTO.AgencyName = (agency != null) ? agency.AgencyName : null;
                readDTO.AgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                return readDTO;
            }
            else
            {
                _errorMessage = ErrorMessageHelper.UnrecognizedOfferType;
                return null;
            }
        }


        public OfferListing? GetOffers(Paging paging, SortOrder? sortOrder, OfferFilteringDTO offerFilteringDTO, string imgFolderPath, int? userId, int? agencyId)
        {
            string offerImgPath = System.IO.Path.Combine(imgFolderPath, "Offers");
            string avatarImgPath = System.IO.Path.Combine(imgFolderPath, "Avatars");
            string agencyLogoImgPath = System.IO.Path.Combine(imgFolderPath, "AgencyLogo");

            //agency doesn't exist or is deleted
            if(agencyId!=null)
            {
                Agency? agency = _agencyRepository.GetById(agencyId.Value);
                if (agency == null || agency.DeletedDate != null) { return null; }
            }
            

            if (offerFilteringDTO is RoomRentFilteringDTO)
            {
                IQueryable<RoomRentingOffer>? offers = (IQueryable<RoomRentingOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                RoomRentFilteringDTO offerFilteringDTOConverted = (RoomRentFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.RoommateCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.NumberOfPeopleInTheRoom == offerFilteringDTOConverted.RoommateCount);
                    }
                    if (offerFilteringDTOConverted.HasInternet.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Internet == offerFilteringDTOConverted.HasInternet);
                    }
                    if (offerFilteringDTOConverted.HasPhone.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.HomePhone == offerFilteringDTOConverted.HasPhone);
                    }
                    if (offerFilteringDTOConverted.HasCableTV.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.CableTV == offerFilteringDTOConverted.HasCableTV);
                    }

                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<RoomRentingOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<RoomRentingOffer>.Sort(offers, sortOrder.Sort);
                    }


                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x => 
                    { 
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            Voivodeship = x.Voivodeship,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        }; 
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is PlotOfferFilteringDTO)
            {

                IQueryable<PlotOffer>? offers = (IQueryable<PlotOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                PlotOfferFilteringDTO offerFilteringDTOConverted = (PlotOfferFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.NearForest.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Forest == offerFilteringDTOConverted.NearForest);
                    }
                    if (offerFilteringDTOConverted.NearLake.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Lake == offerFilteringDTOConverted.NearLake);
                    }
                    if (offerFilteringDTOConverted.NearMountains.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Mountains == offerFilteringDTOConverted.NearMountains);
                    }
                    if (offerFilteringDTOConverted.NearSea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Sea == offerFilteringDTOConverted.NearSea);
                    }
                    if (offerFilteringDTOConverted.NearOpenArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.OpenArea == offerFilteringDTOConverted.NearOpenArea);
                    }
                    if (offerFilteringDTOConverted.availableOfferTypes == AvailableOfferTypes.SALE)
                    {
                        offers = offers.Where(s => s.OfferType == EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    }
                    else if (offerFilteringDTOConverted.availableOfferTypes == AvailableOfferTypes.RENT)
                    {
                        offers = offers.Where(s => s.OfferType == EnumHelper.GetDescriptionFromEnum(OfferType.RENT));
                    }
                    if (offerFilteringDTOConverted.availablePlotTypes is not null && offerFilteringDTOConverted.availablePlotTypes.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var plotType in offerFilteringDTOConverted.availablePlotTypes)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(plotType));
                        }

                        offers = offers.Where(s => allOfferTypeStrings.Contains(s.PlotType));
                    }
                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<PlotOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else if (offers != null)
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<PlotOffer>.Sort(offers, sortOrder.Sort);
                    }


                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x => {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Voivodeship = x.Voivodeship,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }



            }
            else if (offerFilteringDTO is ApartmentRentFilteringDTO)
            {
                IQueryable<ApartmentRentOffer>? offers = (IQueryable<ApartmentRentOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                ApartmentRentFilteringDTO offerFilteringDTOConverted = (ApartmentRentFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount >= offerFilteringDTOConverted.minRoomCount);
                    }
                    if (offerFilteringDTOConverted.maxRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount <= offerFilteringDTOConverted.maxRoomCount);
                    }
                    if (offerFilteringDTOConverted.minFloorCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.FloorCount >= offerFilteringDTOConverted.minFloorCount);
                    }
                    if (offerFilteringDTOConverted.maxFloorCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.FloorCount <= offerFilteringDTOConverted.maxFloorCount);
                    }
                    if (offerFilteringDTOConverted.oldestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.YearOfConstruction >= offerFilteringDTOConverted.oldestBuildingYear);
                    }
                    if (offerFilteringDTOConverted.newestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.YearOfConstruction <= offerFilteringDTOConverted.newestBuildingYear);
                    }
                    if (offerFilteringDTOConverted.AvailableForStudents.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AvailableForStudents == offerFilteringDTOConverted.AvailableForStudents);
                    }
                    if (offerFilteringDTOConverted.HasInternet.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Internet == offerFilteringDTOConverted.HasInternet);
                    }
                    if (offerFilteringDTOConverted.HasCableTV.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.CableTV == offerFilteringDTOConverted.HasCableTV);
                    }
                    if (offerFilteringDTOConverted.HasHomePhone.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.HomePhone == offerFilteringDTOConverted.HasHomePhone);
                    }
                    if (offerFilteringDTOConverted.HasBalcony.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Balcony == offerFilteringDTOConverted.HasBalcony);
                    }
                    if (offerFilteringDTOConverted.HasUtilityRoom.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.UtilityRoom == offerFilteringDTOConverted.HasUtilityRoom);
                    }
                    if (offerFilteringDTOConverted.HasParkingSpace.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ParkingSpace == offerFilteringDTOConverted.HasParkingSpace);
                    }
                    if (offerFilteringDTOConverted.HasBasement.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Basement == offerFilteringDTOConverted.HasBasement);
                    }
                    if (offerFilteringDTOConverted.HasGarden.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Garden == offerFilteringDTOConverted.HasGarden);
                    }
                    if (offerFilteringDTOConverted.HasTerrace.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Terrace == offerFilteringDTOConverted.HasTerrace);
                    }
                    if (offerFilteringDTOConverted.HasElevator.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Elevator == offerFilteringDTOConverted.HasElevator);
                    }
                    if (offerFilteringDTOConverted.HasTwoLevel.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.TwoLevel == offerFilteringDTOConverted.HasTwoLevel);
                    }
                    if (offerFilteringDTOConverted.HasSeparateKitchen.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.SeparateKitchen == offerFilteringDTOConverted.HasSeparateKitchen);
                    }
                    if (offerFilteringDTOConverted.HasAirConditioning.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AirConditioning == offerFilteringDTOConverted.HasAirConditioning);
                    }

                    //if (offerFilteringDTOConverted.availableTypesOfBuilding is not null && offerFilteringDTOConverted.availableTypesOfBuilding.Count != 0)
                    //{
                    //    List<string> allOfferTypeStrings = new List<string>();
                    //    foreach (var buildingType in offerFilteringDTOConverted.availableTypesOfBuilding)
                    //    {
                    //        allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(buildingType));
                    //    }

                    //    offers = offers.Where(s => s.TypeOfBuilding != null && allOfferTypeStrings.Contains(s.TypeOfBuilding));
                    //}

                    //if (offerFilteringDTOConverted.availableFloors is not null && offerFilteringDTOConverted.availableFloors.Count != 0)
                    //{
                    //    List<string> allOfferTypeStrings = new List<string>();
                    //    foreach (var floorCounts in offerFilteringDTOConverted.availableFloors)
                    //    {
                    //        allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(floorCounts));
                    //    }

                    //    offers = offers.Where(s => s.Floor != null && allOfferTypeStrings.Contains(s.Floor));
                    //}

                    //if (offerFilteringDTOConverted.availableHeatingTypes is not null && offerFilteringDTOConverted.availableHeatingTypes.Count != 0)
                    //{
                    //    List<string> allOfferTypeStrings = new List<string>();
                    //    foreach (var heatingTypes in offerFilteringDTOConverted.availableHeatingTypes)
                    //    {
                    //        allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(heatingTypes));
                    //    }

                    //    offers = offers.Where(s => s.HeatingType != null && allOfferTypeStrings.Contains(s.HeatingType));
                    //}

                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<ApartmentRentOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<ApartmentRentOffer>.Sort(offers, sortOrder.Sort);
                    }


                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Voivodeship = x.Voivodeship,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }


            }
            else if (offerFilteringDTO is ApartmentSaleFilteringDTO)
            {
                IQueryable<ApartmentSaleOffer>? offers = (IQueryable<ApartmentSaleOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                ApartmentSaleFilteringDTO offerFilteringDTOConverted = (ApartmentSaleFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount >= offerFilteringDTOConverted.minRoomCount);
                    }
                    if (offerFilteringDTOConverted.maxRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount <= offerFilteringDTOConverted.maxRoomCount);
                    }
                    if (offerFilteringDTOConverted.minFloorCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.FloorCount >= offerFilteringDTOConverted.minFloorCount);
                    }
                    if (offerFilteringDTOConverted.maxFloorCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.FloorCount <= offerFilteringDTOConverted.maxFloorCount);
                    }
                    if (offerFilteringDTOConverted.oldestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.YearOfConstruction >= offerFilteringDTOConverted.oldestBuildingYear);
                    }
                    if (offerFilteringDTOConverted.newestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.YearOfConstruction <= offerFilteringDTOConverted.newestBuildingYear);
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }


                    if (offerFilteringDTOConverted.HasBalcony.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Balcony == offerFilteringDTOConverted.HasBalcony);
                    }
                    if (offerFilteringDTOConverted.HasUtilityRoom.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.UtilityRoom == offerFilteringDTOConverted.HasUtilityRoom);
                    }
                    if (offerFilteringDTOConverted.HasParkingSpace.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ParkingSpace == offerFilteringDTOConverted.HasParkingSpace);
                    }
                    if (offerFilteringDTOConverted.HasBasement.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Basement == offerFilteringDTOConverted.HasBasement);
                    }
                    if (offerFilteringDTOConverted.HasGarden.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Garden == offerFilteringDTOConverted.HasGarden);
                    }
                    if (offerFilteringDTOConverted.HasTerrace.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Terrace == offerFilteringDTOConverted.HasTerrace);
                    }
                    if (offerFilteringDTOConverted.HasElevator.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Elevator == offerFilteringDTOConverted.HasElevator);
                    }
                    if (offerFilteringDTOConverted.HasTwoLevel.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.TwoLevel == offerFilteringDTOConverted.HasTwoLevel);
                    }
                    if (offerFilteringDTOConverted.HasSeparateKitchen.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.SeparateKitchen == offerFilteringDTOConverted.HasSeparateKitchen);
                    }
                    if (offerFilteringDTOConverted.HasAirConditioning.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AirConditioning == offerFilteringDTOConverted.HasAirConditioning);
                    }

                    if (offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.PRIMARY_MARKET || offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.AFTERMARKET)
                    {
                        offers = offers.Where(s => s.TypeOfMarket == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.typesOfMarketInSearch));
                    }

                    if (offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.AGENCY || offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.PRIVATE)
                    {
                        offers = offers.Where(s => s.Seller.RoleName == offerFilteringDTOConverted.userRolesInSearch.ToString());
                    }

                    if (offerFilteringDTOConverted.availableTypesOfBuilding is not null && offerFilteringDTOConverted.availableTypesOfBuilding.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var buildingType in offerFilteringDTOConverted.availableTypesOfBuilding)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(buildingType));
                        }

                        offers = offers.Where(s => s.TypeOfBuilding != null && allOfferTypeStrings.Contains(s.TypeOfBuilding));
                    }
                    if (offerFilteringDTOConverted.availableBuildingMaterials is not null && offerFilteringDTOConverted.availableBuildingMaterials.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var buildingType in offerFilteringDTOConverted.availableBuildingMaterials)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(buildingType));
                        }

                        offers = offers.Where(s => s.BuildingMaterial != null && allOfferTypeStrings.Contains(s.BuildingMaterial));
                    }
                    if (offerFilteringDTOConverted.availableFloors is not null && offerFilteringDTOConverted.availableFloors.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var floorCounts in offerFilteringDTOConverted.availableFloors)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(floorCounts));
                        }

                        offers = offers.Where(s => s.Floor != null && allOfferTypeStrings.Contains(s.Floor));
                    }


                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<ApartmentSaleOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<ApartmentSaleOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            Voivodeship = x.Voivodeship,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }

            }
            else if (offerFilteringDTO is HallRentFilteringDTO)
            {
                IQueryable<HallRentOffer>? offers = (IQueryable<HallRentOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                HallRentFilteringDTO offerFilteringDTOConverted = (HallRentFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minHeight.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Height >= offerFilteringDTOConverted.minHeight);
                    }
                    if (offerFilteringDTOConverted.maxHeight.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Height <= offerFilteringDTOConverted.maxHeight);
                    }
                    if (offerFilteringDTOConverted.IsStorage.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Storage == offerFilteringDTOConverted.IsStorage);
                    }
                    if (offerFilteringDTOConverted.IsProduction.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Production == offerFilteringDTOConverted.IsProduction);
                    }
                    if (offerFilteringDTOConverted.IsOffice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Office == offerFilteringDTOConverted.IsOffice);
                    }
                    if (offerFilteringDTOConverted.IsCommercial.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Commercial == offerFilteringDTOConverted.IsCommercial);
                    }
                    if (offerFilteringDTOConverted.Heating.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Heating == offerFilteringDTOConverted.Heating);
                    }
                    if (offerFilteringDTOConverted.HallConstruction.HasValue)
                    {
                        offers = offers.Where(s => s.Construction == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.HallConstruction));
                    }


                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<HallRentOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<HallRentOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            Voivodeship = x.Voivodeship,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }


            }
            else if (offerFilteringDTO is HallSaleFilteringDTO)
            {
                IQueryable<HallSaleOffer>? offers = (IQueryable<HallSaleOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                HallSaleFilteringDTO offerFilteringDTOConverted = (HallSaleFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minHeight.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Height >= offerFilteringDTOConverted.minHeight);
                    }
                    if (offerFilteringDTOConverted.maxHeight.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Height <= offerFilteringDTOConverted.maxHeight);
                    }
                    if (offerFilteringDTOConverted.IsStorage.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Storage == offerFilteringDTOConverted.IsStorage);
                    }
                    if (offerFilteringDTOConverted.IsProduction.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Production == offerFilteringDTOConverted.IsProduction);
                    }
                    if (offerFilteringDTOConverted.IsOffice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Office == offerFilteringDTOConverted.IsOffice);
                    }
                    if (offerFilteringDTOConverted.IsCommercial.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Commercial == offerFilteringDTOConverted.IsCommercial);
                    }
                    if (offerFilteringDTOConverted.Heating.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Heating == offerFilteringDTOConverted.Heating);
                    }
                    if (offerFilteringDTOConverted.HallConstruction.HasValue)
                    {
                        offers = offers.Where(s => s.Construction == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.HallConstruction));
                    }
                    if (offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.PRIMARY_MARKET || offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.AFTERMARKET)
                    {
                        offers = offers.Where(s => s.TypeOfMarket == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.typesOfMarketInSearch));
                    }

                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }


                    //SALE


                    if (offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.AGENCY || offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.PRIVATE)
                    {
                        offers = offers.Where(s => s.Seller.RoleName == offerFilteringDTOConverted.userRolesInSearch.ToString());
                    }

                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<HallSaleOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<HallSaleOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    User? user = (userId != null) ? _userRepository.GetById(userId.Value) : null;
                    int? userAgencyId = null;
                    if (user != null)
                    {
                        if (user.OwnerOfAgencyId != null)
                        {
                            userAgencyId = user.OwnerOfAgencyId;
                        }
                        else if (user.AgentInAgencyId != null)
                        {
                            userAgencyId = user.AgentInAgencyId;
                        }
                    }
                    string? userAgencyName = (userAgencyId != null) ? _agencyRepository.GetById(userAgencyId.Value).AgencyName : null;
                    byte[]? userAgencyLogo = (userAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, userAgencyId.ToString())).Result : null;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Voivodeship = x.Voivodeship,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is HouseRentFilteringDTO)
            {
                IQueryable<HouseRentOffer>? offers = (IQueryable<HouseRentOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                HouseRentFilteringDTO offerFilteringDTOConverted = (HouseRentFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] splitString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string fragment in splitString)
                        {
                            offers = offers.Where(s => s.Description.Contains(fragment));
                        }
                    }

                    if (offerFilteringDTOConverted.availableTypesOfBuilding is not null && offerFilteringDTOConverted.availableTypesOfBuilding.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var buildingType in offerFilteringDTOConverted.availableTypesOfBuilding)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(buildingType));
                        }

                        offers = offers.Where(s => s.TypeOfBuilding != null && allOfferTypeStrings.Contains(s.TypeOfBuilding));
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount >= offerFilteringDTOConverted.minRoomCount);
                    }
                    if (offerFilteringDTOConverted.maxRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount <= offerFilteringDTOConverted.maxRoomCount);
                    }
                    if (offerFilteringDTOConverted.minPlotArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.LandArea >= offerFilteringDTOConverted.minPlotArea);
                    }
                    if (offerFilteringDTOConverted.maxPlotArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.LandArea <= offerFilteringDTOConverted.maxPlotArea);
                    }

                    if (offerFilteringDTOConverted.oldestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ConstructionYear >= offerFilteringDTOConverted.oldestBuildingYear);
                    }
                    if (offerFilteringDTOConverted.newestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ConstructionYear <= offerFilteringDTOConverted.newestBuildingYear);
                    }

                    if (offerFilteringDTOConverted.HasInternet.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Internet == offerFilteringDTOConverted.HasInternet);
                    }
                    if (offerFilteringDTOConverted.HasCableTV.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.CableTV == offerFilteringDTOConverted.HasCableTV);
                    }
                    if (offerFilteringDTOConverted.HasHomePhone.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.HomePhone == offerFilteringDTOConverted.HasHomePhone);
                    }
                    if (offerFilteringDTOConverted.HasWater.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Water == offerFilteringDTOConverted.HasWater);
                    }
                    if (offerFilteringDTOConverted.HasElectricity.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Electricity == offerFilteringDTOConverted.HasElectricity);
                    }
                    if (offerFilteringDTOConverted.HasSewageSystem.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.SewageSystem == offerFilteringDTOConverted.HasSewageSystem);
                    }
                    if (offerFilteringDTOConverted.HasGas.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Gas == offerFilteringDTOConverted.HasGas);
                    }
                    if (offerFilteringDTOConverted.HasSepticTank.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.SepticTank == offerFilteringDTOConverted.HasSepticTank);
                    }
                    if (offerFilteringDTOConverted.HasSewageTreatmentPlant.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.SewageTreatmentPlant == offerFilteringDTOConverted.HasSewageTreatmentPlant);
                    }

                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<HouseRentOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<HouseRentOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing(); 
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Voivodeship = x.Voivodeship,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is HouseSaleFilteringDTO)
            {
                IQueryable<HouseSaleOffer>? offers = (IQueryable<HouseSaleOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                HouseSaleFilteringDTO offerFilteringDTOConverted = (HouseSaleFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.PRIMARY_MARKET || offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.AFTERMARKET)
                    {
                        offers = offers.Where(s => s.TypeOfMarket == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.typesOfMarketInSearch));
                    }

                    if (offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.AGENCY || offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.PRIVATE)
                    {
                        offers = offers.Where(s => s.Seller.RoleName == offerFilteringDTOConverted.userRolesInSearch.ToString());
                    }


                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount >= offerFilteringDTOConverted.minRoomCount);
                    }
                    if (offerFilteringDTOConverted.maxRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount <= offerFilteringDTOConverted.maxRoomCount);
                    }
                    if (offerFilteringDTOConverted.minPlotArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.LandArea >= offerFilteringDTOConverted.minPlotArea);
                    }
                    if (offerFilteringDTOConverted.maxPlotArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.LandArea <= offerFilteringDTOConverted.maxPlotArea);
                    }

                    if (offerFilteringDTOConverted.oldestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ConstructionYear >= offerFilteringDTOConverted.oldestBuildingYear);
                    }
                    if (offerFilteringDTOConverted.newestBuildingYear.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ConstructionYear <= offerFilteringDTOConverted.newestBuildingYear);
                    }

                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }

                    if (offerFilteringDTOConverted.availableTypesOfBuilding is not null && offerFilteringDTOConverted.availableTypesOfBuilding.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var buildingType in offerFilteringDTOConverted.availableTypesOfBuilding)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(buildingType));
                        }

                        offers = offers.Where(s => s.TypeOfBuilding != null && allOfferTypeStrings.Contains(s.TypeOfBuilding));
                    }
                    if (offerFilteringDTOConverted.availableBuildingMaterials is not null && offerFilteringDTOConverted.availableBuildingMaterials.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var buildingType in offerFilteringDTOConverted.availableBuildingMaterials)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(buildingType));
                        }

                        offers = offers.Where(s => s.BuildingMaterial != null && allOfferTypeStrings.Contains(s.BuildingMaterial));
                    }
                    if (offerFilteringDTOConverted.availableRoofTypes is not null && offerFilteringDTOConverted.availableRoofTypes.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var floorCounts in offerFilteringDTOConverted.availableRoofTypes)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(floorCounts));
                        }

                        offers = offers.Where(s => s.RoofType != null && allOfferTypeStrings.Contains(s.RoofType));
                    }




                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<HouseSaleOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<HouseSaleOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    
                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x => 
                    { 
                    User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            Voivodeship = x.Voivodeship,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is PremisesRentFilteringDTO)
            {
                IQueryable<BusinessPremisesRentOffer>? offers = (IQueryable<BusinessPremisesRentOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                PremisesRentFilteringDTO offerFilteringDTOConverted = (PremisesRentFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount >= offerFilteringDTOConverted.minRoomCount);
                    }
                    if (offerFilteringDTOConverted.maxRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount <= offerFilteringDTOConverted.maxRoomCount);
                    }

                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }

                    if (offerFilteringDTOConverted.IsOffice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Office == offerFilteringDTOConverted.IsOffice);
                    }
                    if (offerFilteringDTOConverted.IsService.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Service == offerFilteringDTOConverted.IsService);
                    }
                    if (offerFilteringDTOConverted.IsGastronomic.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Gastronomic == offerFilteringDTOConverted.IsGastronomic);
                    }
                    if (offerFilteringDTOConverted.IsIndustrial.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Industrial == offerFilteringDTOConverted.IsIndustrial);
                    }
                    if (offerFilteringDTOConverted.IsCommercial.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Commercial == offerFilteringDTOConverted.IsCommercial);
                    }
                    if (offerFilteringDTOConverted.IsHotel.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Hotel == offerFilteringDTOConverted.IsHotel);
                    }
                    if (offerFilteringDTOConverted.HasShopwindow.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Shopwindow == offerFilteringDTOConverted.HasShopwindow);
                    }
                    if (offerFilteringDTOConverted.HasParkingSpace.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ParkingSpace == offerFilteringDTOConverted.HasParkingSpace);
                    }
                    if (offerFilteringDTOConverted.HasAsphaltDriveway.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AsphaltDriveway == offerFilteringDTOConverted.HasAsphaltDriveway);
                    }
                    if (offerFilteringDTOConverted.HasHeating.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Heating == offerFilteringDTOConverted.HasHeating);
                    }
                    if (offerFilteringDTOConverted.HasElevator.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Elevator == offerFilteringDTOConverted.HasElevator);
                    }
                    if (offerFilteringDTOConverted.HasFurnishings.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Furnishings == offerFilteringDTOConverted.HasFurnishings);
                    }
                    if (offerFilteringDTOConverted.HasAirConditioning.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AirConditioning == offerFilteringDTOConverted.HasAirConditioning);
                    }
                    if (offerFilteringDTOConverted.acceptableFloors is not null && offerFilteringDTOConverted.acceptableFloors.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var floorCounts in offerFilteringDTOConverted.acceptableFloors)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(floorCounts));
                        }

                        offers = offers.Where(s => s.Floor != null && allOfferTypeStrings.Contains(s.Floor));
                    }


                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<BusinessPremisesRentOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<BusinessPremisesRentOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Voivodeship = x.Voivodeship,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is PremisesSaleFilteringDTO)
            {
                IQueryable<BusinessPremisesSaleOffer>? offers = (IQueryable<BusinessPremisesSaleOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                PremisesSaleFilteringDTO offerFilteringDTOConverted = (PremisesSaleFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.IsOffice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Office == offerFilteringDTOConverted.IsOffice);
                    }
                    if (offerFilteringDTOConverted.IsService.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Service == offerFilteringDTOConverted.IsService);
                    }
                    if (offerFilteringDTOConverted.IsGastronomic.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Gastronomic == offerFilteringDTOConverted.IsGastronomic);
                    }
                    if (offerFilteringDTOConverted.IsIndustrial.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Industrial == offerFilteringDTOConverted.IsIndustrial);
                    }
                    if (offerFilteringDTOConverted.IsCommercial.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Commercial == offerFilteringDTOConverted.IsCommercial);
                    }
                    if (offerFilteringDTOConverted.IsHotel.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Hotel == offerFilteringDTOConverted.IsHotel);
                    }
                    if (offerFilteringDTOConverted.HasShopwindow.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Shopwindow == offerFilteringDTOConverted.HasShopwindow);
                    }
                    if (offerFilteringDTOConverted.HasParkingSpace.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.ParkingSpace == offerFilteringDTOConverted.HasParkingSpace);
                    }
                    if (offerFilteringDTOConverted.HasAsphaltDriveway.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AsphaltDriveway == offerFilteringDTOConverted.HasAsphaltDriveway);
                    }
                    if (offerFilteringDTOConverted.HasHeating.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Heating == offerFilteringDTOConverted.HasHeating);
                    }
                    if (offerFilteringDTOConverted.HasElevator.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Elevator == offerFilteringDTOConverted.HasElevator);
                    }
                    if (offerFilteringDTOConverted.HasFurnishings.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Furnishings == offerFilteringDTOConverted.HasFurnishings);
                    }
                    if (offerFilteringDTOConverted.HasAirConditioning.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.AirConditioning == offerFilteringDTOConverted.HasAirConditioning);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.minRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount >= offerFilteringDTOConverted.minRoomCount);
                    }
                    if (offerFilteringDTOConverted.maxRoomCount.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RoomCount <= offerFilteringDTOConverted.maxRoomCount);
                    }

                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }

                    if (offerFilteringDTOConverted.acceptableFloors is not null && offerFilteringDTOConverted.acceptableFloors.Count != 0)
                    {
                        List<string> allOfferTypeStrings = new List<string>();
                        foreach (var floorCounts in offerFilteringDTOConverted.acceptableFloors)
                        {
                            allOfferTypeStrings.Add(EnumHelper.GetDescriptionFromEnum(floorCounts));
                        }

                        offers = offers.Where(s => s.Floor != null && allOfferTypeStrings.Contains(s.Floor));
                    }

                    if (offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.PRIMARY_MARKET || offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.AFTERMARKET)
                    {
                        offers = offers.Where(s => s.TypeOfMarket == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.typesOfMarketInSearch));
                    }

                    if (offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.AGENCY || offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.PRIVATE)
                    {
                        offers = offers.Where(s => s.Seller.RoleName == offerFilteringDTOConverted.userRolesInSearch.ToString());
                    }


                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<BusinessPremisesSaleOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<BusinessPremisesSaleOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Voivodeship = x.Voivodeship,
                            Price = x.Price,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);
                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is GarageRentFilteringDTO)
            {
                IQueryable<GarageRentOffer>? offers = (IQueryable<GarageRentOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                GarageRentFilteringDTO offerFilteringDTOConverted = (GarageRentFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }


                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<GarageRentOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<GarageRentOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            Voivodeship = x.Voivodeship,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);

                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is GarageSaleFilteringDTO)
            {
                IQueryable<GarageSaleOffer>? offers = (IQueryable<GarageSaleOffer>?)_offerRepository.GetAllOfType(offerFilteringDTO);
                GarageSaleFilteringDTO offerFilteringDTOConverted = (GarageSaleFilteringDTO)offerFilteringDTO;

                if (offers != null)
                {
                    if (userId != null)
                    {
                        offers = offers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (offerFilteringDTOConverted.RemoteControl && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.RemoteControl == offerFilteringDTOConverted.RemoteControl);
                    }
                    if (offerFilteringDTOConverted.HowRecent.HasValue && !offers.IsNullOrEmpty())
                    {
                        if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_WEEK)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-7));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_MONTH)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-30));
                        }
                        else if (offerFilteringDTOConverted.HowRecent == HowRecent.LAST_3_MONTHS)
                        {
                            offers = offers.Where(s => s.AddedDate >= DateTime.Now.AddDays(-90));
                        }
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers = offers.Where(s => s.Description.Contains(napis));
                        }
                    }

                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.IsNullOrEmpty())
                    {
                        offers = offers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }
                    if (offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.PRIMARY_MARKET || offerFilteringDTOConverted.typesOfMarketInSearch == TypesOfMarketInSearch.AFTERMARKET)
                    {
                        offers = offers.Where(s => s.TypeOfMarket == EnumHelper.GetDescriptionFromEnum(offerFilteringDTOConverted.typesOfMarketInSearch));
                    }

                    if (offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.AGENCY || offerFilteringDTOConverted.userRolesInSearch == UserRolesInSearch.PRIVATE)
                    {
                        offers = offers.Where(s => s.Seller.RoleName == offerFilteringDTOConverted.userRolesInSearch.ToString());
                    }


                    if (offers != null && sortOrder != null && sortOrder.Sort != null)
                    {
                        offers = Sorter<GarageSaleOffer>.Sort(offers, sortOrder.Sort);
                    }
                    else
                    {
                        sortOrder = new SortOrder();
                        sortOrder.Sort = new List<KeyValuePair<string, string>>();
                        sortOrder.Sort.Add(new KeyValuePair<string, string>("AddedDate", ""));

                        offers = Sorter<GarageSaleOffer>.Sort(offers, sortOrder.Sort);
                    }

                    OfferListing offerListing = new OfferListing();
                    offerListing.TotalCount = offers.Count();
                    offerListing.OfferFilteringDTO = offerFilteringDTO;
                    offerListing.Paging = paging;
                    offerListing.SortOrder = sortOrder;

                    offerListing.OfferDTOs = offers.AsEnumerable().Select(x =>
                    {
                        User? user = _userRepository.GetById(x.SellerId);
                        return new OfferListThumbnailDTO
                        {
                            Id = x.Id,
                            OfferTitle = x.OfferTitle,
                            City = x.City,
                            Price = x.Price,
                            Voivodeship = x.Voivodeship,
                            PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                            RoomCount = x.RoomCount,
                            Area = x.Area,
                            AgencyId = (user.OwnerOfAgencyId == null ? (user.AgentInAgencyId == null ? null : user.AgentInAgencyId) : user.OwnerOfAgencyId),
                            AgencyName = (user.OwnerOfAgencyId != null) ? _agencyRepository.GetById(user.OwnerOfAgencyId.Value).AgencyName : (user.AgentInAgencyId != null ? _agencyRepository.GetById(user.AgentInAgencyId.Value).AgencyName : null),
                            AgencyLogo = (user.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.OwnerOfAgencyId.ToString())).Result : (user.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, user.AgentInAgencyId.ToString())).Result : null),
                            SellerType = x.SellerType,
                            OfferType = x.OfferType,
                            EstateType = x.EstateType,
                            Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                        };
                    }).ToPagedList(paging.PageNumber, paging.PageSize);
                    return offerListing;
                }
                else
                {
                    return null;
                }
            }
            else if (offerFilteringDTO is UserOffersFilteringDTO)
            {
                var offers = _offerRepository.GetAllUsersOffers(userId, null);
                UserOffersFilteringDTO offerFilteringDTOConverted = (UserOffersFilteringDTO)offerFilteringDTO;
                int totalCount = 0;
                if (offers.roomOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.roomOffers = offers.roomOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.roomOffers = offers.roomOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.roomOffers.IsNullOrEmpty())
                    {
                        offers.roomOffers = offers.roomOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.roomOffers.Count();
                }                 
                if (offers.plotOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.plotOffers = offers.plotOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.plotOffers = offers.plotOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.plotOffers.IsNullOrEmpty())
                    {
                        offers.plotOffers = offers.plotOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.plotOffers.Count();
                }
                if (offers.houseRentOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.houseRentOffers.IsNullOrEmpty())
                    {
                        offers.houseRentOffers = offers.houseRentOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.houseRentOffers.Count();
                }
                if (offers.houseSaleOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.houseSaleOffers.IsNullOrEmpty())
                    {
                        offers.houseSaleOffers = offers.houseSaleOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.houseSaleOffers.Count();
                }
                if (offers.hallRentOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.hallRentOffers.IsNullOrEmpty())
                    {
                        offers.hallRentOffers = offers.hallRentOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.hallRentOffers.Count();
                }
                if (offers.hallSaleOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.hallSaleOffers.IsNullOrEmpty())
                    {
                        offers.hallSaleOffers = offers.hallSaleOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.hallSaleOffers.Count();
                }
                if (offers.apartmentRentOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.apartmentRentOffers.IsNullOrEmpty())
                    {
                        offers.apartmentRentOffers = offers.apartmentRentOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.apartmentRentOffers.Count();
                }
                if (offers.apartmentSaleOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.apartmentSaleOffers.IsNullOrEmpty())
                    {
                        offers.apartmentSaleOffers = offers.apartmentSaleOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.apartmentSaleOffers.Count();
                }
                if (offers.businessPremisesRentOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.businessPremisesRentOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesRentOffers = offers.businessPremisesRentOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.businessPremisesRentOffers.Count();
                }
                if (offers.businessPremisesSaleOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.businessPremisesSaleOffers.IsNullOrEmpty())
                    {
                        offers.businessPremisesSaleOffers = offers.businessPremisesSaleOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.businessPremisesSaleOffers.Count();
                }
                if (offers.garageRentOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.garageRentOffers.IsNullOrEmpty())
                    {
                        offers.garageRentOffers = offers.garageRentOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.garageRentOffers.Count();
                }
                if (offers.garageSaleOffers != null) 
                {
                    if (userId != null)
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(x => x.SellerId == userId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.City) && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(x => x.City == offerFilteringDTOConverted.City);
                    }
                    if (offerFilteringDTOConverted.MinPrice.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Price >= offerFilteringDTOConverted.MinPrice);
                    }
                    if (offerFilteringDTOConverted.MaxPrice.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Price <= offerFilteringDTOConverted.MaxPrice);
                    }
                    if (offerFilteringDTOConverted.OfferId.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Id == offerFilteringDTOConverted.OfferId);
                    }
                    if (!String.IsNullOrEmpty(offerFilteringDTOConverted.DescriptionFragment))
                    {
                        string[] rozbityString = offerFilteringDTOConverted.DescriptionFragment.Split(' ');
                        foreach (string napis in rozbityString)
                        {
                            offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Description.Contains(napis));
                        }
                    }
                    if (offerFilteringDTOConverted.minPricePerMeterSquared.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) >= offerFilteringDTOConverted.minPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.maxPricePerMeterSquared.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Area != null && s.Area != 0 && (s.Price / s.Area) <= offerFilteringDTOConverted.maxPricePerMeterSquared);
                    }
                    if (offerFilteringDTOConverted.minArea.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Area >= offerFilteringDTOConverted.minArea);
                    }
                    if (offerFilteringDTOConverted.maxArea.HasValue && !offers.garageSaleOffers.IsNullOrEmpty())
                    {
                        offers.garageSaleOffers = offers.garageSaleOffers.Where(s => s.Area <= offerFilteringDTOConverted.maxArea);
                    }

                    totalCount += offers.garageSaleOffers.Count();
                }

                //teraz mam wszystkie offers.typOferty przefiltrowane. Muszę tylko na podstawie wszystkich z nich stworzyć UserOfferListing trzymający miniaturki

                UserOfferListing offerListing = new UserOfferListing();
                offerListing.TotalCount = totalCount;
                offerListing.OfferFilteringDTO = offerFilteringDTO;
                offerListing.Paging = paging;
                offerListing.SortOrder = sortOrder;
                // offerListing.OfferDTOs = offers.roomOffers.Select(x => new OfferListThumbnailDTO



                User? user = (userId != null) ? _userRepository.GetById(userId.Value) : null;
                offerListing.UserDescription = user.Description;
                int? id_agencji;
                if(user.OwnerOfAgencyId != null)
                {
                    id_agencji = user.OwnerOfAgencyId;
                }
                else if (user.AgentInAgencyId != null)
                {
                    id_agencji = user.AgentInAgencyId;
                }
                else
                {
                    id_agencji = null;
                }
                string? userAgencyName = null;
                byte[]? userAgencyLogo = null;

                if (id_agencji != null)
                {
                    Agency? agency = _agencyRepository.GetById(id_agencji.Value);
                    offerListing.AgencyId = agency.Id;
                    offerListing.AgencyName = agency.AgencyName;
                    offerListing.AgencyAddress = agency.Address;
                    offerListing.AgencyCity = agency.City;
                    offerListing.AgencyPhoneNumber = agency.PhoneNumber;
                    offerListing.AgencyDescription = agency.Description;

                    userAgencyName = (id_agencji != null) ? _agencyRepository.GetById(id_agencji.Value).AgencyName : null;
                    userAgencyLogo = (id_agencji != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, id_agencji.ToString())).Result : null;
                    offerListing.AgencyLogo = userAgencyLogo;
                }
                

                IEnumerable<OfferListThumbnailDTO>? roomOfferListingDTOs = (offers.roomOffers!=null) ? offers.roomOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Voivodeship = x.Voivodeship,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? plotOfferListingDTOs = (offers.plotOffers != null) ? offers.plotOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Price = x.Price,
                    Voivodeship = x.Voivodeship,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? houseRentOfferListingDTOs = (offers.houseRentOffers != null) ? offers.houseRentOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Price = x.Price,
                    Voivodeship = x.Voivodeship,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? houseSaleOfferListingDTOs = (offers.houseSaleOffers != null) ? offers.houseSaleOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? hallRentOfferListingDTOs = (offers.hallRentOffers != null) ? offers.hallRentOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    Voivodeship = x.Voivodeship,
                    City = x.City,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? hallSaleOfferListingDTOs = (offers.hallSaleOffers != null) ? offers.hallSaleOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Price = x.Price,
                    Voivodeship = x.Voivodeship,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? apartmentRentOfferListingDTOs = (offers.apartmentRentOffers != null) ? offers.apartmentRentOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Voivodeship = x.Voivodeship,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? apartmentSaleOfferListingDTOs = (offers.apartmentSaleOffers != null) ? offers.apartmentSaleOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Voivodeship = x.Voivodeship,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? premisesRentOfferListingDTOs = (offers.businessPremisesRentOffers != null) ? offers.businessPremisesRentOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? premisesSaleOfferListingDTOs = (offers.businessPremisesSaleOffers != null) ? offers.businessPremisesSaleOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? garageRentOfferListingDTOs = (offers.garageRentOffers != null) ? offers.garageRentOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;
                IEnumerable<OfferListThumbnailDTO>? garageSaleOfferListingDTOs = (offers.garageSaleOffers != null) ? offers.garageSaleOffers.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    AgencyId = id_agencji,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (id_agencji != null) ? userAgencyLogo : null,
                    SellerType = x.SellerType,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                }) : null;

                List<OfferListThumbnailDTO> listaxd = new List<OfferListThumbnailDTO>();
                if(roomOfferListingDTOs!=null) listaxd.AddRange(roomOfferListingDTOs);
                if (plotOfferListingDTOs != null) listaxd.AddRange(plotOfferListingDTOs);
                if (houseRentOfferListingDTOs != null) listaxd.AddRange(houseRentOfferListingDTOs);
                if (houseSaleOfferListingDTOs != null) listaxd.AddRange(houseSaleOfferListingDTOs);
                if (hallRentOfferListingDTOs != null) listaxd.AddRange(hallRentOfferListingDTOs);
                if (hallSaleOfferListingDTOs != null) listaxd.AddRange(hallSaleOfferListingDTOs);
                if (apartmentRentOfferListingDTOs != null) listaxd.AddRange(apartmentRentOfferListingDTOs);
                if (apartmentSaleOfferListingDTOs != null) listaxd.AddRange(apartmentSaleOfferListingDTOs);
                if (premisesRentOfferListingDTOs != null) listaxd.AddRange(premisesRentOfferListingDTOs);
                if (premisesSaleOfferListingDTOs != null) listaxd.AddRange(premisesSaleOfferListingDTOs);
                if (garageRentOfferListingDTOs != null) listaxd.AddRange(garageRentOfferListingDTOs);
                if (garageSaleOfferListingDTOs != null) listaxd.AddRange(garageSaleOfferListingDTOs);

                offerListing.OfferDTOs = listaxd.OrderByDescending(o=>o.Id).ToPagedList(paging.PageNumber, paging.PageSize);

                if (userId != null)
                {
                    User? user2 = (userId != null) ? _userRepository.GetById(userId.HasValue == true ? userId.Value : default(int)): null;
                    offerListing.UserFirstName = user2.FirstName;
                    offerListing.UserLastName = user2.LastName;
                    offerListing.UserPhoneNumber = user2.PhoneNumber;
                    offerListing.UserCreatedAt = user2.CreatedDate;
                    offerListing.UserAvatar = _offerRepository.GetPhoto(System.IO.Path.Combine(avatarImgPath, user2.Id.ToString())).Result;
                }

                return offerListing;
            }
            else if(offerFilteringDTO is AgencyOffersFilteringDTO)
            {
                //get list of all offers (all of agency)
                //convert offerFilteringDTO to AgencyOffersFilteringDTO
                //skopiować WSZYSTKIE if'y dotyczące poszczególnych typów ofert do tamtego drugiego warunku dla agencji
                //
                
                string? userAgencyName = null;
                byte[]? userAgencyLogo = null;
                var offers = _offerRepository.GetAllUsersOffers(null, agencyId);
                AgencyOffersFilteringDTO offerFilteringDTOConverted = (AgencyOffersFilteringDTO)offerFilteringDTO;
                int totalCount = 0;
                if (offers.roomOffers != null)
                {                    
                    totalCount += offers.roomOffers.Count();
                }
                if (offers.plotOffers != null)
                {
                    totalCount += offers.plotOffers.Count();
                }
                if (offers.houseRentOffers != null)
                {
                    totalCount += offers.houseRentOffers.Count();
                }
                if (offers.houseSaleOffers != null)
                {
                    totalCount += offers.houseSaleOffers.Count();
                }
                if (offers.hallRentOffers != null)
                {
                    totalCount += offers.hallRentOffers.Count();
                }
                if (offers.hallSaleOffers != null)
                {
                    totalCount += offers.hallSaleOffers.Count();
                }
                if (offers.apartmentRentOffers != null)
                {
                    totalCount += offers.apartmentRentOffers.Count();
                }
                if (offers.apartmentSaleOffers != null)
                {
                    totalCount += offers.apartmentSaleOffers.Count();
                }
                if (offers.businessPremisesRentOffers != null)
                {
                    totalCount += offers.businessPremisesRentOffers.Count();
                }
                if (offers.businessPremisesSaleOffers != null)
                {
                    totalCount += offers.businessPremisesSaleOffers.Count();
                }
                if (offers.garageRentOffers != null)
                {
                    totalCount += offers.garageRentOffers.Count();
                }
                if (offers.garageSaleOffers != null)
                {
                    totalCount += offers.garageSaleOffers.Count();
                }

                //teraz mam wszystkie offers.typOferty przefiltrowane. Muszę tylko na podstawie wszystkich z nich stworzyć UserOfferListing trzymający miniaturki
                AgencyOfferListing offerListing = new AgencyOfferListing();
                offerListing.TotalCount = totalCount;
                offerListing.OfferFilteringDTO = offerFilteringDTO;
                offerListing.Paging = paging;
                offerListing.SortOrder = sortOrder;

                userAgencyName = (agencyId != null) ? _agencyRepository.GetById(agencyId.Value).AgencyName : null;
                userAgencyLogo = (agencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, agencyId.ToString())).Result : null;
                offerListing.AgencyLogo = userAgencyLogo;

                IEnumerable<AgencyOffersListThumbnailDTO>? roomOfferListingDTOs = (offers.roomOffers != null) ? offers.roomOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? plotOfferListingDTOs = (offers.plotOffers != null) ? offers.plotOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? houseRentOfferListingDTOs = (offers.houseRentOffers != null) ? offers.houseRentOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? houseSaleOfferListingDTOs = (offers.houseSaleOffers != null) ? offers.houseSaleOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? hallRentOfferListingDTOs = (offers.hallRentOffers != null) ? offers.hallRentOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? hallSaleOfferListingDTOs = (offers.hallSaleOffers != null) ? offers.hallSaleOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? apartmentRentOfferListingDTOs = (offers.apartmentRentOffers != null) ? offers.apartmentRentOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? apartmentSaleOfferListingDTOs = (offers.apartmentSaleOffers != null) ? offers.apartmentSaleOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? premisesRentOfferListingDTOs = (offers.businessPremisesRentOffers != null) ? offers.businessPremisesRentOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? premisesSaleOfferListingDTOs = (offers.businessPremisesSaleOffers != null) ? offers.businessPremisesSaleOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? garageRentOfferListingDTOs = (offers.garageRentOffers != null) ? offers.garageRentOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;
                IEnumerable<AgencyOffersListThumbnailDTO>? garageSaleOfferListingDTOs = (offers.garageSaleOffers != null) ? offers.garageSaleOffers.Select(x => new AgencyOffersListThumbnailDTO
                {
                    Id = x.Id,
                    OfferTitle = x.OfferTitle,
                    City = x.City,
                    Voivodeship = x.Voivodeship,
                    Price = x.Price,
                    PriceForOneSquareMeter = (x.Area == 0 || x.Area == null) ? (0) : (int)(x.Price / x.Area),
                    RoomCount = x.RoomCount,
                    Area = x.Area,
                    OfferType = x.OfferType,
                    EstateType = x.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerImgPath, x.Id.ToString())).Result,
                    AgencyId = agencyId,
                    AgencyName = (userAgencyName != null) ? userAgencyName : null,
                    AgencyLogo = (agencyId != null) ? userAgencyLogo : null,
                }) : null;

                List<AgencyOffersListThumbnailDTO> listaxd = new List<AgencyOffersListThumbnailDTO>();
                if (roomOfferListingDTOs != null) listaxd.AddRange(roomOfferListingDTOs);
                if (plotOfferListingDTOs != null) listaxd.AddRange(plotOfferListingDTOs);
                if (houseRentOfferListingDTOs != null) listaxd.AddRange(houseRentOfferListingDTOs);
                if (houseSaleOfferListingDTOs != null) listaxd.AddRange(houseSaleOfferListingDTOs);
                if (hallRentOfferListingDTOs != null) listaxd.AddRange(hallRentOfferListingDTOs);
                if (hallSaleOfferListingDTOs != null) listaxd.AddRange(hallSaleOfferListingDTOs);
                if (apartmentRentOfferListingDTOs != null) listaxd.AddRange(apartmentRentOfferListingDTOs);
                if (apartmentSaleOfferListingDTOs != null) listaxd.AddRange(apartmentSaleOfferListingDTOs);
                if (premisesRentOfferListingDTOs != null) listaxd.AddRange(premisesRentOfferListingDTOs);
                if (premisesSaleOfferListingDTOs != null) listaxd.AddRange(premisesSaleOfferListingDTOs);
                if (garageRentOfferListingDTOs != null) listaxd.AddRange(garageRentOfferListingDTOs);
                if (garageSaleOfferListingDTOs != null) listaxd.AddRange(garageSaleOfferListingDTOs);

                offerListing.AgencyOfferDTOs = listaxd.OrderByDescending(o => o.Id).ToPagedList(paging.PageNumber, paging.PageSize);

                if (agencyId != null)
                {
                    Agency? agency = _agencyRepository.GetById(agencyId.HasValue == true ? agencyId.Value : default(int));
                    if(agency != null)
                    {
                        offerListing.AgencyName = agency.AgencyName;
                        offerListing.AgencyAddress = agency.Address;
                        offerListing.AgencyCity = agency.City;
                        offerListing.AgencyPhoneNumber = agency.PhoneNumber;
                        offerListing.AgencyDescription = agency.Description;
                        offerListing.AgencyCreatedAt = agency.CreatedDate;
                        offerListing.AgencyInvitationGuid = agency.InvitationGuid;
                        offerListing.AgencyLogo = _offerRepository.GetPhoto(System.IO.Path.Combine(agencyLogoImgPath, agency.Id.ToString())).Result;
                    }
                }
                return offerListing;
            }
            else
            {
                return null;
            }

        }

        public bool Create(OfferCreateDTO offerCreate, int userCreatedId, out string errorMessage)
        {
            Offer? result;

            if (offerCreate is PlotCreateOfferDTO)
            {
                PlotCreateOfferDTO offerCreateConverted = (PlotCreateOfferDTO)offerCreate;
                PlotOffer offer = _mapper.Map<PlotOffer>(offerCreateConverted);

                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.PLOT);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
                   
                result = offer;
            }
            else if (offerCreate is RoomOfferCreateDTO)
            {
                RoomOfferCreateDTO offerCreateConverted = (RoomOfferCreateDTO)offerCreate;
                RoomRentingOffer offer = _mapper.Map<RoomRentingOffer>(offerCreateConverted);

                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.ROOM);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is HouseRentOfferCreateDTO)
            {
                HouseRentOfferCreateDTO offerCreateConverted = (HouseRentOfferCreateDTO)offerCreate;
                HouseRentOffer offer = _mapper.Map<HouseRentOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is HouseSaleOfferCreateDTO)
            {
                HouseSaleOfferCreateDTO offerCreateConverted = (HouseSaleOfferCreateDTO)offerCreate;
                HouseSaleOffer offer = _mapper.Map<HouseSaleOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is HallRentOfferCreateDTO)
            {
                HallRentOfferCreateDTO offerCreateConverted = (HallRentOfferCreateDTO)offerCreate;
                HallRentOffer offer = _mapper.Map<HallRentOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.HALL);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is HallSaleOfferCreateDTO)
            {
                HallSaleOfferCreateDTO offerCreateConverted = (HallSaleOfferCreateDTO)offerCreate;
                HallSaleOffer offer = _mapper.Map<HallSaleOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.HALL);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is PremisesRentOfferCreateDTO)
            {
                PremisesRentOfferCreateDTO offerCreateConverted = (PremisesRentOfferCreateDTO)offerCreate;
                BusinessPremisesRentOffer offer = _mapper.Map<BusinessPremisesRentOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is PremisesSaleOfferCreateDTO)
            {
                PremisesSaleOfferCreateDTO offerCreateConverted = (PremisesSaleOfferCreateDTO)offerCreate;
                BusinessPremisesSaleOffer offer = _mapper.Map<BusinessPremisesSaleOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is GarageRentCreateOfferDTO)
            {
                GarageRentCreateOfferDTO offerCreateConverted = (GarageRentCreateOfferDTO)offerCreate;
                GarageRentOffer offer = _mapper.Map<GarageRentOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is GarageSaleOfferCreateDTO)
            {
                GarageSaleOfferCreateDTO offerCreateConverted = (GarageSaleOfferCreateDTO)offerCreate;
                GarageSaleOffer offer = _mapper.Map<GarageSaleOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is ApartmentRentOfferCreateDTO)
            {
                ApartmentRentOfferCreateDTO offerCreateConverted = (ApartmentRentOfferCreateDTO)offerCreate;
                ApartmentRentOffer offer = _mapper.Map<ApartmentRentOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else if (offerCreate is ApartmentSaleOfferCreateDTO)
            {
                ApartmentSaleOfferCreateDTO offerCreateConverted = (ApartmentSaleOfferCreateDTO)offerCreate;
                ApartmentSaleOffer offer = _mapper.Map<ApartmentSaleOffer>(offerCreateConverted);

                offer.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
                offer.SellerId = userCreatedId;
                offer.AddedDate = DateTime.UtcNow;
                offer.EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT);
                offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);

                result = offer;
            }
            else result = null;
            

            //if (offer.EstateType == "House" && offer.Area != 0 && offer.Area != null)
            //{
            //    offer.PriceForOneSquareMeter=Math.Round((decimal)(offer.Price / offer.Area), 2);
            //}
            //else offer.PriceForOneSquareMeter = offer.Price;

            try
            {
                if(result != null)
                {
                    _offerRepository.AddAndSaveChanges(result);
                }
                else
                {
                    errorMessage = "Error while saving offer to database!";
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errorMessage = ErrorMessageHelper.ErrorCreatingOffer;
                return false;
            }
            errorMessage = "";
            return true;
        }

        public bool Update(OfferUpdateDTO offerUpdate, string type, int offerId, int userId, out string errorMessage)
        {
            Offer? offerxd = _offerRepository.GetOffer(offerId, type);
            if (offerxd == null)
            {
                errorMessage = ErrorMessageHelper.NoOffer;
                return false;
            }
            else if(offerxd.SellerId != userId)
            {
                errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer;
                return false;
            }

            if (offerUpdate is PlotOfferUpdateDTO)
            {
                PlotOfferUpdateDTO offerCreateConverted = (PlotOfferUpdateDTO)offerUpdate;
                PlotOffer plotOffer = (PlotOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, plotOffer);

                plotOffer.PlotType = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.PlotType);
                plotOffer.IsFenced = offerCreateConverted.IsFenced;
                plotOffer.Location = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Location);
                plotOffer.Dimensions = offerCreateConverted.Dimensions;
                plotOffer.FieldDriveway = offerCreateConverted.FieldDriveway;
                plotOffer.PavedDriveway = offerCreateConverted.PavedDriveway;
                plotOffer.AsphaltDriveway = offerCreateConverted.AsphaltDriveway;
                plotOffer.Phone = offerCreateConverted.Phone;
                plotOffer.Water = offerCreateConverted.Water;
                plotOffer.Electricity = offerCreateConverted.Electricity;
                plotOffer.Sewerage = offerCreateConverted.Sewerage;
                plotOffer.Gas = offerCreateConverted.Gas;
                plotOffer.SepticTank = offerCreateConverted.SepticTank;
                plotOffer.SewageTreatmentPlant = offerCreateConverted.SewageTreatmentPlant;
                plotOffer.Forest = offerCreateConverted.Forest;
                plotOffer.Lake = offerCreateConverted.Lake;
                plotOffer.Mountains = offerCreateConverted.Mountains;
                plotOffer.Sea = offerCreateConverted.Sea;
                plotOffer.OpenArea = offerCreateConverted.OpenArea;
            }
            else if (offerUpdate is RoomOfferUpdateDTO)
            {
                RoomOfferUpdateDTO offerCreateConverted = (RoomOfferUpdateDTO)offerUpdate;
                RoomRentingOffer roomOffer = (RoomRentingOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, roomOffer);

                roomOffer.AdditionalFees = offerCreateConverted.AdditionalFees;
                roomOffer.Deposit = offerCreateConverted.Deposit;
                roomOffer.Floor = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Floor);
                roomOffer.NumberOfPeopleInTheRoom = offerCreateConverted.NumberOfPeopleInTheRoom;
                roomOffer.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfBuilding);
                roomOffer.AvailableFromDate = offerCreateConverted.AvailableFromDate;
                roomOffer.AvailableForStudents = offerCreateConverted.AvailableForStudents;
                roomOffer.OnlyForNonsmoking = offerCreateConverted.OnlyForNonsmoking;
                roomOffer.Furniture = offerCreateConverted.Furniture;
                roomOffer.WashingMachine = offerCreateConverted.WashingMachine;
                roomOffer.Dishwasher = offerCreateConverted.Dishwasher;
                roomOffer.Fridge = offerCreateConverted.Fridge;
                roomOffer.Stove = offerCreateConverted.Stove;
                roomOffer.Oven = offerCreateConverted.Oven;
                roomOffer.TV = offerCreateConverted.TV;
                roomOffer.Internet = offerCreateConverted.Internet;
                roomOffer.CableTV = offerCreateConverted.CableTV;
                roomOffer.HomePhone = offerCreateConverted.HomePhone;

            }
            else if (offerUpdate is HouseRentOfferUpdateDTO)
            {
                HouseRentOfferUpdateDTO offerUpdateConverted = (HouseRentOfferUpdateDTO)offerUpdate;
                HouseRentOffer houserentOffer = (HouseRentOffer)offerxd;
                MapUpdateDTOToObject(offerUpdateConverted, houserentOffer);

                houserentOffer.LandArea = offerUpdateConverted.LandArea;
                houserentOffer.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.TypeOfBuilding);
                houserentOffer.FloorCount = offerUpdateConverted.FloorCount;
                houserentOffer.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.BuildingMaterial);
                houserentOffer.ConstructionYear = offerUpdateConverted.ConstructionYear;
                houserentOffer.AtticType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.AtticType);
                houserentOffer.RoofType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.RoofType);
                houserentOffer.RoofingType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.RoofingType);
                houserentOffer.FinishCondition = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.FinishCondition);
                houserentOffer.WindowsType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.WindowsType);
                houserentOffer.Location = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.Location);
                houserentOffer.AvailableFromDate = offerUpdateConverted.AvailableFromDate;
                houserentOffer.IsARecreationHouse = offerUpdateConverted.IsARecreationHouse;
                houserentOffer.AntiBurglaryBlinds = offerUpdateConverted.AntiBurglaryBlinds;
                houserentOffer.AntiBurglaryWindowsOrDoors = offerUpdateConverted.AntiBurglaryWindowsOrDoors;
                houserentOffer.IntercomOrVideophone = offerUpdateConverted.IntercomOrVideophone;
                houserentOffer.MonitoringOrSecurity = offerUpdateConverted.MonitoringOrSecurity;
                houserentOffer.AlarmSystem = offerUpdateConverted.AlarmSystem;
                houserentOffer.ClosedArea = offerUpdateConverted.ClosedArea;
                houserentOffer.BrickFence = offerUpdateConverted.BrickFence;
                houserentOffer.Net = offerUpdateConverted.Net;
                houserentOffer.MetalFence = offerUpdateConverted.MetalFence;
                houserentOffer.WoodenFence = offerUpdateConverted.WoodenFence;
                houserentOffer.ConcreteFence = offerUpdateConverted.ConcreteFence;
                houserentOffer.Hedge = offerUpdateConverted.Hedge;
                houserentOffer.OtherFencing = offerUpdateConverted.OtherFencing;
                houserentOffer.Geothermics = offerUpdateConverted.Geothermics;
                houserentOffer.OilHeating = offerUpdateConverted.OilHeating;
                houserentOffer.ElectricHeating = offerUpdateConverted.ElectricHeating;
                houserentOffer.DistrictHeating = offerUpdateConverted.DistrictHeating;
                houserentOffer.TileStoves = offerUpdateConverted.TileStoves;
                houserentOffer.GasHeating = offerUpdateConverted.GasHeating;
                houserentOffer.CoalHeating = offerUpdateConverted.CoalHeating;
                houserentOffer.Biomass = offerUpdateConverted.Biomass;
                houserentOffer.HeatPump = offerUpdateConverted.HeatPump;
                houserentOffer.SolarCollector = offerUpdateConverted.SolarCollector;
                houserentOffer.FireplaceHeating = offerUpdateConverted.FireplaceHeating;
                houserentOffer.Internet = offerUpdateConverted.Internet;
                houserentOffer.CableTV = offerUpdateConverted.CableTV;
                houserentOffer.HomePhone = offerUpdateConverted.HomePhone;
                houserentOffer.Water = offerUpdateConverted.Water;
                houserentOffer.Electricity = offerUpdateConverted.Electricity;
                houserentOffer.SewageSystem = offerUpdateConverted.SewageSystem;
                houserentOffer.Gas = offerUpdateConverted.Gas;
                houserentOffer.SepticTank = offerUpdateConverted.SepticTank;
                houserentOffer.SewageTreatmentPlant = offerUpdateConverted.SewageTreatmentPlant;
                houserentOffer.FieldDriveway = offerUpdateConverted.FieldDriveway;
                houserentOffer.PavedDriveway = offerUpdateConverted.PavedDriveway;
                houserentOffer.AsphaltDriveway = offerUpdateConverted.AsphaltDriveway;
                houserentOffer.SwimmingPool = offerUpdateConverted.SwimmingPool;
                houserentOffer.ParkingSpace = offerUpdateConverted.ParkingSpace;
                houserentOffer.Basement = offerUpdateConverted.Basement;
                houserentOffer.Attic = offerUpdateConverted.Attic;
                houserentOffer.Garden = offerUpdateConverted.Garden;
                houserentOffer.Furnishings = offerUpdateConverted.Furnishings;
                houserentOffer.AirConditioning = offerUpdateConverted.AirConditioning;
                houserentOffer.AvailableForStudents = offerUpdateConverted.AvailableForStudents;
                houserentOffer.OnlyForNonsmoking = offerUpdateConverted.OnlyForNonsmoking;
                houserentOffer.Rent = offerUpdateConverted.Rent;
                
            }
            else if (offerUpdate is HouseSaleOfferUpdateDTO)
            {
                HouseSaleOfferUpdateDTO offerUpdateConverted = (HouseSaleOfferUpdateDTO)offerUpdate;
                HouseSaleOffer housesaleOffer = (HouseSaleOffer)offerxd;
                MapUpdateDTOToObject(offerUpdateConverted, housesaleOffer);

                housesaleOffer.LandArea = offerUpdateConverted.LandArea;
                housesaleOffer.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.TypeOfBuilding);
                housesaleOffer.FloorCount = offerUpdateConverted.FloorCount;
                housesaleOffer.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.BuildingMaterial);
                housesaleOffer.ConstructionYear = offerUpdateConverted.ConstructionYear;
                housesaleOffer.AtticType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.AtticType);
                housesaleOffer.RoofType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.RoofType);
                housesaleOffer.RoofingType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.RoofingType);
                housesaleOffer.FinishCondition = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.FinishCondition);
                housesaleOffer.WindowsType = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.WindowsType);
                housesaleOffer.Location = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.Location);
                housesaleOffer.AvailableFromDate = offerUpdateConverted.AvailableFromDate;
                housesaleOffer.IsARecreationHouse = offerUpdateConverted.IsARecreationHouse;
                housesaleOffer.AntiBurglaryBlinds = offerUpdateConverted.AntiBurglaryBlinds;
                housesaleOffer.AntiBurglaryWindowsOrDoors = offerUpdateConverted.AntiBurglaryWindowsOrDoors;
                housesaleOffer.IntercomOrVideophone = offerUpdateConverted.IntercomOrVideophone;
                housesaleOffer.MonitoringOrSecurity = offerUpdateConverted.MonitoringOrSecurity;
                housesaleOffer.AlarmSystem = offerUpdateConverted.AlarmSystem;
                housesaleOffer.ClosedArea = offerUpdateConverted.ClosedArea;
                housesaleOffer.BrickFence = offerUpdateConverted.BrickFence;
                housesaleOffer.Net = offerUpdateConverted.Net;
                housesaleOffer.MetalFence = offerUpdateConverted.MetalFence;
                housesaleOffer.WoodenFence = offerUpdateConverted.WoodenFence;
                housesaleOffer.ConcreteFence = offerUpdateConverted.ConcreteFence;
                housesaleOffer.Hedge = offerUpdateConverted.Hedge;
                housesaleOffer.OtherFencing = offerUpdateConverted.OtherFencing;
                housesaleOffer.Geothermics = offerUpdateConverted.Geothermics;
                housesaleOffer.OilHeating = offerUpdateConverted.OilHeating;
                housesaleOffer.ElectricHeating = offerUpdateConverted.ElectricHeating;
                housesaleOffer.DistrictHeating = offerUpdateConverted.DistrictHeating;
                housesaleOffer.TileStoves = offerUpdateConverted.TileStoves;
                housesaleOffer.GasHeating = offerUpdateConverted.GasHeating;
                housesaleOffer.CoalHeating = offerUpdateConverted.CoalHeating;
                housesaleOffer.Biomass = offerUpdateConverted.Biomass;
                housesaleOffer.HeatPump = offerUpdateConverted.HeatPump;
                housesaleOffer.SolarCollector = offerUpdateConverted.SolarCollector;
                housesaleOffer.FireplaceHeating = offerUpdateConverted.FireplaceHeating;
                housesaleOffer.Internet = offerUpdateConverted.Internet;
                housesaleOffer.CableTV = offerUpdateConverted.CableTV;
                housesaleOffer.HomePhone = offerUpdateConverted.HomePhone;
                housesaleOffer.Water = offerUpdateConverted.Water;
                housesaleOffer.Electricity = offerUpdateConverted.Electricity;
                housesaleOffer.SewageSystem = offerUpdateConverted.SewageSystem;
                housesaleOffer.Gas = offerUpdateConverted.Gas;
                housesaleOffer.SepticTank = offerUpdateConverted.SepticTank;
                housesaleOffer.SewageTreatmentPlant = offerUpdateConverted.SewageTreatmentPlant;
                housesaleOffer.FieldDriveway = offerUpdateConverted.FieldDriveway;
                housesaleOffer.PavedDriveway = offerUpdateConverted.PavedDriveway;
                housesaleOffer.AsphaltDriveway = offerUpdateConverted.AsphaltDriveway;
                housesaleOffer.SwimmingPool = offerUpdateConverted.SwimmingPool;
                housesaleOffer.ParkingSpace = offerUpdateConverted.ParkingSpace;
                housesaleOffer.Basement = offerUpdateConverted.Basement;
                housesaleOffer.Attic = offerUpdateConverted.Attic;
                housesaleOffer.Garden = offerUpdateConverted.Garden;
                housesaleOffer.Furnishings = offerUpdateConverted.Furnishings;
                housesaleOffer.AirConditioning = offerUpdateConverted.AirConditioning;
                housesaleOffer.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(offerUpdateConverted.TypeOfMarket);
            }
            else if (offerUpdate is HallRentUpdateOfferDTO)
            {
                HallRentUpdateOfferDTO offerCreateConverted = (HallRentUpdateOfferDTO)offerUpdate;
                HallRentOffer hallRentOffer = (HallRentOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, hallRentOffer);

                hallRentOffer.Height = offerCreateConverted.Height;
                hallRentOffer.Construction = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Construction);
                hallRentOffer.ParkingSpace = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.ParkingSpace);
                hallRentOffer.FinishCondition = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.FinishCondition);
                hallRentOffer.Flooring = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Flooring);
                hallRentOffer.Heating = offerCreateConverted.Heating;
                hallRentOffer.Fencing = offerCreateConverted.Fencing;
                hallRentOffer.HasOfficeRooms = offerCreateConverted.HasOfficeRooms;
                hallRentOffer.HasSocialFacilities = offerCreateConverted.HasSocialFacilities;
                hallRentOffer.HasRamp = offerCreateConverted.HasRamp;
                hallRentOffer.Storage = offerCreateConverted.Storage;
                hallRentOffer.Production = offerCreateConverted.Production;
                hallRentOffer.Office = offerCreateConverted.Office;
                hallRentOffer.Commercial = offerCreateConverted.Commercial;
                hallRentOffer.AntiBurglaryBlinds = offerCreateConverted.AntiBurglaryBlinds;
                hallRentOffer.AntiBurglaryWindowsOrDoors = offerCreateConverted.AntiBurglaryWindowsOrDoors;
                hallRentOffer.IntercomOrVideophone = offerCreateConverted.IntercomOrVideophone;
                hallRentOffer.MonitoringOrSecurity = offerCreateConverted.MonitoringOrSecurity;
                hallRentOffer.AlarmSystem = offerCreateConverted.AlarmSystem;
                hallRentOffer.ClosedArea = offerCreateConverted.ClosedArea;
                hallRentOffer.Internet = offerCreateConverted.Internet;
                hallRentOffer.ThreePhaseElectricPower = offerCreateConverted.ThreePhaseElectricPower;
                hallRentOffer.Phone = offerCreateConverted.Phone;
                hallRentOffer.Water = offerCreateConverted.Water;
                hallRentOffer.Electricity = offerCreateConverted.Electricity;
                hallRentOffer.Sewerage = offerCreateConverted.Sewerage;
                hallRentOffer.Gas = offerCreateConverted.Gas;
                hallRentOffer.Area = offerCreateConverted.Area;
                hallRentOffer.SepticTank = offerCreateConverted.SepticTank;
                hallRentOffer.SewageTreatmentPlant = offerCreateConverted.SewageTreatmentPlant;
                hallRentOffer.FieldDriveway = offerCreateConverted.FieldDriveway;
                hallRentOffer.PavedDriveway = offerCreateConverted.PavedDriveway;
                hallRentOffer.AsphaltDriveway = offerCreateConverted.AsphaltDriveway;
                hallRentOffer.AvailableFromDate = offerCreateConverted.AvailableFromDate;
            }
            else if (offerUpdate is HallSaleUpdateOfferDTO)
            {
                HallSaleUpdateOfferDTO offerCreateConverted = (HallSaleUpdateOfferDTO)offerUpdate;
                HallSaleOffer hallSaleOffer = (HallSaleOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, hallSaleOffer);

                hallSaleOffer.Height = offerCreateConverted.Height;
                hallSaleOffer.Construction = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Construction);
                hallSaleOffer.ParkingSpace = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.ParkingSpace);
                hallSaleOffer.FinishCondition = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.FinishCondition);
                hallSaleOffer.Flooring = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Flooring);
                hallSaleOffer.Heating = offerCreateConverted.Heating;
                hallSaleOffer.Fencing = offerCreateConverted.Fencing;
                hallSaleOffer.HasOfficeRooms = offerCreateConverted.HasOfficeRooms;
                hallSaleOffer.HasSocialFacilities = offerCreateConverted.HasSocialFacilities;
                hallSaleOffer.HasRamp = offerCreateConverted.HasRamp;
                hallSaleOffer.Storage = offerCreateConverted.Storage;
                hallSaleOffer.Production = offerCreateConverted.Production;
                hallSaleOffer.Office = offerCreateConverted.Office;
                hallSaleOffer.Commercial = offerCreateConverted.Commercial;
                hallSaleOffer.AntiBurglaryBlinds = offerCreateConverted.AntiBurglaryBlinds;
                hallSaleOffer.AntiBurglaryWindowsOrDoors = offerCreateConverted.AntiBurglaryWindowsOrDoors;
                hallSaleOffer.IntercomOrVideophone = offerCreateConverted.IntercomOrVideophone;
                hallSaleOffer.MonitoringOrSecurity = offerCreateConverted.MonitoringOrSecurity;
                hallSaleOffer.AlarmSystem = offerCreateConverted.AlarmSystem;
                hallSaleOffer.ClosedArea = offerCreateConverted.ClosedArea;
                hallSaleOffer.Internet = offerCreateConverted.Internet;
                hallSaleOffer.ThreePhaseElectricPower = offerCreateConverted.ThreePhaseElectricPower;
                hallSaleOffer.Phone = offerCreateConverted.Phone;
                hallSaleOffer.Water = offerCreateConverted.Water;
                hallSaleOffer.Electricity = offerCreateConverted.Electricity;
                hallSaleOffer.Sewerage = offerCreateConverted.Sewerage;
                hallSaleOffer.Gas = offerCreateConverted.Gas;
                hallSaleOffer.Area = offerCreateConverted.Area;
                hallSaleOffer.SepticTank = offerCreateConverted.SepticTank;
                hallSaleOffer.SewageTreatmentPlant = offerCreateConverted.SewageTreatmentPlant;
                hallSaleOffer.FieldDriveway = offerCreateConverted.FieldDriveway;
                hallSaleOffer.PavedDriveway = offerCreateConverted.PavedDriveway;
                hallSaleOffer.AsphaltDriveway = offerCreateConverted.AsphaltDriveway;
                hallSaleOffer.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfMarket);
            }
            else if (offerUpdate is BusinessPremisesRentUpdateOfferDTO)
            {
                BusinessPremisesRentUpdateOfferDTO offerCreateConverted = (BusinessPremisesRentUpdateOfferDTO)offerUpdate;
                BusinessPremisesRentOffer premisesRentOffer = (BusinessPremisesRentOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, premisesRentOffer);

                premisesRentOffer.Location = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Location);
                premisesRentOffer.Floor = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Floor);
                premisesRentOffer.YearOfConstruction = offerCreateConverted.YearOfConstruction;
                premisesRentOffer.FinishCondition = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.FinishCondition);
                premisesRentOffer.AntiBurglaryBlinds = offerCreateConverted.AntiBurglaryBlinds;
                premisesRentOffer.AntiBurglaryWindowsOrDoors = offerCreateConverted.AntiBurglaryWindowsOrDoors;
                premisesRentOffer.IntercomOrVideophone = offerCreateConverted.IntercomOrVideophone;
                premisesRentOffer.MonitoringOrSecurity = offerCreateConverted.MonitoringOrSecurity;
                premisesRentOffer.AlarmSystem = offerCreateConverted.AlarmSystem;
                premisesRentOffer.ClosedArea = offerCreateConverted.ClosedArea;
                premisesRentOffer.Service = offerCreateConverted.Service;
                premisesRentOffer.Gastronomic = offerCreateConverted.Gastronomic;
                premisesRentOffer.Office = offerCreateConverted.Office;
                premisesRentOffer.Industrial = offerCreateConverted.Industrial;
                premisesRentOffer.Commercial = offerCreateConverted.Commercial;
                premisesRentOffer.Hotel = offerCreateConverted.Hotel;
                premisesRentOffer.Internet = offerCreateConverted.Internet;
                premisesRentOffer.CableTV = offerCreateConverted.CableTV;
                premisesRentOffer.HomePhone = offerCreateConverted.HomePhone;
                premisesRentOffer.Water = offerCreateConverted.Water;
                premisesRentOffer.Electricity = offerCreateConverted.Electricity;
                premisesRentOffer.SewageSystem = offerCreateConverted.SewageSystem;
                premisesRentOffer.Gas = offerCreateConverted.Gas;
                premisesRentOffer.SepticTank = offerCreateConverted.SepticTank;
                premisesRentOffer.SewageTreatmentPlant = offerCreateConverted.SewageTreatmentPlant;
                premisesRentOffer.Shopwindow = offerCreateConverted.Shopwindow;
                premisesRentOffer.ParkingSpace = offerCreateConverted.ParkingSpace;
                premisesRentOffer.AsphaltDriveway = offerCreateConverted.AsphaltDriveway;
                premisesRentOffer.Heating = offerCreateConverted.Heating;
                premisesRentOffer.Elevator = offerCreateConverted.Elevator;
                premisesRentOffer.Furnishings = offerCreateConverted.Furnishings;
                premisesRentOffer.AirConditioning = offerCreateConverted.AirConditioning;
                premisesRentOffer.AvailableFromDate = offerCreateConverted.AvailableFromDate;
            }
            else if (offerUpdate is BusinessPremisesSaleUpdateOfferDTO)
            {
                BusinessPremisesSaleUpdateOfferDTO offerCreateConverted = (BusinessPremisesSaleUpdateOfferDTO)offerUpdate;
                BusinessPremisesSaleOffer premisesSaleOffer = (BusinessPremisesSaleOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, premisesSaleOffer);

                premisesSaleOffer.Location = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Location);
                premisesSaleOffer.Floor = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Floor);
                premisesSaleOffer.YearOfConstruction = offerCreateConverted.YearOfConstruction;
                premisesSaleOffer.FinishCondition = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.FinishCondition);
                premisesSaleOffer.AntiBurglaryBlinds = offerCreateConverted.AntiBurglaryBlinds;
                premisesSaleOffer.AntiBurglaryWindowsOrDoors = offerCreateConverted.AntiBurglaryWindowsOrDoors;
                premisesSaleOffer.IntercomOrVideophone = offerCreateConverted.IntercomOrVideophone;
                premisesSaleOffer.MonitoringOrSecurity = offerCreateConverted.MonitoringOrSecurity;
                premisesSaleOffer.AlarmSystem = offerCreateConverted.AlarmSystem;
                premisesSaleOffer.ClosedArea = offerCreateConverted.ClosedArea;
                premisesSaleOffer.Service = offerCreateConverted.Service;
                premisesSaleOffer.Gastronomic = offerCreateConverted.Gastronomic;
                premisesSaleOffer.Office = offerCreateConverted.Office;
                premisesSaleOffer.Industrial = offerCreateConverted.Industrial;
                premisesSaleOffer.Commercial = offerCreateConverted.Commercial;
                premisesSaleOffer.Hotel = offerCreateConverted.Hotel;
                premisesSaleOffer.Internet = offerCreateConverted.Internet;
                premisesSaleOffer.CableTV = offerCreateConverted.CableTV;
                premisesSaleOffer.HomePhone = offerCreateConverted.HomePhone;
                premisesSaleOffer.Water = offerCreateConverted.Water;
                premisesSaleOffer.Electricity = offerCreateConverted.Electricity;
                premisesSaleOffer.SewageSystem = offerCreateConverted.SewageSystem;
                premisesSaleOffer.Gas = offerCreateConverted.Gas;
                premisesSaleOffer.SepticTank = offerCreateConverted.SepticTank;
                premisesSaleOffer.SewageTreatmentPlant = offerCreateConverted.SewageTreatmentPlant;
                premisesSaleOffer.Shopwindow = offerCreateConverted.Shopwindow;
                premisesSaleOffer.ParkingSpace = offerCreateConverted.ParkingSpace;
                premisesSaleOffer.AsphaltDriveway = offerCreateConverted.AsphaltDriveway;
                premisesSaleOffer.Heating = offerCreateConverted.Heating;
                premisesSaleOffer.Elevator = offerCreateConverted.Elevator;
                premisesSaleOffer.Furnishings = offerCreateConverted.Furnishings;
                premisesSaleOffer.AirConditioning = offerCreateConverted.AirConditioning;
                premisesSaleOffer.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfMarket);
            }
            else if (offerUpdate is GarageUpdateRentOfferDTO)
            {
                GarageUpdateRentOfferDTO offerCreateConverted = (GarageUpdateRentOfferDTO)offerUpdate;
                GarageRentOffer garageRentOffer = (GarageRentOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, garageRentOffer);

                garageRentOffer.Construction = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Construction);
                garageRentOffer.Location = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Location);
                garageRentOffer.Lighting = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Lighting);
                garageRentOffer.Heating = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Heating);
            }
            else if (offerUpdate is GarageUpdateSaleOfferDTO)
            {
                GarageUpdateSaleOfferDTO offerCreateConverted = (GarageUpdateSaleOfferDTO)offerUpdate;
                GarageSaleOffer garageSaleOffer = (GarageSaleOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, garageSaleOffer);

                garageSaleOffer.Construction = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Construction);
                garageSaleOffer.Location = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Location);
                garageSaleOffer.Lighting = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Lighting);
                garageSaleOffer.Heating = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Heating);
                garageSaleOffer.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfMarket);
            }
            else if (offerUpdate is ApartmentRentUpdateOfferDTO)
            {
                ApartmentRentUpdateOfferDTO offerCreateConverted = (ApartmentRentUpdateOfferDTO)offerUpdate;
                ApartmentRentOffer apartmentRentOffer = (ApartmentRentOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, apartmentRentOffer);

                apartmentRentOffer.ApartmentFinishCondition = offerCreateConverted.ApartmentFinishCondition;
                apartmentRentOffer.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfBuilding);
                apartmentRentOffer.Floor = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Floor);
                apartmentRentOffer.FloorCount = offerCreateConverted.FloorCount;
                apartmentRentOffer.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.BuildingMaterial);
                apartmentRentOffer.WindowsType = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.WindowsType);
                apartmentRentOffer.HeatingType = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.HeatingType);
                apartmentRentOffer.Rent = offerCreateConverted.Rent;
                apartmentRentOffer.AvailableSinceDate = offerCreateConverted.AvailableSinceDate;
                apartmentRentOffer.Internet = offerCreateConverted.Internet;
                apartmentRentOffer.CableTV = offerCreateConverted.CableTV;
                apartmentRentOffer.HomePhone = offerCreateConverted.HomePhone;
                apartmentRentOffer.Balcony = offerCreateConverted.Balcony;
                apartmentRentOffer.UtilityRoom = offerCreateConverted.UtilityRoom;
                apartmentRentOffer.ParkingSpace = offerCreateConverted.ParkingSpace;
                apartmentRentOffer.Basement = offerCreateConverted.Basement;
                apartmentRentOffer.Garden = offerCreateConverted.Garden;
                apartmentRentOffer.Terrace = offerCreateConverted.Terrace;
                apartmentRentOffer.Elevator = offerCreateConverted.Elevator;
                apartmentRentOffer.TwoLevel = offerCreateConverted.TwoLevel;
                apartmentRentOffer.SeparateKitchen = offerCreateConverted.SeparateKitchen;
                apartmentRentOffer.AirConditioning = offerCreateConverted.AirConditioning;
                apartmentRentOffer.AvailableForStudents = offerCreateConverted.AvailableForStudents;
                apartmentRentOffer.OnlyForNonsmoking = offerCreateConverted.OnlyForNonsmoking;
                apartmentRentOffer.AntiBurglaryBlinds = offerCreateConverted.AntiBurglaryBlinds;
                apartmentRentOffer.AntiBurglaryWindowsOrDoors = offerCreateConverted.AntiBurglaryWindowsOrDoors;
                apartmentRentOffer.IntercomOrVideophone = offerCreateConverted.IntercomOrVideophone;
                apartmentRentOffer.MonitoringOrSecurity = offerCreateConverted.MonitoringOrSecurity;
                apartmentRentOffer.AlarmSystem = offerCreateConverted.AlarmSystem;
                apartmentRentOffer.ClosedArea = offerCreateConverted.ClosedArea;
                apartmentRentOffer.Furniture = offerCreateConverted.Furniture;
                apartmentRentOffer.WashingMachine = offerCreateConverted.WashingMachine;
                apartmentRentOffer.Dishwasher = offerCreateConverted.Dishwasher;
                apartmentRentOffer.Fridge = offerCreateConverted.Fridge;
                apartmentRentOffer.Stove = offerCreateConverted.Stove;
                apartmentRentOffer.Oven = offerCreateConverted.Oven;
                apartmentRentOffer.TV = offerCreateConverted.TV;
                apartmentRentOffer.YearOfConstruction = offerCreateConverted.YearOfConstruction;
            }
            else if (offerUpdate is ApartmentSaleUpdateOfferDTO)
            {
                ApartmentSaleUpdateOfferDTO offerCreateConverted = (ApartmentSaleUpdateOfferDTO)offerUpdate;
                ApartmentSaleOffer apartmentSaleOffer = (ApartmentSaleOffer)offerxd;
                MapUpdateDTOToObject(offerCreateConverted, apartmentSaleOffer);

                apartmentSaleOffer.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfBuilding);
                apartmentSaleOffer.Floor = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.Floor);
                apartmentSaleOffer.FloorCount = offerCreateConverted.FloorCount;
                apartmentSaleOffer.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.BuildingMaterial);
                apartmentSaleOffer.WindowsType = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.WindowsType);
                apartmentSaleOffer.HeatingType = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.HeatingType);
                apartmentSaleOffer.Rent = offerCreateConverted.Rent;
                apartmentSaleOffer.FormOfProperty = offerCreateConverted.FormOfProperty;
                apartmentSaleOffer.ApartmentFinishCondition = offerCreateConverted.ApartmentFinishCondition;
                apartmentSaleOffer.AvailableSinceDate = offerCreateConverted.AvailableSinceDate;
                apartmentSaleOffer.Internet = offerCreateConverted.Internet;
                apartmentSaleOffer.CableTV = offerCreateConverted.CableTV;
                apartmentSaleOffer.HomePhone = offerCreateConverted.HomePhone;
                apartmentSaleOffer.Balcony = offerCreateConverted.Balcony;
                apartmentSaleOffer.UtilityRoom = offerCreateConverted.UtilityRoom;
                apartmentSaleOffer.ParkingSpace = offerCreateConverted.ParkingSpace;
                apartmentSaleOffer.Basement = offerCreateConverted.Basement;
                apartmentSaleOffer.Garden = offerCreateConverted.Garden;
                apartmentSaleOffer.Terrace = offerCreateConverted.Terrace;
                apartmentSaleOffer.Elevator = offerCreateConverted.Elevator;
                apartmentSaleOffer.TwoLevel = offerCreateConverted.TwoLevel;
                apartmentSaleOffer.SeparateKitchen = offerCreateConverted.SeparateKitchen;
                apartmentSaleOffer.AirConditioning = offerCreateConverted.AirConditioning;
                apartmentSaleOffer.AntiBurglaryBlinds = offerCreateConverted.AntiBurglaryBlinds;
                apartmentSaleOffer.AntiBurglaryWindowsOrDoors = offerCreateConverted.AntiBurglaryWindowsOrDoors;
                apartmentSaleOffer.IntercomOrVideophone = offerCreateConverted.IntercomOrVideophone;
                apartmentSaleOffer.MonitoringOrSecurity = offerCreateConverted.MonitoringOrSecurity;
                apartmentSaleOffer.AlarmSystem = offerCreateConverted.AlarmSystem;
                apartmentSaleOffer.ClosedArea = offerCreateConverted.ClosedArea;
                apartmentSaleOffer.Furniture = offerCreateConverted.Furniture;
                apartmentSaleOffer.WashingMachine = offerCreateConverted.WashingMachine;
                apartmentSaleOffer.Dishwasher = offerCreateConverted.Dishwasher;
                apartmentSaleOffer.Fridge = offerCreateConverted.Fridge;
                apartmentSaleOffer.Stove = offerCreateConverted.Stove;
                apartmentSaleOffer.Oven = offerCreateConverted.Oven;
                apartmentSaleOffer.TV = offerCreateConverted.TV;
                apartmentSaleOffer.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(offerCreateConverted.TypeOfMarket);
                apartmentSaleOffer.YearOfConstruction = offerCreateConverted.YearOfConstruction;
            }
            else offerxd = null;

            try
            {
                if (offerxd != null)
                {
                    offerxd.LastEditedDate = DateTime.Now;
                    _offerRepository.UpdateAndSaveChanges(offerxd);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errorMessage = ErrorMessageHelper.ErrorUpdatingOffer;
                return false;
            }
            errorMessage = "";
            return true;
        }

        public bool DeleteOffer(int userId, DeleteOfferDTO dto, out string errorMessage)
        {
            
            try
            {
                Offer? offer = _offerRepository.GetById(dto.Id);

                if (offer == null || offer.DeletedDate != null)
                {
                    errorMessage = ErrorMessageHelper.NoOffer;
                    return false;
                }
                if (offer.SellerId != userId)
                {
                    errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer;
                    return false;
                }

                offer.LastEditedDate = dto.LastUpdatedDate;
                offer.DeletedDate = dto.DeletedDate;

                _offerRepository.UpdateAndSaveChanges(offer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errorMessage = ErrorMessageHelper.ErrorDeletingOffer;
                return false;
            }
            errorMessage = "";
            return true;
        }

        public async Task<(bool UploadSuccessful, string errorMessage)> UploadPhotos(List<string>? photos, int UserId, string imgPath, string type)
        {
            //get ID of last offer created by current user
            int lastOfferId = _offerRepository.GetUsersLastOfferId(UserId, type);
            
            imgPath = System.IO.Path.Combine(imgPath, lastOfferId.ToString());

            //if there are no photos to be added
            if (photos == null)
            {
                return (true, "No photos selected!");
            }
            else if (lastOfferId==0)
            {
                return (false, "Error getting last offer from database!");
            }

            //get the total size of all photos
            long size = photos.Sum(f => f.Length);

            //create directory if there is none
            if (!Directory.Exists(imgPath))
            { 
                Directory.CreateDirectory(imgPath); 
            }

            //get list of files in offers directory
            DirectoryInfo d = new(imgPath);
            FileInfo[] JPGFiles = d.GetFiles("*.jpg"); //Getting JPG files
            List<int> fileList = new();

            //Add existing files to list
            foreach (FileInfo file in JPGFiles)
            {
                fileList.Add(int.Parse(file.Name));
            }

            int maxId = 1;

            //list of files in directory sorted by name
            if (fileList.Count>0)
            {
                maxId = fileList.Max(f => f); 
            }

           

            //for every photo in the list
            foreach (var photo in photos)
            {
                //if the file is valid
                if (photo.Length > 0)
                {
                    //set the next name
                    string filenameWithExtension = maxId++.ToString() + ".jpg";
                    File.WriteAllBytes($"{System.IO.Path.Combine(imgPath, filenameWithExtension)}", Convert.FromBase64String(photo));
                }
                else return (false, "Offer created successfully but one of the files was invalid or corrupted");
            }
            return (true, @"Offer created successfully / Files uploaded successfully");
        }

        #region UserFavourites

        public bool CheckIfFavourite(int userId, int offerId)
        {
            var userFavs = _userFavouriteRepository.GetUserFavourites(userId);
            userFavs = userFavs.Where(uf => !uf.Offer.DeletedDate.HasValue && uf.OfferId == offerId);
            if(userFavs.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserFavouriteListing GetUserFavourites(Paging paging, int userId, string imgFolderPath)
        {
            string offerPath = Path.Combine(imgFolderPath, "Offers");
            string agencyPath = Path.Combine(imgFolderPath, "AgencyLogo");
            var userFavs = _userFavouriteRepository.GetUserFavourites(userId);
            userFavs = userFavs.Where(uf => !uf.Offer.DeletedDate.HasValue);

            UserFavouriteListing userFavouriteListing = new()
            {
                TotalCount = userFavs.Count(),
                Paging = paging,

                FavouritesDTOs = userFavs.Select(x => new OfferListThumbnailDTO
                {
                    Id = x.Offer.Id,
                    OfferTitle = x.Offer.OfferTitle,
                    City = x.Offer.City,
                    Price = x.Offer.Price,
                    Voivodeship = x.Offer.Voivodeship,
                    PriceForOneSquareMeter = (x.Offer.Area == 0 || x.Offer.Area == null) ? (0) : (int)(x.Offer.Price / x.Offer.Area),
                    RoomCount = x.Offer.RoomCount,
                    Area = x.Offer.Area,
                    AgencyId = x.Offer.Seller.AgentInAgencyId == null ?(x.Offer.Seller.OwnerOfAgencyId==null?null: x.Offer.Seller.OwnerOfAgencyId) : x.Offer.Seller.AgentInAgencyId,
                    AgencyName = (x.Offer.Seller.OwnerOfAgencyId != null) ? _agencyRepository.GetById(x.Offer.Seller.OwnerOfAgencyId.Value).AgencyName : (x.Offer.Seller.AgentInAgencyId != null ? _agencyRepository.GetById(x.Offer.Seller.AgentInAgencyId.Value).AgencyName : null),
                    AgencyLogo = (x.Offer.Seller.OwnerOfAgencyId != null) ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyPath, x.Offer.Seller.OwnerOfAgencyId.ToString())).Result : (x.Offer.Seller.AgentInAgencyId != null ? _offerRepository.GetPhoto(System.IO.Path.Combine(agencyPath, x.Offer.Seller.AgentInAgencyId.ToString())).Result : null),
                    SellerType = x.Offer.SellerType,
                    OfferType = x.Offer.OfferType,
                    EstateType = x.Offer.EstateType,
                    Photo = _offerRepository.GetPhoto(System.IO.Path.Combine(offerPath, x.Offer.Id.ToString())).Result,
                }).ToPagedList(paging.PageNumber, paging.PageSize)
            };

            return userFavouriteListing;
        }

        public bool AddToFavourites(int offerId, int userId, out string errorMessage)
        {
            UserFavourite result = new()
            {
                OfferId = offerId,
                UserId = userId,
                LikeDate = DateTime.Now,
            };

            try
            {
                if (result != null)
                {
                    _userFavouriteRepository.AddAndSaveChanges(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                errorMessage = ErrorMessageHelper.ErrorAddingToFavourites;
                return false;
            }
            errorMessage = "";
            return true;
        }

        public bool RemoveFromFavourites(int offerId, int userId, out string errorMessage)
        {
            UserFavourite? userFav = _userFavouriteRepository.GetFav(offerId, userId);
            bool result=false;

            if (userFav != null)
            {
                try
                {
                     result = _userFavouriteRepository.RemoveFavByIdAndSaveChanges(offerId, userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    errorMessage = ErrorMessageHelper.ErrorRemovingFromFavourites;
                    return false;
                }
                errorMessage = "";
                return result;
            }
            else
            {
                errorMessage = "";
                return false;
            }
        }

        #endregion

        public byte[]? GenerateContract(string pdfFolderPath, int userId, int offerId, string contractType, out string _errorMessage, out string fileName)
        {
            fileName = "";
            try
            {
                Offer? offer = _offerRepository.GetById(offerId);
                if(offer == null)
                {
                    _errorMessage = ErrorMessageHelper.NoOffer;
                    fileName = "Error.pdf";
                    return null;
                }

                User? user = _userRepository.GetById(userId);
                if (user == null)
                {
                    _errorMessage = ErrorMessageHelper.NoUser;
                    fileName = "Error.pdf";
                    return null;
                }

                if(offer.SellerId != user.Id)
                {
                    _errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer;
                    fileName = "Error.pdf";
                    return null;
                }

                if (contractType == "APARTMENT_SALE_WITH_DEPOSIT_CONTRACT") // z zadatkiem
                {
                    ApartmentSaleOffer? asOffer = (ApartmentSaleOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Przedwstepna_umowa_sprzedazy_mieszkania_z_zadatkiem.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());

                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);

                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("MieszkanieNr", out toset);
                            toset.SetValue(Nr!=null?Nr:"");
                            fields.TryGetValue("MieszkanieCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("MieszkanieStreet", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("Pi?tro", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor : "");
                            fields.TryGetValue("RoomCount", out toset);
                            toset.SetValue(asOffer.RoomCount!=null?asOffer.RoomCount.ToString():"");
                            fields.TryGetValue("AreaS?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("Area", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.ToString():"");
                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.APARTMENT_SALE_WITH_DEPOSIT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }

                }
                else if (contractType == "APARTMENT_SALE_WITH_ADVANCE_CONTRACT") // z zaliczką
                {
                    ApartmentSaleOffer? asOffer = (ApartmentSaleOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    if(asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Przedwstepna_umowa_sprzedazy_mieszkania_-z_zaliczka.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());

                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("LokalNr", out toset);
                            toset.SetValue(Nr!=null?Nr:"");
                            fields.TryGetValue("LokalCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("LokalStreet", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("Pi?tro", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor.ToString() : "");
                            fields.TryGetValue("RoomCount", out toset);
                            toset.SetValue(asOffer.RoomCount.ToString());
                            fields.TryGetValue("AreaS?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ?  NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("Area", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.ToString():"");
                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price)); 

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.APARTMENT_SALE_WITH_ADVANCE_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "APARTMENT_SALE_WITH_COOPERATIVE_OWNERSHIP_RIGHT_CONTRACT")
                {
                    ApartmentSaleOffer? asOffer = (ApartmentSaleOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Przedwstepna_umowa_sprzedazy_spoldzielczego_wlasnosciowego_prawa_do_lokalu.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());

                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("NumerLokalu", out toset);
                            toset.SetValue(Nr != null ? Nr : "");
                            fields.TryGetValue("LokalCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("LokalStreet", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("Pi?tro", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor.ToString() : "");
                            fields.TryGetValue("RoomCount", out toset);
                            toset.SetValue(asOffer.RoomCount.ToString());
                            fields.TryGetValue("PowierzchniaS?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("Powierzchnia", out toset);
                            toset.SetValue(asOffer.Area.ToString());
                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.APARTMENT_SALE_WITH_COOPERATIVE_OWNERSHIP_RIGHT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "APARTMENT_RENT_FOR_INDEFINITE_PERIOD")
                {
                    ApartmentRentOffer? asOffer = (ApartmentRentOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Umowa_najmu_mieszkania_na_czas_nieokreslony.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("zawartawdniu", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());

                            fields.TryGetValue("wynajmuj?cyname", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("wynajmuj?cycity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("wynajmuj?cykod", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("wynajmuj?cyulica", out toset);
                            toset.SetValue(asOffer.Seller.Street!=null? asOffer.Seller.Street:"");

                            fields.TryGetValue("lokalnr", out toset);
                            toset.SetValue(Nr != null ? Nr : "");
                            fields.TryGetValue("po?o?onymw", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("budynkupo?o?onymw", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("po?o?onymprzyulicy", out toset);

                            toset.SetValue(Street != null ? Street : "");
                            fields.TryGetValue("przyulicy", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("dolokalumieszkalnegonr", out toset);
                            toset.SetValue(Nr!=null?Nr:"");
                            fields.TryGetValue("znajdujesi?na", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor.ToString() : "");
                            fields.TryGetValue("sk?adasi?z", out toset);
                            toset.SetValue(asOffer.RoomCount != null ? asOffer.RoomCount.ToString() : "");
                            fields.TryGetValue("powierzchniawynosi", out toset);
                            toset.SetValue(asOffer.Area!=null?asOffer.Area.ToString():"");
                            fields.TryGetValue("czynszwkwocie", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("czynszs?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            fields.TryGetValue("kaucj?wkwocie", out toset);
                            toset.SetValue(asOffer.Rent != null ? asOffer.Rent.ToString() : "");
                            fields.TryGetValue("kaucj?s?ownie", out toset);
                            toset.SetValue(asOffer.Rent != null ? NumberToText.Convert(asOffer.Rent.Value) : "");
                            fields.TryGetValue("wynajmuj?cyemail", out toset);
                            toset.SetValue(asOffer.Seller.Email);
                            

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.APARTMENT_RENT_FOR_INDEFINITE_PERIOD) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                    
                }
                else if (contractType == "PREMISES_SALE_CONTRACT")
                {
                    BusinessPremisesSaleOffer? asOffer = (BusinessPremisesSaleOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Przedwstepna_umowa_sprzedazy_lokalu_uzytkowego.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("LokalCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("LokalStreet", out toset);
                            toset.SetValue(asOffer.Address);
                            fields.TryGetValue("IlośćPomieszczeń", out toset);
                            toset.SetValue(asOffer.RoomCount != null ? asOffer.RoomCount.ToString() : "");
                            fields.TryGetValue("PowierzchniaPomieszczeńS?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("PowierzchniaPomieszczeń", out toset);
                            toset.SetValue(asOffer.Area!=null?asOffer.Area.ToString():"");
                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.PREMISES_SALE_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "PREMISES_RENT_CONTRACT")
                {
                    BusinessPremisesRentOffer? asOffer = (BusinessPremisesRentOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Umowa_najmu_lokalu_uzytkowego.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null? asOffer.Seller.City:"");

                            fields.TryGetValue("LokalArea", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.Value.ToString() : "");
                            fields.TryGetValue("LokalAreaS?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("LokalCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("LokalStreet", out toset);
                            toset.SetValue(asOffer.Address);
                            fields.TryGetValue("RoomCount", out toset);
                            toset.SetValue(asOffer.RoomCount!=null?asOffer.RoomCount.ToString():"");
                            fields.TryGetValue("Floor", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor.ToString() : "");
                            
                            fields.TryGetValue("Czynsz", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CzynszS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            fields.TryGetValue("WynMail", out toset);
                            toset.SetValue(asOffer.Seller.Email);
                            
                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.PREMISES_RENT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "GARAGE_SALE_CONTRACT")
                {
                    GarageSaleOffer? asOffer = (GarageSaleOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "UMOWA SPRZEDAZY GARAZU.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerAddress", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode!=null?asOffer.Seller.PostalCode:"" + " " + asOffer.Seller.City!=null?asOffer.Seller.City:"" + " " + asOffer.Seller.Street!=null?asOffer.Seller.Street:"");

                            fields.TryGetValue("GarageMaterial", out toset);
                            toset.SetValue(asOffer.Construction != null ? asOffer.Construction: "");
                            fields.TryGetValue("GarageArea", out toset);
                            toset.SetValue(asOffer.Area!=null?asOffer.Area.ToString():"");

                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.GARAGE_SALE_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "GARAGE_RENT_CONTRACT")
                {
                    GarageRentOffer? asOffer = (GarageRentOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Umowa najmu miejsca garazowego.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));


                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("Wynajmuj?cy", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            

                            fields.TryGetValue("GarageCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("GarageStreet", out toset);
                            toset.SetValue(Street != null ?Street:"");
                            fields.TryGetValue("GarageLocation", out toset);
                            toset.SetValue(asOffer.Location != null ? asOffer.Location : "");
                            fields.TryGetValue("Czynsz", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CzynszS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.GARAGE_RENT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();

                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "HOUSE_SALE_CONTRACT")
                {
                    HouseSaleOffer? asOffer = (HouseSaleOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Przedwstepna_umowa_sprzedazy_domu.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);

                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("HouseCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("HouseStreet", out toset);
                            toset.SetValue(Street != null ? Street: "");
                            fields.TryGetValue("HouseNumber", out toset);
                            toset.SetValue(Nr!=null?Nr:"");
                            fields.TryGetValue("HouseArea", out toset); //Działka
                            toset.SetValue(asOffer.LandArea != null ? asOffer.LandArea.Value.ToString() : "");
                            fields.TryGetValue("HouseAreaS?ownie", out toset);
                            toset.SetValue(asOffer.LandArea != null ? NumberToText.Convert(asOffer.LandArea.Value) : "");
                            fields.TryGetValue("HouseArea2", out toset); //Dom
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.Value.ToString() : "");
                            fields.TryGetValue("HouseArea2S?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.HOUSE_SALE_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();

                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "HOUSE_RENT_CONTRACT")
                {
                    HouseRentOffer? asOffer = (HouseRentOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Umowa_najmu_domu_jednorodzinnego_lokatorem.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("umowazawartawdniu", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("wynajmuj?cyname", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("wynajmuj?cycity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("wynajmuj?cykod", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("wynajmuj?cyulica", out toset);
                            toset.SetValue(asOffer.Seller.Street!=null?asOffer.Seller.Street:"");
                            fields.TryGetValue("wynajmuj?cyemail", out toset);
                            toset.SetValue(asOffer.Seller.Email);
                            fields.TryGetValue("nieruchomościpo?ożonejw", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("przyulicy", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("nakwot?", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("nakwot?s?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            fields.TryGetValue("opowierzchni", out toset);
                            toset.SetValue(asOffer.LandArea != null ? asOffer.LandArea.Value.ToString() : "");
                            fields.TryGetValue("opowierzchnis?ownie", out toset);
                            toset.SetValue(asOffer.LandArea != null ? NumberToText.Convert(asOffer.LandArea.Value) : "");
                            fields.TryGetValue("opowierzchniużytkowej", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.Value.ToString() : "");
                            fields.TryGetValue("opowierzchniużytkowejs?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.HOUSE_RENT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();

                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "PLOT_SALE_CONTRACT")
                {
                    PlotOffer? asOffer = (PlotOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.PLOT));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Przedwstepna_umowa_sprzedazy_gruntu.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("PlotCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("PlotStreet", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("PlotNumber", out toset);
                            toset.SetValue(Nr!=null?Nr:"");
                            fields.TryGetValue("PlotArea", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.Value.ToString() : "");
                            fields.TryGetValue("Cena", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CenaS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));


                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.PLOT_SALE_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();

                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "PLOT_RENT_CONTRACT")
                {
                    //Umowa_dzierzawy_gruntu
                    PlotOffer? asOffer = (PlotOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.PLOT));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Umowa_dzierzawy_gruntu.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("Day", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("SellerName", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("SellerCity", out toset);
                            toset.SetValue(asOffer.Seller.City != null ? asOffer.Seller.City : "");
                            fields.TryGetValue("SellerPostalCode", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("SellerStreet", out toset);
                            toset.SetValue(asOffer.Seller.Street != null ? asOffer.Seller.Street : "");
                            fields.TryGetValue("PlotCity", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("PlotWoj", out toset);
                            toset.SetValue(asOffer.Voivodeship);
                            fields.TryGetValue("PlotArea", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.Value.ToString() : "");
                            fields.TryGetValue("PlotAreaS?ownie", out toset);
                            toset.SetValue(asOffer.Area != null ? NumberToText.Convert(asOffer.Area.Value) : "");
                            fields.TryGetValue("Czynsz", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("CzynszS?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));
                            fields.TryGetValue("MailWydz", out toset);
                            toset.SetValue(asOffer.Seller.Email);

                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.PLOT_RENT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();

                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "ROOM_RENT_CONTRACT")
                {
                    RoomRentingOffer? asOffer = (RoomRentingOffer?)_offerRepository.GetOffer(offerId, EnumHelper.GetDescriptionFromEnum(EstateType.ROOM));
                    if (asOffer == null)
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        fileName = "";
                        return null;
                    }
                    string doc = Path.Combine(pdfFolderPath, "Umowa_najmu_lokalu_mieszkalnego_z_podzialem_na_pokoje.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();

                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                        if (form != null)
                        {
                            IDictionary<string, PdfFormField> fields = form.GetFormFields();
                            PdfFormField? toset;

                            var parts = asOffer.Address != null ? asOffer.Address.Split(' ') : new string[5] { "", "", "", "", "", };
                            var Nr = parts.LastOrDefault();
                            int BuildingNr;
                            int ApNr=0;
                            
                            var Street = string.Join(" ", parts.Take(parts.Length - 1));

                            fields.TryGetValue("zawartawdniu", out toset);
                            toset.SetValue(DateTime.Now.ToShortDateString());
                            fields.TryGetValue("wynajmujacyname", out toset);
                            toset.SetValue(asOffer.Seller.FirstName + " " + asOffer.Seller.LastName);
                            fields.TryGetValue("wynajmujacycity", out toset);
                            toset.SetValue(asOffer.Seller.City!=null?asOffer.Seller.City:"");
                            fields.TryGetValue("wynajmujacykod", out toset);
                            toset.SetValue(asOffer.Seller.PostalCode != null ? asOffer.Seller.PostalCode : "");
                            fields.TryGetValue("wynajmujacyulica", out toset);
                            toset.SetValue(asOffer.Seller.Street !=null ? asOffer.Seller.Street:"");
                            fields.TryGetValue("nrlokalu", out toset);
                            toset.SetValue(Nr!=null?Nr:"");
                            fields.TryGetValue("budynekpolozonyw", out toset);
                            toset.SetValue(asOffer.City);
                            fields.TryGetValue("przyulicy", out toset);
                            toset.SetValue(Street!=null?Street:"");
                            fields.TryGetValue("usytuowanyjestna", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor : "");
                            fields.TryGetValue("kondygnacji", out toset);
                            toset.SetValue(asOffer.Floor != null ? asOffer.Floor : "");
                            fields.TryGetValue("wynajmuj?cyemail", out toset);
                            toset.SetValue(asOffer.Seller.Email);
                            fields.TryGetValue("opowierzchni", out toset);
                            toset.SetValue(asOffer.Area != null ? asOffer.Area.Value.ToString(): "");
                            fields.TryGetValue("kaucj?wkwocie", out toset);
                            toset.SetValue(asOffer.Deposit != null ? asOffer.Deposit.Value.ToString() : "");
                            fields.TryGetValue("kaucj?wkwocies?ownie", out toset);
                            toset.SetValue(asOffer.Deposit != null ? NumberToText.Convert(asOffer.Deposit.Value) : "");

                            fields.TryGetValue("czynszuwkwocie", out toset);
                            toset.SetValue(asOffer.Price.ToString());
                            fields.TryGetValue("s?ownie", out toset);
                            toset.SetValue(NumberToText.Convert(asOffer.Price));

                            fields.TryGetValue("wynajmuj?cyemail", out toset);
                            toset.SetValue(asOffer.Seller.Email);


                            pdf.Close();
                            _errorMessage = "";
                            fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.ROOM_RENT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                            return outputStream.ToArray();
                        }
                        else
                        {
                            _errorMessage = "Nie ma formularza!";
                            fileName = "";
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "DELIVERY_AND_ACCEPTANCE_REPORT_FOR_SALE")
                {
                    string doc = Path.Combine(pdfFolderPath, "Protokol_zdawczo-odbiorczy sprzedazy.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();
                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));

                        pdf.Close();
                        _errorMessage = "";
                        fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.DELIVERY_AND_ACCEPTANCE_REPORT_FOR_SALE) + ".pdf";
                        return outputStream.ToArray();
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "DELIVERY_AND_ACCEPTANCE_REPORT_FOR_RENT")
                {
                    string doc = Path.Combine(pdfFolderPath, "Protokol_zdawczo_odbiorczy najmu.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();
                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));

                        pdf.Close();
                        _errorMessage = "";
                        fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.DELIVERY_AND_ACCEPTANCE_REPORT_FOR_RENT) + ".pdf";
                        return outputStream.ToArray();
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "ESTATE_SALE_MEDIATION_CONTRACT")
                {
                    string doc = Path.Combine(pdfFolderPath, "umowa-posrednictwa-na-sprzedaz-nieruchomosci.pdf");

                    if (File.Exists(doc))
                    {
                        var SourceFileStream = File.OpenRead(doc);
                        var outputStream = new MemoryStream();
                        var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));

                        pdf.Close();
                        _errorMessage = "";
                        fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.ESTATE_SALE_MEDIATION_CONTRACT) + ".pdf";
                        return outputStream.ToArray();
                    }
                    else
                    {
                        _errorMessage = "Plik nie istnieje!";
                        fileName = "";
                        return null;
                    }
                }
                else if (contractType == "HALL_SALE_CONTRACT")
                {
                    fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.HALL_SALE_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                }
                else if (contractType == "HALL_RENT_CONTRACT")
                {
                    fileName = EnumHelper.GetDescriptionFromEnum(ContractTypes.HALL_RENT_CONTRACT) + "_" + offerId.ToString() + ".pdf";
                }
                else
                {
                    _errorMessage = ErrorMessageHelper.UnsupportedContractType;
                    fileName = "UnsupportedContract";
                    return null;
                }
                _errorMessage = "";
                fileName = fileName.Replace(' ', '_');
                return null; // tutaj zmienić
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorGeneratingContract;
                fileName = "Error.pdf";
                return null;
            }
        }

        public static void MapUpdateDTOToObject(OfferUpdateDTO offerCreateConverted, Offer offerxd)
        {
            offerxd.OfferTitle = offerCreateConverted.OfferTitle;
            offerxd.Price = offerCreateConverted.Price;
            offerxd.RoomCount = offerCreateConverted.RoomCount;
            offerxd.Area = offerCreateConverted.Area;
            offerxd.Voivodeship = offerCreateConverted.Voivodeship;
            offerxd.City = offerCreateConverted.City;
            offerxd.Address = offerCreateConverted.Address;
            offerxd.Description = offerCreateConverted.Description;
            offerxd.RemoteControl = offerCreateConverted.RemoteControl;
            offerxd.OfferStatus = offerCreateConverted.OfferStatus;
            offerxd.LastEditedDate = DateTime.Now;
        }
    }
}
