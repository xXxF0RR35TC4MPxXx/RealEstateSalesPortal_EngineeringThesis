using Inżynierka.Shared.ViewModels;
using Inżynierka_Common.Helpers;
using Inżynierka.Shared.DTOs.Offers;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using System.Text.Json;
using Inżynierka.Shared.ViewModels.Offer.Create;
using Inżynierka_Common.Enums;
using Inżynierka.Shared.DTOs.Offers.Create;
using AutoMapper;
using Inżynierka.Server.Attributes;
using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.DTOs.Offers.Update;
using Inżynierka.Shared.ViewModels.Offer.Update;
using Inżynierka_Common.Listing;
using Inżynierka_Services.Listing;
using Inżynierka.Shared.ViewModels.UserFavourites;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.ViewModels.Offer.Filtering;
using Inżynierka.Shared.Entities;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.IO;
using Swashbuckle.AspNetCore.Annotations;
using Inżynierka.Shared.Entities.OfferTypes.Apartment;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.Entities.OfferTypes.Hall;
using Inżynierka.Shared.Entities.OfferTypes.Garage;
using Inżynierka.Shared.Entities.OfferTypes.BusinessPremises;
using Inżynierka.Shared.Entities.OfferTypes.Plot;
using Inżynierka.Shared.Entities.OfferTypes.Room;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("OfferController")]
    public class OfferController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserActionService _userActionService;
        private readonly OfferService _offerService;
        private string? _errorMessage;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        protected CultureInfo culture;


        public OfferController(ILogger<HomeController> logger, UserActionService userActionService, IMapper map, OfferService offerService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _userActionService = userActionService;
            _offerService = offerService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = map;
            culture = new CultureInfo("en-GB");
        }


        protected void LogUserAction(string controller, string controllerAction, string actionParameters, UserActionService userActionService)
        {
            int userId = GetUserId();
            UserAction userAction = new()
            {
                UserId = userId,
                Controller = controller,
                ControllerAction = controllerAction,
                ActionParameters = actionParameters,
                Date = DateTime.Now
            };

            userActionService.CreateUserAction(userAction);
        }

        protected int GetUserId()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            Claim? idClaim = claims.FirstOrDefault(e => e.Type == "Id");

            int id;
            if (idClaim == null)
            {
                id = 0;
            }
            else
            {
                int.TryParse(idClaim.Value, out id);
            }

            return id;
        }

        protected string GetUserRole()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            Claim? roleClaim = claims.FirstOrDefault(e => e.Type == "RoleName");
            string role;
            if (roleClaim == null)
            {
                role = UserRoles.ANONYMOUS.ToString();
            }
            else
            {
                role = roleClaim.Value;
            }

            return role;
        }

        protected string Translate(string message)
        {
            if (HttpContext.Session.GetString("Language") == null || HttpContext.Session.GetString("Language") == "en-GB")
            {
                return message;
            }

            string language = HttpContext.Session.GetString("Language");
            CultureInfo culture = new CultureInfo(language);

            //string result = _resourceManager.GetString(message, culture);

            return message;
        }

        /// <summary>
        /// Gets an offer specified by given ID
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>JSON string representing an object of the Offer class</returns>
        /// <response code="200">Object of offer</response>
        /// <response code="400">string "No with this OfferId"</response>
        [HttpGet("Get/{offerId}")]
        public IActionResult Get(int offerId)
        {
            LogUserAction("OfferController", "Get", offerId.ToString(), _userActionService);
            ReadOfferDTO? offer = _offerService.Get(offerId, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), out _errorMessage);
            if(offer == null) { return BadRequest(new ResponseViewModel("No offer")); }
            return Ok(offer);
        }

        /// <summary>
        /// Gets the owner id of offer specified by given ID
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>JSON string representing an object of the Offer class</returns>
        /// <response code="200">Object of offer</response>
        /// <response code="400">string "No with this OfferId"</response>
        [HttpGet("GetOwner/{offerId}")]
        public int? GetOwner(int offerId)
        {
            LogUserAction("OfferController", "GetOwner", offerId.ToString(), _userActionService);
            ReadOfferDTO? offer = _offerService.Get(offerId, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), out _errorMessage);
            if (offer == null) return null;
            return offer.SellerId;
        }

        /// <summary>
        /// Gets an update view model of offer specified by given ID
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>JSON string representing an object of the OfferUpdateViewModel class</returns>
        /// <response code="200">Object of OfferUpdateViewModel</response>
        /// <response code="400">null</response>
        [HttpGet("GetUpdateViewModel/{offerId}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        public OfferUpdateViewModel? GetUpdateViewModel(int offerId)
        {
            LogUserAction("OfferController", "GetUpdateViewModel", offerId.ToString(), _userActionService);
            Offer? offer = _offerService.GetOfferForUpdate(offerId);
            if (offer is ApartmentRentOffer)
            {
                ApartmentRentOffer? offerParsed = (ApartmentRentOffer)offer;
                ApartmentRentOfferUpdateViewModel result = _mapper.Map<ApartmentRentOfferUpdateViewModel>(offerParsed);
                result.TypeOfBuilding = EnumHelper.GetEnumFromDescription<TypeOfBuilding>(offerParsed.TypeOfBuilding);
                result.Floor = EnumHelper.GetEnumFromDescription<Floor>(offerParsed.Floor);
                result.BuildingMaterial = EnumHelper.GetEnumFromDescription<BuildingMaterial>(offerParsed.BuildingMaterial);
                result.WindowsType = EnumHelper.GetEnumFromDescription<WindowType>(offerParsed.WindowsType);
                result.HeatingType = EnumHelper.GetEnumFromDescription<ApartmentHeating>(offerParsed.HeatingType);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<ApartmentFinishCondition>(offerParsed.ApartmentFinishCondition);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is ApartmentSaleOffer)
            {
                ApartmentSaleOffer? offerParsed = (ApartmentSaleOffer)offer;
                ApartmentSaleOfferUpdateViewModel result = _mapper.Map<ApartmentSaleOfferUpdateViewModel>(offerParsed);
                result.TypeOfBuilding = EnumHelper.GetEnumFromDescription<TypeOfBuilding>(offerParsed.TypeOfBuilding);
                result.Floor = EnumHelper.GetEnumFromDescription<Floor>(offerParsed.Floor);
                result.BuildingMaterial = EnumHelper.GetEnumFromDescription<BuildingMaterial>(offerParsed.BuildingMaterial);
                result.WindowsType = EnumHelper.GetEnumFromDescription<WindowType>(offerParsed.WindowsType);
                result.HeatingType = EnumHelper.GetEnumFromDescription<ApartmentHeating>(offerParsed.HeatingType);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<ApartmentFinishCondition>(offerParsed.ApartmentFinishCondition);
                result.TypeOfMarket = EnumHelper.GetEnumFromDescription<TypeOfMarket>(offerParsed.TypeOfMarket);
                result.FormOfProperty = EnumHelper.GetEnumFromDescription<FormOfProperty>(offerParsed.FormOfProperty);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is HouseRentOffer)
            {
                HouseRentOffer? offerParsed = (HouseRentOffer)offer;
                HouseRentOfferUpdateViewModel result = _mapper.Map<HouseRentOfferUpdateViewModel>(offerParsed);
                result.TypeOfBuilding = EnumHelper.GetEnumFromDescription<TypeOfBuilding>(offerParsed.TypeOfBuilding);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.BuildingMaterial = EnumHelper.GetEnumFromDescription<BuildingMaterial>(offerParsed.BuildingMaterial);
                result.AtticType = EnumHelper.GetEnumFromDescription<AtticType>(offerParsed.AtticType);
                result.RoofType = EnumHelper.GetEnumFromDescription<RoofType>(offerParsed.RoofType);
                result.RoofingType = EnumHelper.GetEnumFromDescription<RoofingType>(offerParsed.RoofingType);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<HouseFinishCondition>(offerParsed.FinishCondition);
                result.WindowsType = EnumHelper.GetEnumFromDescription<WindowType>(offerParsed.WindowsType);
                result.Location = EnumHelper.GetEnumFromDescription<Location>(offerParsed.Location);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is HouseSaleOffer)
            {
                HouseSaleOffer? offerParsed = (HouseSaleOffer)offer;
                HouseSaleOfferUpdateViewModel result = _mapper.Map<HouseSaleOfferUpdateViewModel>(offerParsed);
                result.TypeOfBuilding = EnumHelper.GetEnumFromDescription<TypeOfBuilding>(offerParsed.TypeOfBuilding);
                result.BuildingMaterial = EnumHelper.GetEnumFromDescription<BuildingMaterial>(offerParsed.BuildingMaterial);
                result.AtticType = EnumHelper.GetEnumFromDescription<AtticType>(offerParsed.AtticType);
                result.RoofType = EnumHelper.GetEnumFromDescription<RoofType>(offerParsed.RoofType);
                result.RoofingType = EnumHelper.GetEnumFromDescription<RoofingType>(offerParsed.RoofingType);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<HouseFinishCondition>(offerParsed.FinishCondition);
                result.WindowsType = EnumHelper.GetEnumFromDescription<WindowType>(offerParsed.WindowsType);
                result.Location = EnumHelper.GetEnumFromDescription<Location>(offerParsed.Location);
                result.TypeOfMarket = EnumHelper.GetEnumFromDescription<TypeOfMarket>(offerParsed.TypeOfMarket);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is HallRentOffer)
            {
                HallRentOffer? offerParsed = (HallRentOffer)offer;
                HallRentOfferUpdateViewModel result = _mapper.Map<HallRentOfferUpdateViewModel>(offerParsed);
                result.Construction = EnumHelper.GetEnumFromDescription<HallConstruction>(offerParsed.Construction);
                result.ParkingSpace = EnumHelper.GetEnumFromDescription<ParkingType>(offerParsed.ParkingSpace);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<HallFinishCondition>(offerParsed.FinishCondition);
                result.Flooring = EnumHelper.GetEnumFromDescription<Flooring>(offerParsed.Flooring);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is HallSaleOffer)
            {
                HallSaleOffer? offerParsed = (HallSaleOffer)offer;
                HallSaleOfferUpdateViewModel result = _mapper.Map<HallSaleOfferUpdateViewModel>(offerParsed);
                result.Construction = EnumHelper.GetEnumFromDescription<HallConstruction>(offerParsed.Construction);
                result.ParkingSpace = EnumHelper.GetEnumFromDescription<ParkingType>(offerParsed.ParkingSpace);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<HallFinishCondition>(offerParsed.FinishCondition);
                result.Flooring = EnumHelper.GetEnumFromDescription<Flooring>(offerParsed.Flooring);
                result.TypeOfMarket = EnumHelper.GetEnumFromDescription<TypeOfMarket>(offerParsed.TypeOfMarket);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is GarageRentOffer)
            {
                GarageRentOffer? offerParsed = (GarageRentOffer)offer;
                GarageRentOfferUpdateViewModel result = _mapper.Map<GarageRentOfferUpdateViewModel>(offerParsed);
                result.Construction = EnumHelper.GetEnumFromDescription<GarageConstruction>(offerParsed.Construction);
                result.Location = EnumHelper.GetEnumFromDescription<GarageLocation>(offerParsed.Location);
                result.Lighting = EnumHelper.GetEnumFromDescription<GarageLighting>(offerParsed.Lighting);
                result.Heating = EnumHelper.GetEnumFromDescription<GarageHeating>(offerParsed.Heating);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is GarageSaleOffer)
            {
                GarageSaleOffer? offerParsed = (GarageSaleOffer)offer;
                GarageSaleOfferUpdateViewModel result = _mapper.Map<GarageSaleOfferUpdateViewModel>(offerParsed);
                result.Construction = EnumHelper.GetEnumFromDescription<GarageConstruction>(offerParsed.Construction);
                result.Location = EnumHelper.GetEnumFromDescription<GarageLocation>(offerParsed.Location);
                result.Lighting = EnumHelper.GetEnumFromDescription<GarageLighting>(offerParsed.Lighting);
                result.Heating = EnumHelper.GetEnumFromDescription<GarageHeating>(offerParsed.Heating);
                result.TypeOfMarket = EnumHelper.GetEnumFromDescription<TypeOfMarket>(offerParsed.TypeOfMarket);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is BusinessPremisesRentOffer)
            {
                BusinessPremisesRentOffer? offerParsed = (BusinessPremisesRentOffer)offer;
                BusinessPremisesRentOfferUpdateViewModel result = _mapper.Map<BusinessPremisesRentOfferUpdateViewModel>(offerParsed);
                result.Location = EnumHelper.GetEnumFromDescription<PremisesLocation>(offerParsed.Location);
                result.Floor = EnumHelper.GetEnumFromDescription<Floor>(offerParsed.Floor);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<PremisesFinishCondition>(offerParsed.FinishCondition);
                return result;
            }
            else if (offer is BusinessPremisesSaleOffer)
            {
                BusinessPremisesSaleOffer? offerParsed = (BusinessPremisesSaleOffer)offer;
                BusinessPremisesSaleOfferUpdateViewModel result = _mapper.Map<BusinessPremisesSaleOfferUpdateViewModel>(offerParsed);
                result.Location = EnumHelper.GetEnumFromDescription<PremisesLocation>(offerParsed.Location);
                result.Floor = EnumHelper.GetEnumFromDescription<Floor>(offerParsed.Floor);
                result.FinishCondition = EnumHelper.GetEnumFromDescription<PremisesFinishCondition>(offerParsed.FinishCondition);
                result.TypeOfMarket = EnumHelper.GetEnumFromDescription<TypeOfMarket>(offerParsed.TypeOfMarket);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is PlotOffer)
            {
                PlotOffer? offerParsed = (PlotOffer)offer;
                PlotOfferUpdateViewModel result = _mapper.Map<PlotOfferUpdateViewModel>(offerParsed);
                result.PlotType = EnumHelper.GetEnumFromDescription<PlotType>(offerParsed.PlotType);
                result.Location = EnumHelper.GetEnumFromDescription<Location>(offerParsed.Location);
                result.OfferType = EnumHelper.GetEnumFromDescription<OfferType>(offerParsed.OfferType);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else if (offer is RoomRentingOffer)
            {
                RoomRentingOffer? offerParsed = (RoomRentingOffer)offer;
                RoomRentingOfferUpdateViewModel result = _mapper.Map<RoomRentingOfferUpdateViewModel>(offerParsed);
                result.Floor = EnumHelper.GetEnumFromDescription<Floor>(offerParsed.Floor);
                result.TypeOfBuilding = EnumHelper.GetEnumFromDescription<TypeOfBuilding>(offerParsed.TypeOfBuilding);
                result.OfferStatus = EnumHelper.GetEnumFromDescription<OfferStatus>(offerParsed.OfferStatus);
                result.Voivodeship = EnumHelper.GetEnumFromDescription<Voivodeships>(offerParsed.Voivodeship);
                return result;
            }
            else { return null; }
        }

        /// <summary>
        /// Checks if offer is in users favourite list
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>JSON string representing an array of objects of the Offer class</returns>
        /// <response code="200">List of offers</response>
        /// <response code="400"></response>
        [HttpGet("CheckIfFavourite/{userId}/{offerId}")]
        public bool CheckIfFavourite(int userId, int offerId)
        {
            LogUserAction("OfferController", "CheckIfFavourite", userId.ToString() + " / " + offerId.ToString(), _userActionService);
            bool result = _offerService.CheckIfFavourite(userId, offerId);

            return result;
        }


        /// <summary>
        /// Gets offers similar to offer with given Id
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>JSON string representing an array of objects of the Offer class</returns>
        /// <response code="200">List of offers</response>
        /// <response code="400"></response>
        [HttpGet("GetSimilar/{offerId}")]
        public IEnumerable<HomepageOffersDTO> GetSimilar(int offerId)
        {
            LogUserAction("OfferController", "GetSimilar", offerId.ToString(), _userActionService);
            IEnumerable<HomepageOffersDTO>? offers = _offerService.GetSimilar(offerId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"));

            return offers;
        }



        /// <summary>
        /// Gets an the latitude and longitude of offer specified by given address
        /// </summary>
        /// <param name="vm">Address of the offer</param>
        /// <returns>JSON string representing an object of the string class</returns>
        /// <response code="200">Object of string</response>
        /// <response code="400">string "Wrong address"</response>
        [HttpPost("GetCoordinates/")]
        [ProducesResponseType(typeof(Coordinates), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetCoordinates(GetCoordsViewModel vm)
        {
            if (vm == null)
            {
                return null;
            }

            LogUserAction("OfferController", "GetCoordinates", vm.address.ToString(), _userActionService);
            Coordinates? coordinates = _offerService.GetCoordinates(vm.address).Result;

            return Ok(coordinates);
        }

        /// <summary>
        /// Gets the type of offer specified by given ID
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>JSON string representing an object of the Offer class</returns>
        /// <response code="200">Object of GetTypeDTO</response>
        /// <response code="400">null</response>
        [HttpGet("GetType/{offerId}")]
        public IActionResult? GetType(int offerId)
        {
            LogUserAction("OfferController", "GetType", offerId.ToString(), _userActionService);
            GetTypeDTO? type = _offerService.GetTypeOfOffer(offerId);

            if (type == null) return BadRequest(false);
            return Ok(type);
        }

        #region Search()

        /// <summary>
        /// Returns a list of offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of offers</response>
        [HttpPost]
        [Route("Search/Room/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchRoomRent(RoomRentFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Room/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            RoomRentFilteringDTO offerFilteringDTO = _mapper.Map<RoomRentFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        /// <summary>
        /// Returns a list of plot offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of PlotOffers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of plot offers</response>
        [HttpPost]
        [Route("Search/Plot/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchPlot(PlotOfferFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Plot/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            PlotOfferFilteringDTO offerFilteringDTO = _mapper.Map<PlotOfferFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }


        /// <summary>
        /// Returns a list of apartment rent offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of apartment rent offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of apartment rent offers</response>
        [HttpPost]
        [Route("Search/Rent/Apartment/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchApartmentRent(ApartmentRentFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Rent/Apartment/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            ApartmentRentFilteringDTO offerFilteringDTO = _mapper.Map<ApartmentRentFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        /// <summary>
        /// Returns a list of apartment sale offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of apartment sale offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of apartment sale offers</response>
        [HttpPost]
        [Route("Search/Sale/Apartment/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchApartmentSale(ApartmentSaleFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Sale/Apartment/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            ApartmentSaleFilteringDTO offerFilteringDTO = _mapper.Map<ApartmentSaleFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        /// <summary>
        /// Returns a list of hall rent offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of hall rent offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of hall rent offers</response>
        [HttpPost]
        [Route("Search/Rent/Hall/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchHallRent(HallRentFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Rent/Hall/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            HallRentFilteringDTO offerFilteringDTO = _mapper.Map<HallRentFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }


        /// <summary>
        /// Returns a list of hall sale offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of hall sale offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of hall sale offers</response>
        [HttpPost]
        [Route("Search/Sale/Hall/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchHallSale(HallSaleFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Sale/Hall/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            HallSaleFilteringDTO offerFilteringDTO = _mapper.Map<HallSaleFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        /// <summary>
        /// Returns a list of House rent offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of House rent offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of House rent offers</response>
        [HttpPost]
        [Route("Search/Rent/House/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchHouseRent(HouseRentFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Rent/House/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            HouseRentFilteringDTO offerFilteringDTO = _mapper.Map<HouseRentFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }


        /// <summary>
        /// Returns a list of House sale offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of House sale offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of House sale offers</response>
        [HttpPost]
        [Route("Search/Sale/House/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchHouseSale(HouseSaleFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Sale/House/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            HouseSaleFilteringDTO offerFilteringDTO = _mapper.Map<HouseSaleFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        /// <summary>
        /// Returns a list of Premises rent offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Premises rent offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of Premises rent offers</response>
        [HttpPost]
        [Route("Search/Rent/Premises/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchPremisesRent(PremisesRentFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Rent/Premises/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            PremisesRentFilteringDTO offerFilteringDTO = _mapper.Map<PremisesRentFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }


        /// <summary>
        /// Returns a list of Premises sale offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Premises sale offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of Premises sale offers</response>
        [HttpPost]
        [Route("Search/Sale/Premises/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchPremisesSale(PremisesSaleFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Sale/Premises/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            PremisesSaleFilteringDTO offerFilteringDTO = _mapper.Map<PremisesSaleFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        /// <summary>
        /// Returns a list of Garage rent offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Garage rent offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of Garage rent offers</response>
        [HttpPost]
        [Route("Search/Rent/Garage/")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchGarageRent(GarageRentFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Rent/Garage/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            GarageRentFilteringDTO offerFilteringDTO = _mapper.Map<GarageRentFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }


        /// <summary>
        /// Returns a list of Garage sale offers
        /// </summary>
        /// <param name="offerListFilterViewModel">Object containing information about the filtering</param>
        /// <returns>Object of the JsonResult class representing the list of Garage sale offers in JSON format</returns>
        /// <response code="200"></response>
        /// <response code="400">Error getting list of offers</response>
        /// <response code="200">List of Garage sale offers</response>
        [HttpPost]
        [Route("Search/Sale/Garage")]
        [ProducesResponseType(typeof(OfferListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult SearchGarageSale(GarageSaleFilteringViewModel offerListFilterViewModel)
        {
            LogUserAction("OfferController", "Search/Sale/Garage/", JsonSerializer.Serialize(offerListFilterViewModel), _userActionService);

            Paging paging = offerListFilterViewModel.Paging;
            SortOrder? sortOrder = offerListFilterViewModel.SortOrder;
            GarageSaleFilteringDTO offerFilteringDTO = _mapper.Map<GarageSaleFilteringDTO>(offerListFilterViewModel);


            OfferListing? offers = _offerService.GetOffers(paging, sortOrder, offerFilteringDTO, Path.Combine(_webHostEnvironment.ContentRootPath, "img"), null, null);

            if (offers == null)
            {
                string message = Translate(MessageHelper.EmptyOfferList);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(offers);
        }

        #endregion

        #region Delete()
        /// <summary>
        /// Delete an offer specified by given ID
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Offer deleted successfully</response>
        /// <response code="400">Error deleting Offer or there is no Offer with such Id</response>
        [HttpGet]
        [Route("Delete/{offerId}")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int offerId)
        {
            LogUserAction("OfferController", "Delete", offerId.ToString(), _userActionService);
            string message;
            int userId = GetUserId();

            DeleteOfferDTO dto = new(offerId)
            {
                LastUpdatedDate = DateTime.Now,
                DeletedDate = DateTime.Now
            };

            bool result = _offerService.DeleteOffer(userId, dto, out _errorMessage);
            if (result == false)
            {
                message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            message = Translate(MessageHelper.OfferDeletedSuccessfully);

            return Ok(new ResponseViewModel(message));
        }
        #endregion

        #region Create()

        /// <summary> Create a new plot offer </summary>
        /// <param name="newOffer">Object of the PlotOfferCreateViewModel class containing information about the new plot offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Plot/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreatePlotOffer(PlotOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "Plot/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.PLOT);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            PlotCreateOfferDTO offerCreateDTO = _mapper.Map<PlotCreateOfferDTO>(newOffer);

            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(newOffer.OfferType);
            offerCreateDTO.PlotType = EnumHelper.GetDescriptionFromEnum(newOffer.PlotType);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);

            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }


        /// <summary> Create a new apartment offer </summary>
        /// <param name="newOffer">Object of the ApartmentRentOfferCreateViewModel class containing information about the new apartment rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Rent/Apartment/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateApartmentRentOffer(ApartmentRentOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "Rent/Apartment/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            ApartmentRentOfferCreateDTO offerCreateDTO = _mapper.Map<ApartmentRentOfferCreateDTO>(newOffer);

            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.RoomCount = newOffer.RoomCount;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            offerCreateDTO.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfBuilding);
            offerCreateDTO.Floor = EnumHelper.GetDescriptionFromEnum(newOffer.Floor);
            offerCreateDTO.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(newOffer.BuildingMaterial);
            offerCreateDTO.WindowsType = EnumHelper.GetDescriptionFromEnum(newOffer.WindowsType);
            offerCreateDTO.HeatingType = EnumHelper.GetDescriptionFromEnum(newOffer.HeatingType);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            offerCreateDTO.ApartmentFinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);
            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }


        /// <summary> Create a new apartment offer </summary>
        /// <param name="newOffer">Object of the ApartmentSaleOfferCreateViewModel class containing information about the new apartment sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Sale/Apartment/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateApartmentSaleOffer(ApartmentSaleOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "Sale/Apartment/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            ApartmentSaleOfferCreateDTO offerCreateDTO = _mapper.Map<ApartmentSaleOfferCreateDTO>(newOffer);

            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.RoomCount = newOffer.RoomCount;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfBuilding);
            offerCreateDTO.Floor = EnumHelper.GetDescriptionFromEnum(newOffer.Floor);
            offerCreateDTO.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(newOffer.BuildingMaterial);
            offerCreateDTO.WindowsType = EnumHelper.GetDescriptionFromEnum(newOffer.WindowsType);
            offerCreateDTO.HeatingType = EnumHelper.GetDescriptionFromEnum(newOffer.HeatingType);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            offerCreateDTO.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfMarket);
            offerCreateDTO.FormOfProperty = EnumHelper.GetDescriptionFromEnum(newOffer.FormOfProperty);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            offerCreateDTO.ApartmentFinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }



        /// <summary> Create a new garage rent offer </summary>
        /// <param name="newOffer">Object of the GarageRentOfferCreateViewModel class containing information about the new garage rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Rent/Garage/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateGarageRentOffer(GarageRentOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "Rent/Garage/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            GarageRentCreateOfferDTO offerCreateDTO = _mapper.Map<GarageRentCreateOfferDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.Construction = EnumHelper.GetDescriptionFromEnum(newOffer.Construction);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.Lighting = EnumHelper.GetDescriptionFromEnum(newOffer.Lighting);
            offerCreateDTO.Heating = EnumHelper.GetDescriptionFromEnum(newOffer.Heating);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //second - add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }



        /// <summary> Create a new garage sale offer </summary>
        /// <param name="newOffer">Object of the GarageSaleOfferCreateViewModel class containing information about the new garage sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Sale/Garage/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateGarageSaleOffer(GarageSaleOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "Sale/Garage/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);

            GarageSaleOfferCreateDTO offerCreateDTO = _mapper.Map<GarageSaleOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.Construction = EnumHelper.GetDescriptionFromEnum(newOffer.Construction);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.Lighting = EnumHelper.GetDescriptionFromEnum(newOffer.Lighting);
            offerCreateDTO.Heating = EnumHelper.GetDescriptionFromEnum(newOffer.Heating);
            offerCreateDTO.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfMarket);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //second - add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }



        /// <summary> Create a new hall rent offer </summary>
        /// <param name="newOffer">Object of the HallRentOfferCreateViewModel  class containing information about the new hall rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Rent/Hall/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateHallRentOffer(HallRentOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "Rent/Hall/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            //map the ViewModel to DTO
            HallRentOfferCreateDTO offerCreateDTO = _mapper.Map<HallRentOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.Construction = EnumHelper.GetDescriptionFromEnum(newOffer.Construction);
            offerCreateDTO.ParkingSpace = EnumHelper.GetDescriptionFromEnum(newOffer.ParkingSpace);
            offerCreateDTO.FinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            offerCreateDTO.Flooring = EnumHelper.GetDescriptionFromEnum(newOffer.Flooring);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //second - add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }



        /// <summary> Create a new hall sale offer </summary>
        /// <param name="newOffer">Object of the HallSaleOfferCreateViewModel class containing information about the new hall sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Sale/Hall/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateHallSaleOffer(HallSaleOfferCreateViewModel newOffer)
        {
            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            LogUserAction("OfferController", "Sale/Hall/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            HallSaleOfferCreateDTO offerCreateDTO = _mapper.Map<HallSaleOfferCreateDTO>(newOffer);

            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.Construction = EnumHelper.GetDescriptionFromEnum(newOffer.Construction);
            offerCreateDTO.ParkingSpace = EnumHelper.GetDescriptionFromEnum(newOffer.ParkingSpace);
            offerCreateDTO.FinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            offerCreateDTO.Flooring = EnumHelper.GetDescriptionFromEnum(newOffer.Flooring);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            offerCreateDTO.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfMarket);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //first - create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //second - add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }


        /// <summary> Create a new house rent offer </summary>
        /// <param name="newOffer">Object of the HouseRentOfferCreateViewModel class containing information about the new house rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Rent/House/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateHouseRentOffer(HouseRentOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "/Rent/House/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            string message, uploadResultMessage;
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();

            HouseRentOfferCreateDTO offerCreateDTO = _mapper.Map<HouseRentOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.RoomCount = newOffer.RoomCount;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfBuilding);
            offerCreateDTO.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(newOffer.BuildingMaterial);
            offerCreateDTO.AtticType = EnumHelper.GetDescriptionFromEnum(newOffer.AtticType);
            offerCreateDTO.RoofType = EnumHelper.GetDescriptionFromEnum(newOffer.RoofType);
            offerCreateDTO.RoofingType = EnumHelper.GetDescriptionFromEnum(newOffer.RoofingType);
            offerCreateDTO.FinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            offerCreateDTO.WindowsType = EnumHelper.GetDescriptionFromEnum(newOffer.WindowsType);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }

        /// <summary> Create a new house sale offer </summary>
        /// <param name="newOffer">Object of the HouseSaleOfferCreateViewModel class containing information about the new house sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Sale/House/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateHouseSaleOffer(HouseSaleOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "/Sale/House/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            string message, uploadResultMessage;
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();

            HouseSaleOfferCreateDTO offerCreateDTO = _mapper.Map<HouseSaleOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.RoomCount = newOffer.RoomCount;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfBuilding);
            offerCreateDTO.BuildingMaterial = EnumHelper.GetDescriptionFromEnum(newOffer.BuildingMaterial);
            offerCreateDTO.AtticType = EnumHelper.GetDescriptionFromEnum(newOffer.AtticType);
            offerCreateDTO.RoofType = EnumHelper.GetDescriptionFromEnum(newOffer.RoofType);
            offerCreateDTO.RoofingType = EnumHelper.GetDescriptionFromEnum(newOffer.RoofingType);
            offerCreateDTO.FinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            offerCreateDTO.WindowsType = EnumHelper.GetDescriptionFromEnum(newOffer.WindowsType);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            offerCreateDTO.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfMarket);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }



        /// <summary> Create a new room rent offer </summary>
        /// <param name="newOffer">Object of the RoomRentingOfferCreateViewModel class containing information about the new room rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Rent/Room/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateRoomRentOffer(RoomRentingOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "/Rent/Room/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.ROOM);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            RoomOfferCreateDTO offerCreateDTO = _mapper.Map<RoomOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            offerCreateDTO.TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfBuilding);
            offerCreateDTO.Floor = EnumHelper.GetDescriptionFromEnum(newOffer.Floor);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }



        /// <summary> Create a new business premises sale offer </summary>
        /// <param name="newOffer">Object of the BusinessPremisesSaleOfferCreateViewModel class containing information about the new premises sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Sale/Premises/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreatePremisesSaleOffer(BusinessPremisesSaleOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "/Sale/Premises/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            PremisesSaleOfferCreateDTO offerCreateDTO = _mapper.Map<PremisesSaleOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.RoomCount = newOffer.RoomCount;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            offerCreateDTO.FinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.Floor = EnumHelper.GetDescriptionFromEnum(newOffer.Floor);
            offerCreateDTO.TypeOfMarket = EnumHelper.GetDescriptionFromEnum(newOffer.TypeOfMarket);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }

        /// <summary> Create a new business premises rent offer </summary>
        /// <param name="newOffer">Object of the BusinessPremisesRentOfferCreateViewModel class containing information about the new premises rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer created successfully"</response>
        /// <response code="400">string "Error creating offer (check parameters)<br />
        /// string "Error creating offer<br />
        /// string "Error saving offer in database"
        /// </response>
        [HttpPost]
        [Route("Rent/Premises/Create")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreatePremisesRentOffer(BusinessPremisesRentOfferCreateViewModel newOffer)
        {
            LogUserAction("OfferController", "/Rent/Premises/Create", JsonSerializer.Serialize(newOffer), _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            string userRole = GetUserRole();
            int userCreatedId = GetUserId();
            string message, uploadResultMessage;

            PremisesRentOfferCreateDTO offerCreateDTO = _mapper.Map<PremisesRentOfferCreateDTO>(newOffer);
            offerCreateDTO.SellerType = userRole;
            offerCreateDTO.RoomCount = newOffer.RoomCount;
            offerCreateDTO.Area = newOffer.Area;
            offerCreateDTO.FinishCondition = EnumHelper.GetDescriptionFromEnum(newOffer.FinishCondition);
            offerCreateDTO.Location = EnumHelper.GetDescriptionFromEnum(newOffer.Location);
            offerCreateDTO.Floor = EnumHelper.GetDescriptionFromEnum(newOffer.Floor);
            offerCreateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW);
            offerCreateDTO.OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            offerCreateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(newOffer.Voivodeship);
            //create a new offer
            bool result = _offerService.Create(offerCreateDTO, userCreatedId, out _errorMessage);

            if (result == false)
            {
                message = Translate(_errorMessage);
                return BadRequest(new ResponseViewModel(message));
            }

            //second - add the new photos for the offer we just created
            (result, uploadResultMessage) = _offerService.UploadPhotos(newOffer.Photos, userCreatedId, Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"), type).Result;
            message = Translate(uploadResultMessage);

            return result == false ? BadRequest(new ResponseViewModel(message)) : Ok(new ResponseViewModel(message));
        }
        #endregion

        #region Update()

        /// <summary>
        /// Update an offer specified by given ID
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>RedirectToActionResult</returns>
        /// <response code="200">Offer updated successfully</response>
        /// <response code="400">Error updating Offer or there is no Offer with such Id</response>
        [HttpGet]
        [Route("Update/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Update(int offerId)
        {
            LogUserAction("OfferController", "Update", offerId.ToString(), _userActionService);
            string message;
            GetTypeDTO? typeDto = _offerService.GetTypeOfOffer(offerId);

            if (typeDto?.type != null)
            {
                if (typeDto.type.Contains(EnumHelper.GetDescriptionFromEnum(EstateType.ROOM)))
                {
                    return RedirectToAction("UpdateRoomOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdateRoomOffer", offerId = offerId }));
                }
                else if (typeDto.type.Contains(EnumHelper.GetDescriptionFromEnum(EstateType.PLOT)))
                {
                    return RedirectToAction("UpdatePlotOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
                {
                    return RedirectToAction("UpdateHouseRentOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
                {
                    return RedirectToAction("UpdateHouseSaleOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
                {
                    return RedirectToAction("UpdateHallRentOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
                {
                    return RedirectToAction("UpdateHallSaleOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
                {
                    return RedirectToAction("UpdatePremisesRentOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
                {
                    return RedirectToAction("UpdatePremisesSaleOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
                {
                    return RedirectToAction("UpdateGarageRentOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
                {
                    return RedirectToAction("UpdateGarageSaleOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
                {
                    return RedirectToAction("UpdateApartmentRentOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else if (typeDto.type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
                {
                    return RedirectToAction("UpdateApartmentSaleOffer", new RouteValueDictionary(new { Controller = "Offer", Action = "UpdatePlotOffer", offerId = offerId }));
                }
                else
                {
                    message = Translate(ErrorMessageHelper.ErrorUpdatingOffer);

                    return BadRequest(new ResponseViewModel(message));
                }
            }
            else
            {
                message = Translate(ErrorMessageHelper.OfferUpdateError_UnknownType);

                return BadRequest(new ResponseViewModel(message));
            }
        }

        /// <summary> Update a plot offer </summary>
        /// <param name="updatedOffer">Object of the PlotOfferUpdateViewModel class containing information about the new plot offer</param>
        /// <param name="offerId">Id of updated offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/Plot/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdatePlotOffer(PlotOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/Plot", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.PLOT);
            int userId = GetUserId();

            PlotOfferUpdateDTO offerUpdateDTO = _mapper.Map<PlotOfferUpdateDTO>(updatedOffer);
            offerUpdateDTO.PlotType = updatedOffer.PlotType;
            offerUpdateDTO.Location = updatedOffer.Location;
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);


            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a Room offer </summary>
        /// <param name="updatedOffer">Object of the RoomRentingOfferUpdateViewModel class containing information about the new Room offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/Room/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateRoomOffer(RoomRentingOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/Room", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.ROOM);
            int userId = GetUserId();

            RoomOfferUpdateDTO offerUpdateDTO = _mapper.Map<RoomOfferUpdateDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);
            offerUpdateDTO.Floor = updatedOffer.Floor;

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a apartment sale offer </summary>
        /// <param name="updatedOffer">Object of the ApartmentSaleOfferUpdateViewModel  class containing information about the new apartment sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/ApartmentSale/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateApartmentSaleOffer(ApartmentSaleOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/ApartmentSale", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            int userId = GetUserId();

            ApartmentSaleUpdateOfferDTO offerUpdateDTO = _mapper.Map<ApartmentSaleUpdateOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);
            offerUpdateDTO.FormOfProperty = EnumHelper.GetDescriptionFromEnum(updatedOffer.FormOfProperty);
            offerUpdateDTO.ApartmentFinishCondition = EnumHelper.GetDescriptionFromEnum(updatedOffer.FinishCondition);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a apartment rent offer </summary>
        /// <param name="updatedOffer">Object of the ApartmentRentOfferUpdateViewModel  class containing information about the new apartment rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/ApartmentRent/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateApartmentRentOffer(ApartmentRentOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/ApartmentRent", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            int userId = GetUserId();

            ApartmentRentUpdateOfferDTO offerUpdateDTO = _mapper.Map<ApartmentRentUpdateOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);
            offerUpdateDTO.ApartmentFinishCondition = EnumHelper.GetDescriptionFromEnum(updatedOffer.FinishCondition);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a House rent offer </summary>
        /// <param name="updatedOffer">Object of the HouseRentOfferUpdateViewModel  class containing information about the new House rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/HouseRent/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateHouseRentOffer(HouseRentOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/HouseRent", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            int userId = GetUserId();

            HouseRentOfferUpdateDTO offerUpdateDTO = _mapper.Map<HouseRentOfferUpdateDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }


        /// <summary> Update a House sale offer </summary>
        /// <param name="updatedOffer">Object of the HouseSaleOfferUpdateViewModel  class containing information about the new House sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/HouseSale/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateHouseSaleOffer(HouseSaleOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/HouseSale", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            int userId = GetUserId();

            HouseSaleOfferUpdateDTO offerUpdateDTO = _mapper.Map<HouseSaleOfferUpdateDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);
            offerUpdateDTO.FinishCondition = updatedOffer.FinishCondition;

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a Garage sale offer </summary>
        /// <param name="updatedOffer">Object of the GarageSaleOfferUpdateViewModel  class containing information about the new Garage sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/GarageSale/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateGarageSaleOffer(GarageSaleOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/GarageSale", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            int userId = GetUserId();

            GarageUpdateSaleOfferDTO offerUpdateDTO = _mapper.Map<GarageUpdateSaleOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a Garage Rent offer </summary>
        /// <param name="updatedOffer">Object of the GarageRentOfferUpdateViewModel  class containing information about the new Garage Rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/GarageRent/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateGarageRentOffer(GarageRentOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/GarageRent", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            int userId = GetUserId();

            GarageUpdateRentOfferDTO offerUpdateDTO = _mapper.Map<GarageUpdateRentOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a Hall sale offer </summary>
        /// <param name="updatedOffer">Object of the HallSaleOfferUpdateViewModel  class containing information about the new Hall sale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/HallSale/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateHallSaleOffer(HallSaleOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/HallSale", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            int userId = GetUserId();

            HallSaleUpdateOfferDTO offerUpdateDTO = _mapper.Map<HallSaleUpdateOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a Hall Rent offer </summary>
        /// <param name="updatedOffer">Object of the HallRentOfferUpdateViewModel  class containing information about the new Hall Rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/HallRent/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdateHallRentOffer(HallRentOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/HallRent", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            int userId = GetUserId();

            HallRentUpdateOfferDTO offerUpdateDTO = _mapper.Map<HallRentUpdateOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a PremisesSale offer </summary>
        /// <param name="updatedOffer">Object of the BusinessPremisesSaleOfferUpdateViewModel  class containing information about the new PremisesSale offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/PremisesSale/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdatePremisesSaleOffer(BusinessPremisesSaleOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/PremisesSale", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE);
            int userId = GetUserId();

            BusinessPremisesSaleUpdateOfferDTO offerUpdateDTO = _mapper.Map<BusinessPremisesSaleUpdateOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }

        /// <summary> Update a Premises Rent offer </summary>
        /// <param name="updatedOffer">Object of the BusinessPremisesRentOfferUpdateViewModel  class containing information about the new Premises Rent offer</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">string "Offer updated successfully"</response>
        /// <response code="400">string "Error updating offer (check parameters)<br />
        /// string "Error updating offer<br />
        /// string "Error updating offer in database"
        /// </response>
        [HttpPost]
        [Route("Update/PremisesRent/{offerId}/")]
        [RequireUserRole("PRIVATE", "AGENCY", "ADMIN")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult UpdatePremisesRentOffer(BusinessPremisesRentOfferUpdateViewModel updatedOffer, int offerId)
        {
            LogUserAction("OfferController", "Update/PremisesRent", $"{offerId}, {JsonSerializer.Serialize(updatedOffer)}", _userActionService);

            string type = EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT);
            int userId = GetUserId();

            BusinessPremisesRentUpdateOfferDTO offerUpdateDTO = _mapper.Map<BusinessPremisesRentUpdateOfferDTO>(updatedOffer);
            offerUpdateDTO.OfferStatus = EnumHelper.GetDescriptionFromEnum(updatedOffer.OfferStatus);
            offerUpdateDTO.Voivodeship = EnumHelper.GetDescriptionFromEnum(updatedOffer.Voivodeship);

            //update the offer
            bool result = _offerService.Update(offerUpdateDTO, type, offerId, userId, out _errorMessage);

            return result == false ? BadRequest(new ResponseViewModel(Translate(_errorMessage))) : Ok(new ResponseViewModel(Translate(_errorMessage)));
        }
        #endregion

        #region UserFavourites

        /// <summary>
        /// Adds given offer to users favourites list
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <returns>Confirmation</returns>
        /// <response code="200">"Offer added to favourites"</response>
        /// <response code="400">string "No offer with this OfferId"</response>
        [HttpGet]
        [Route("AddToFavourites/{offerId}/")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult AddToFavourites(int offerId)
        {
            LogUserAction("OfferController", "AddToFavourites", offerId.ToString(), _userActionService);
            int userId = GetUserId();

            if (userId == 0)
            {
                RedirectToAction("SignIn", "Auth");
            }

            bool result = _offerService.AddToFavourites(offerId, userId, out _errorMessage);

            if (result == false)
            {
                string message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(new ResponseViewModel(MessageHelper.OfferAddedToFavouritesSuccessfully));
        }

        /// <summary>
        /// Removes given offer from current users favourites list
        /// </summary>
        /// <returns>Confirmation</returns>
        /// <response code="200">"Offer removed from favourites"</response>
        /// <response code="400">string "No offer with this OfferId"</response>
        [HttpGet]
        [Route("RemoveFromFavourites/{offerId}/")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult RemoveFromFavourites(int offerId)
        {
            LogUserAction("OfferController", "RemoveFromFavourites", offerId.ToString(), _userActionService);
            int userId = GetUserId();

            if (userId == 0)
            {
                //user jest niezalogowany, przekieruj do strony logowania
            }

            bool result = _offerService.RemoveFromFavourites(offerId, userId, out _errorMessage);

            if (result == false)
            {
                string message = Translate(_errorMessage);

                return BadRequest(new ResponseViewModel(message));
            }

            return Ok(new ResponseViewModel(MessageHelper.OfferRemovedFromFavouritesSuccessfully));
        }

        /// <summary>
        /// Gets a list of current users favourite offers
        /// </summary>
        /// <returns>Object of the JsonResult class representing the list of Recruitments in JSON format</returns>
        /// <response code="200">"List of user favourites"</response>
        /// <response code="400">"Error getting current users favourite list"</response>
        [HttpPost]
        [Route("User/GetFavourites/")]
        [ProducesResponseType(typeof(UserFavouriteListing), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult GetFavourites(UserFavouritesFilterViewModel viewModel)
        {
            LogUserAction("UserController", "GetFavourites", JsonSerializer.Serialize(viewModel), _userActionService);
            int userId = GetUserId();

            if (userId == 0)
            {
                return BadRequest("NIE JESTEŚ ZALOGOWANY");
            }

            Paging paging = viewModel.Paging;

            UserFavouriteListing favourites = _offerService.GetUserFavourites(paging, userId, Path.Combine(_webHostEnvironment.ContentRootPath, "img"));

            if (favourites == null)
            {
                string message = Translate(MessageHelper.UserFavListEmpty);

                return Ok(new ResponseViewModel(message));
            }

            return Ok(favourites);

        }

        #endregion

        /// <summary>
        /// Generates a contract of given type
        /// </summary>
        /// <param name="offerId">ID of the offer</param>
        /// <param name="contractType">Type of contract</param>
        /// <returns>Confirmation</returns>
        /// <response code="200">"Offer added to favourites"</response>
        /// <response code="400">string "No offer with this OfferId"</response>
        [HttpGet]
        [Route("GenerateContract/{contractType}/{offerId}/")]
        [SwaggerOperation(OperationId = nameof(GenerateContract), Tags = new[] { "" })]
        [SwaggerResponse(StatusCodes.Status200OK, nameof(StatusCodes.Status200OK), typeof(Stream))]

        public async Task<IActionResult> GenerateContract(string contractType, int offerId)
        {
            LogUserAction("OfferController", "GenerateContract", contractType + " " + offerId.ToString(), _userActionService);
            string message, fileName;
            int userId = GetUserId();
            byte[]? resultMS = _offerService.GenerateContract(Path.Combine(_webHostEnvironment.ContentRootPath, "pdf"), userId, offerId, contractType, out _errorMessage, out fileName);

            if (resultMS == null)
            {
                return BadRequest(_errorMessage);
            }
            else
            {
                return File(resultMS, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }
    }
}