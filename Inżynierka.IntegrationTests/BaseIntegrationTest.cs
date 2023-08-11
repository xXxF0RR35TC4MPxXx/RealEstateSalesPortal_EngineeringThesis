using Inżynierka.Shared.Repositories;
using Inżynierka.Shared;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Inżynierka.Shared.Entities.OfferTypes.House;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using NUnit.Framework;
using System.Diagnostics.Metrics;

namespace Inżynierka.IntegrationTests
{
    public class BaseIntegrationTest
    {
        public string conStr;
        public DataContext context;
        public DataContext context2;
        public AgencyRepository agencyRepo;
        public UserRepository userRepo;
        public OfferRepository offerRepo;
        public OfferRepository offerRepo2;
        public UserRepository userRepo2;
        public UserFavouriteRepository userFavRepo;
        public UserFavouriteRepository userFavRepo2;
        public UserEventRepository userEventRepo;
        public UserEventRepository userEventRepo2;
        public IConfiguration configuration;

        public HouseSaleOffer houseSaleOffer;
        public HouseSaleOffer invalidHouseSaleOffer;

        public BaseIntegrationTest()
        {
            invalidHouseSaleOffer = new()
            {
                TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(TypeOfBuilding.DETACHED_HOUSE),
                FloorCount = 2,
                BuildingMaterial = EnumHelper.GetDescriptionFromEnum(BuildingMaterial.BRICK),
                ConstructionYear = 2022,
                AtticType = EnumHelper.GetDescriptionFromEnum(AtticType.USABLE),
                RoofType = EnumHelper.GetDescriptionFromEnum(RoofType.SLOPING),
                RoofingType = EnumHelper.GetDescriptionFromEnum(RoofingType.CORRUGATED_IRON),
                FinishCondition = EnumHelper.GetDescriptionFromEnum(HouseFinishCondition.READY_TO_MOVE_IN),
                WindowsType = EnumHelper.GetDescriptionFromEnum(WindowType.PLASTIC),
                Location = EnumHelper.GetDescriptionFromEnum(Location.CITY),
                AvailableFromDate = "Now",
                IsARecreationHouse = false,
                AntiBurglaryBlinds = true,
                AntiBurglaryWindowsOrDoors = true,
                IntercomOrVideophone = false,
                MonitoringOrSecurity = false,
                AlarmSystem = false,
                ClosedArea = false,
                BrickFence = false,
                Net = false,
                MetalFence = true,
                WoodenFence = false,
                ConcreteFence = false,
                Hedge = false,
                OtherFencing = false,
                Geothermics = false,
                OilHeating = false,
                ElectricHeating = false,
                DistrictHeating = true,
                TileStoves = false,
                GasHeating = false,
                CoalHeating = true,
                Biomass = false,
                HeatPump = false,
                SolarCollector = false,
                FireplaceHeating = false,
                Internet = true,
                CableTV = true,
                HomePhone = false,
                Water = true,
                Electricity = true,
                SewageSystem = true,
                Gas = true,
                SepticTank = false,
                SewageTreatmentPlant = false,
                FieldDriveway = false,
                PavedDriveway = false,
                AsphaltDriveway = true,
                SwimmingPool = false,
                ParkingSpace = true,
                Basement = true,
                Attic = true,
                Garden = true,
                Furnishings = true,
                AirConditioning = true,
                TypeOfMarket = EnumHelper.GetDescriptionFromEnum(TypeOfMarket.PRIMARY_MARKET),
            };

            houseSaleOffer = new()
            {
                SellerType = EnumHelper.GetDescriptionFromEnum(UserRoles.PRIVATE),
                AddedDate = DateTime.UtcNow,
                EstateType = EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE),
                SellerId = 7,
                OfferTitle = "TestTitle",
                Price = 500000,
                RoomCount = 6,
                OfferType = EnumHelper.GetDescriptionFromEnum(OfferType.SALE),
                Area = 80,
                Voivodeship = "Podlaskie",
                City = "Białystok",
                Address = "Zwierzyniecka 6",
                Description = "TestDesc",
                RemoteControl = false,
                OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.NEW),
                LastEditedDate = DateTime.Now,
                LandArea = 250,
                TypeOfBuilding = EnumHelper.GetDescriptionFromEnum(TypeOfBuilding.DETACHED_HOUSE),
                FloorCount = 2,
                BuildingMaterial = EnumHelper.GetDescriptionFromEnum(BuildingMaterial.BRICK),
                ConstructionYear = 2022,
                AtticType = EnumHelper.GetDescriptionFromEnum(AtticType.USABLE),
                RoofType = EnumHelper.GetDescriptionFromEnum(RoofType.SLOPING),
                RoofingType = EnumHelper.GetDescriptionFromEnum(RoofingType.CORRUGATED_IRON),
                FinishCondition = EnumHelper.GetDescriptionFromEnum(HouseFinishCondition.READY_TO_MOVE_IN),
                WindowsType = EnumHelper.GetDescriptionFromEnum(WindowType.PLASTIC),
                Location = EnumHelper.GetDescriptionFromEnum(Location.CITY),
                AvailableFromDate = "Now",
                IsARecreationHouse = false,
                AntiBurglaryBlinds = true,
                AntiBurglaryWindowsOrDoors = true,
                IntercomOrVideophone = false,
                MonitoringOrSecurity = false,
                AlarmSystem = false,
                ClosedArea = false,
                BrickFence = false,
                Net = false,
                MetalFence = true,
                WoodenFence = false,
                ConcreteFence = false,
                Hedge = false,
                OtherFencing = false,
                Geothermics = false,
                OilHeating = false,
                ElectricHeating = false,
                DistrictHeating = true,
                TileStoves = false,
                GasHeating = false,
                CoalHeating = true,
                Biomass = false,
                HeatPump = false,
                SolarCollector = false,
                FireplaceHeating = false,
                Internet = true,
                CableTV = true,
                HomePhone = false,
                Water = true,
                Electricity = true,
                SewageSystem = true,
                Gas = true,
                SepticTank = false,
                SewageTreatmentPlant = false,
                FieldDriveway = false,
                PavedDriveway = false,
                AsphaltDriveway = true,
                SwimmingPool = false,
                ParkingSpace = true,
                Basement = true,
                Attic = true,
                Garden = true,
                Furnishings = true,
                AirConditioning = true,
                TypeOfMarket = EnumHelper.GetDescriptionFromEnum(TypeOfMarket.PRIMARY_MARKET),
            };




            conStr = "Server=(localdb)\\MSSQLLocalDB; Database=inzynierka-db; Trusted_Connection=true;";
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(conStr,
                x => x.MigrationsHistoryTable("__EFMigrationHistory", "catalog"));
            context = new DataContext(optionsBuilder.Options);
            context2 = new DataContext(optionsBuilder.Options);
            agencyRepo = new AgencyRepository(context);
            userRepo = new UserRepository(context);
            userRepo2 = new UserRepository(context2);
            

            var myConfiguration = new Dictionary<string, string>
                {
                    {"DefaultConnection", conStr}
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            userFavRepo = new UserFavouriteRepository(context, configuration);
            userFavRepo2 = new UserFavouriteRepository(context2, configuration);
            userEventRepo = new UserEventRepository(context);
            userEventRepo2 = new UserEventRepository(context2);
            offerRepo = new OfferRepository(context, configuration);
            offerRepo2 = new OfferRepository(context2, configuration);
        }
    }
}
