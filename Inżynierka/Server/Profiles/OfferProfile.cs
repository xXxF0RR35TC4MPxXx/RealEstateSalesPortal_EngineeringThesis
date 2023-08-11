using AutoMapper;
using Inżynierka.Shared.ViewModels.Offer.Create;
using Inżynierka.Shared.ViewModels.Offer.Filtering;
using Inżynierka.Shared.ViewModels.Offer.Update;
using Inżynierka.Shared.DTOs.Offers.Create;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka.Shared.DTOs.Offers.Update;
using Inżynierka.Shared.Entities.OfferTypes.Apartment;
using Inżynierka.Shared.Entities.OfferTypes.BusinessPremises;
using Inżynierka.Shared.Entities.OfferTypes.Garage;
using Inżynierka.Shared.Entities.OfferTypes.Hall;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka.Shared.Entities.OfferTypes.Plot;
using Inżynierka.Shared.Entities.OfferTypes.Room;

namespace Inżynierka.Server.Profiles
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<HouseSaleOfferCreateViewModel, HouseSaleOfferCreateDTO>()
                .ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<HouseRentOfferCreateViewModel, HouseRentOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<HallSaleOfferCreateViewModel, HallSaleOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<HallRentOfferCreateViewModel, HallRentOfferCreateDTO>()
                .ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<ApartmentSaleOfferCreateViewModel, ApartmentSaleOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<ApartmentRentOfferCreateViewModel, ApartmentRentOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<GarageSaleOfferCreateViewModel, GarageSaleOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<GarageRentOfferCreateViewModel, GarageRentCreateOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<BusinessPremisesSaleOfferCreateViewModel, PremisesSaleOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<BusinessPremisesRentOfferCreateViewModel, PremisesRentOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<PlotOfferCreateViewModel, PlotCreateOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<RoomRentingOfferCreateViewModel, RoomOfferCreateDTO>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));



            CreateMap<HouseSaleOfferCreateDTO, HouseSaleOffer>()
                .ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<HouseRentOfferCreateDTO, HouseRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<HallSaleOfferCreateDTO, HallSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<HallRentOfferCreateDTO, HallRentOffer>()
                .ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<ApartmentSaleOfferCreateDTO, ApartmentSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<ApartmentRentOfferCreateDTO, ApartmentRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<GarageSaleOfferCreateDTO, GarageSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<GarageRentCreateOfferDTO, GarageRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<PremisesSaleOfferCreateDTO, BusinessPremisesSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<PremisesRentOfferCreateDTO, BusinessPremisesRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.RoomCount, opt => opt.MapFrom(src => src.RoomCount))
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<PlotCreateOfferDTO, PlotOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<RoomOfferCreateDTO, RoomRentingOffer>().ForMember(s => s.Photos, opt => opt.Ignore())
                .ForMember(s => s.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<RoomRentingReadOfferDTO, RoomRentingOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<PlotReadOfferDTO, PlotOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HouseRentReadOfferDTO, HouseRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HouseSaleReadOfferDTO, HouseSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HallRentReadOfferDTO, HallRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HallSaleReadOfferDTO, HallSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<GarageRentReadOfferDTO, GarageRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<GarageSaleReadOfferDTO, GarageSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<BusinessPremisesRentReadOfferDTO, BusinessPremisesRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<BusinessPremisesSaleReadOfferDTO, BusinessPremisesSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<ApartmentRentReadOfferDTO, ApartmentRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<ApartmentSaleReadOfferDTO, ApartmentSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
                        
            CreateMap<RoomRentingOffer, RoomRentingReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<PlotOffer, PlotReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HouseRentOffer, HouseRentReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HouseSaleOffer, HouseSaleReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HallRentOffer, HallRentReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HallSaleOffer, HallSaleReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<GarageRentOffer, GarageRentReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<GarageSaleOffer, GarageSaleReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<BusinessPremisesRentOffer, BusinessPremisesRentReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<BusinessPremisesSaleOffer, BusinessPremisesSaleReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<ApartmentRentOffer, ApartmentRentReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<ApartmentSaleOffer, ApartmentSaleReadOfferDTO>().ForMember(s => s.Photos, opt => opt.Ignore());

            CreateMap<RoomOfferUpdateDTO, RoomRentingOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<PlotOfferUpdateDTO, PlotOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HouseRentOfferUpdateDTO, HouseRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HouseSaleOfferUpdateDTO, HouseSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HallRentUpdateOfferDTO, HallRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<HallSaleUpdateOfferDTO, HallSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<GarageUpdateRentOfferDTO, GarageRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<GarageUpdateSaleOfferDTO, GarageSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<BusinessPremisesRentUpdateOfferDTO, BusinessPremisesRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<BusinessPremisesSaleUpdateOfferDTO, BusinessPremisesSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<ApartmentRentUpdateOfferDTO, ApartmentRentOffer>().ForMember(s => s.Photos, opt => opt.Ignore());
            CreateMap<ApartmentSaleUpdateOfferDTO, ApartmentSaleOffer>().ForMember(s => s.Photos, opt => opt.Ignore());


            CreateMap<RoomRentingOfferUpdateViewModel, RoomOfferUpdateDTO>();
            CreateMap<PlotOfferUpdateViewModel, PlotOfferUpdateDTO>();
            CreateMap<HouseRentOfferUpdateViewModel, HouseRentOfferUpdateDTO>();
            CreateMap<HouseSaleOfferUpdateViewModel, HouseSaleOfferUpdateDTO>();
            CreateMap<HallRentOfferUpdateViewModel, HallRentUpdateOfferDTO>();
            CreateMap<HallSaleOfferUpdateViewModel, HallSaleUpdateOfferDTO>();
            CreateMap<GarageRentOfferUpdateViewModel, GarageUpdateRentOfferDTO>();
            CreateMap<GarageSaleOfferUpdateViewModel, GarageUpdateSaleOfferDTO>();
            CreateMap<BusinessPremisesRentOfferUpdateViewModel, BusinessPremisesRentUpdateOfferDTO>();
            CreateMap<BusinessPremisesSaleOfferUpdateViewModel, BusinessPremisesSaleUpdateOfferDTO>();
            CreateMap<ApartmentRentOfferUpdateViewModel, ApartmentRentUpdateOfferDTO>();
            CreateMap<ApartmentSaleOfferUpdateViewModel, ApartmentSaleUpdateOfferDTO>();
            
            CreateMap<RoomRentingOffer, RoomRentingOfferUpdateViewModel>()
                .ForMember(s => s.Floor, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.TypeOfBuilding, opt => opt.Ignore());

            CreateMap<PlotOffer, PlotOfferUpdateViewModel>()
                .ForMember(s => s.PlotType, opt => opt.Ignore())
                .ForMember(s => s.Location, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.OfferType, opt => opt.Ignore());

            CreateMap<HouseRentOffer, HouseRentOfferUpdateViewModel>()
                .ForMember(s => s.TypeOfBuilding, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.BuildingMaterial, opt => opt.Ignore())
                .ForMember(s => s.AtticType, opt => opt.Ignore())
                .ForMember(s => s.RoofType, opt => opt.Ignore())
                .ForMember(s => s.RoofingType, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore())
                .ForMember(s => s.WindowsType, opt => opt.Ignore())
                .ForMember(s => s.Location, opt => opt.Ignore());

            CreateMap<HouseSaleOffer, HouseSaleOfferUpdateViewModel>()
                .ForMember(s => s.TypeOfBuilding, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.BuildingMaterial, opt => opt.Ignore())
                .ForMember(s => s.AtticType, opt => opt.Ignore())
                .ForMember(s => s.RoofType, opt => opt.Ignore())
                .ForMember(s => s.RoofingType, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore())
                .ForMember(s => s.WindowsType, opt => opt.Ignore())
                .ForMember(s => s.Location, opt => opt.Ignore())
                .ForMember(s => s.TypeOfMarket, opt => opt.Ignore());

            CreateMap<HallRentOffer, HallRentOfferUpdateViewModel>()
                .ForMember(s => s.Construction, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.ParkingSpace, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore())
                .ForMember(s => s.Flooring, opt => opt.Ignore());

            CreateMap<HallSaleOffer, HallSaleOfferUpdateViewModel>()
                .ForMember(s => s.Construction, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.ParkingSpace, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore())
                .ForMember(s => s.Flooring, opt => opt.Ignore())
                .ForMember(s => s.TypeOfMarket, opt => opt.Ignore());

            CreateMap<GarageRentOffer,GarageRentOfferUpdateViewModel>()
                .ForMember(s => s.Construction, opt => opt.Ignore())
                .ForMember(s => s.Location, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.Lighting, opt => opt.Ignore())
                .ForMember(s => s.Heating, opt => opt.Ignore());

            CreateMap<GarageSaleOffer,GarageSaleOfferUpdateViewModel>()
                .ForMember(s => s.TypeOfMarket, opt => opt.Ignore())
                .ForMember(s => s.Construction, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.Location, opt => opt.Ignore())
                .ForMember(s => s.Lighting, opt => opt.Ignore())
                .ForMember(s => s.Heating, opt => opt.Ignore());

            CreateMap<BusinessPremisesRentOffer,BusinessPremisesRentOfferUpdateViewModel>()
                .ForMember(s => s.Location, opt => opt.Ignore())
                .ForMember(s => s.Floor, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore());

            CreateMap<BusinessPremisesSaleOffer, BusinessPremisesSaleOfferUpdateViewModel>()
                .ForMember(s => s.Location, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.Floor, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore())
                .ForMember(s => s.TypeOfMarket, opt => opt.Ignore());

            CreateMap<ApartmentRentOffer, ApartmentRentOfferUpdateViewModel>()
                .ForMember(s => s.TypeOfBuilding, opt => opt.Ignore())
                .ForMember(s => s.Floor, opt => opt.Ignore())
                .ForMember(s => s.BuildingMaterial, opt => opt.Ignore())
                .ForMember(s => s.WindowsType, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.HeatingType, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore());

            CreateMap<ApartmentSaleOffer, ApartmentSaleOfferUpdateViewModel>()
                .ForMember(s => s.TypeOfBuilding, opt => opt.Ignore())
                .ForMember(s => s.Floor, opt => opt.Ignore())
                .ForMember(s => s.BuildingMaterial, opt => opt.Ignore())
                .ForMember(s => s.Voivodeship, opt => opt.Ignore())
                .ForMember(s => s.OfferStatus, opt => opt.Ignore())
                .ForMember(s => s.WindowsType, opt => opt.Ignore())
                .ForMember(s => s.HeatingType, opt => opt.Ignore())
                .ForMember(s => s.TypeOfMarket, opt => opt.Ignore())
                .ForMember(s => s.FormOfProperty, opt => opt.Ignore())
                .ForMember(s => s.FinishCondition, opt => opt.Ignore());

            CreateMap<RoomRentFilteringViewModel, RoomRentFilteringDTO>();
            CreateMap<PlotOfferFilteringViewModel, PlotOfferFilteringDTO>();
            CreateMap<ApartmentRentFilteringViewModel, ApartmentRentFilteringDTO>();
            CreateMap<ApartmentSaleFilteringViewModel, ApartmentSaleFilteringDTO>();
            CreateMap<HallRentFilteringViewModel, HallRentFilteringDTO>();
            CreateMap<HallSaleFilteringViewModel, HallSaleFilteringDTO>();
            CreateMap<HouseRentFilteringViewModel, HouseRentFilteringDTO>();
            CreateMap<HouseSaleFilteringViewModel, HouseSaleFilteringDTO>();
            CreateMap<PremisesRentFilteringViewModel, PremisesRentFilteringDTO>();
            CreateMap<PremisesSaleFilteringViewModel, PremisesSaleFilteringDTO>();
            CreateMap<GarageRentFilteringViewModel, GarageRentFilteringDTO>();
            CreateMap<GarageSaleFilteringViewModel, GarageSaleFilteringDTO>();

            CreateMap<UserPageFilteringViewModel, UserOffersFilteringDTO>()
                .ForMember(s=> s.availableEstateType, opt=>opt.Ignore())
                .ForMember(s=> s.availableOfferTypes, opt=>opt.Ignore());

        }
    }
}
