using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Update
{
    public class HouseSaleOfferUpdateDTO : OfferUpdateDTO
    {
        public int? LandArea { get; set; }

        public TypeOfBuilding? TypeOfBuilding { get; set; } //rodzaj zabudowy

        public int? FloorCount { get; set; }

        public BuildingMaterial? BuildingMaterial { get; set; }

        public int? ConstructionYear { get; set; }

        public AtticType? AtticType { get; set; } //poddasze

        public RoofType? RoofType { get; set; } //dach (kształt)

        public RoofingType? RoofingType { get; set; } //pokrycie dachu

        public HouseFinishCondition? FinishCondition { get; set; } //stan wykończenia domu

        public WindowType? WindowsType { get; set; } //rodzaj okien

        public Location? Location { get; set; } //położenie

        public string? AvailableFromDate { get; set; }

        public bool? IsARecreationHouse { get; set; }


        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }


        // ======== ogrodzenie ========
        public bool? BrickFence { get; set; }
        public bool? Net { get; set; }
        public bool? MetalFence { get; set; }
        public bool? WoodenFence { get; set; }
        public bool? ConcreteFence { get; set; }
        public bool? Hedge { get; set; }
        public bool? OtherFencing { get; set; }

        public string OfferType { get; set; } //sale or rent

        // ========= ogrzewanie =========
        public bool? Geothermics { get; set; }
        public bool? OilHeating { get; set; }
        public bool? ElectricHeating { get; set; }
        public bool? DistrictHeating { get; set; }
        public bool? TileStoves { get; set; }
        public bool? GasHeating { get; set; }
        public bool? CoalHeating { get; set; }
        public bool? Biomass { get; set; }
        public bool? HeatPump { get; set; }
        public bool? SolarCollector { get; set; }
        public bool? FireplaceHeating { get; set; }


        // ======== media =========
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }
        public bool? Water { get; set; }
        public bool? Electricity { get; set; }
        public bool? SewageSystem { get; set; }
        public bool? Gas { get; set; }
        public bool? SepticTank { get; set; }
        public bool? SewageTreatmentPlant { get; set; }



        // ======== dojazd =========
        public bool? FieldDriveway { get; set; }
        public bool? PavedDriveway { get; set; }
        public bool? AsphaltDriveway { get; set; }

        // ======== informacje dodatkowe =========
        public bool? SwimmingPool { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? Basement { get; set; }
        public bool? Attic { get; set; }
        public bool? Garden { get; set; }
        public bool? Furnishings { get; set; }
        public bool? AirConditioning { get; set; }

        public TypeOfMarket? TypeOfMarket { get; set; }

        public HouseSaleOfferUpdateDTO(string offerTitle, int price,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            int? landArea, TypeOfBuilding? typeOfBuilding, int? floorCount, BuildingMaterial? buildingMaterial,
            int? constructionYear, AtticType? atticType, RoofType? roofType, RoofingType? roofingType, HouseFinishCondition? finishCondition,
            WindowType? windowsType, Location? location, string? availableFromDate, bool? isARecreationHouse, bool? antiBurglaryBlinds,
            bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone, bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea,
            bool? brickFence, bool? net, bool? metalFence, bool? woodenFence, bool? concreteFence, bool? hedge, bool? otherFencing,
            bool? geothermics, bool? oilHeating, bool? electricHeating, bool? districtHeating, bool? tileStoves, bool? gasHeating,
            bool? coalHeating, bool? biomass, bool? heatPump, bool? solarCollector, bool? otherHeating, bool? internet, bool? cableTV,
            bool? homePhone, bool? water, bool? electricity, bool? sewageSystem, bool? gas, bool? septicTank, bool? sewageTreatmentPlant,
            bool? fieldDriveway, bool? pavedDriveway, bool? asphaltDriveway, bool? swimmingPool, bool? parkingSpace, bool? basement,
            bool? attic, bool? garden, bool? furnishings, bool? airConditioning, TypeOfMarket? typeOfMarket)
        {
            OfferTitle = offerTitle;
            Price = price;
            Voivodeship = voivodeship;
            City = city;
            Address = address;
            Description = description;
            RemoteControl = remoteControl;
            OfferStatus = offerStatus;

            LandArea = landArea;
            TypeOfBuilding = typeOfBuilding;
            FloorCount = floorCount;
            BuildingMaterial = buildingMaterial;
            ConstructionYear = constructionYear;
            AtticType = atticType;
            RoofType = roofType;
            RoofingType = roofingType;
            FinishCondition = finishCondition;
            WindowsType = windowsType;
            Location = location;
            AvailableFromDate = availableFromDate;
            IsARecreationHouse = isARecreationHouse;
            AntiBurglaryBlinds = antiBurglaryBlinds;
            AntiBurglaryWindowsOrDoors = antiBurglaryWindowsOrDoors;
            IntercomOrVideophone = intercomOrVideophone;
            MonitoringOrSecurity = monitoringOrSecurity;
            AlarmSystem = alarmSystem;
            ClosedArea = closedArea;
            BrickFence = brickFence;
            Net = net;
            MetalFence = metalFence;
            WoodenFence = woodenFence;
            ConcreteFence = concreteFence;
            Hedge = hedge;
            OtherFencing = otherFencing;
            Geothermics = geothermics;
            OilHeating = oilHeating;
            ElectricHeating = electricHeating;
            DistrictHeating = districtHeating;
            TileStoves = tileStoves;
            GasHeating = gasHeating;
            CoalHeating = coalHeating;
            Biomass = biomass;
            HeatPump = heatPump;
            SolarCollector = solarCollector;
            FireplaceHeating = otherHeating;
            Internet = internet;
            CableTV = cableTV;
            HomePhone = homePhone;
            Water = water;
            Electricity = electricity;
            SewageSystem = sewageSystem;
            Gas = gas;
            SepticTank = septicTank;
            SewageTreatmentPlant = sewageTreatmentPlant;
            FieldDriveway = fieldDriveway;
            PavedDriveway = pavedDriveway;
            AsphaltDriveway = asphaltDriveway;
            SwimmingPool = swimmingPool;
            ParkingSpace = parkingSpace;
            Basement = basement;
            Attic = attic;
            Garden = garden;
            Furnishings = furnishings;
            AirConditioning = airConditioning;
            TypeOfMarket = typeOfMarket;
        }

        public HouseSaleOfferUpdateDTO()
        {
        }
    }
}
