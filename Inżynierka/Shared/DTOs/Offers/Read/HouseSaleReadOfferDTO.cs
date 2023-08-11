using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Common.Enums;
using System.ComponentModel;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class HouseSaleReadOfferDTO : ReadOfferDTO
    {
        [Description("Powierzchnia podwórza")]
        [Display(Order = 26)]
        public int? LandArea { get; set; }

        [Description("Typ budynku")]
        [Display(Order = 26)]

        public string? TypeOfBuilding { get; set; } //rodzaj zabudowy

        [Description("Liczba pięter")]
        [Display(Order = 27)]
        public int? FloorCount { get; set; }

        [Description("Materiał budowlany")]
        [Display(Order = 28)]
        public string? BuildingMaterial { get; set; }

        [Description("Rok budowy")]
        [Display(Order = 29)]
        public int? ConstructionYear { get; set; }

        [Description("Typ poddasza")]
        [Display(Order = 30)]
        public string? AtticType { get; set; } //poddasze

        [Description("Typ dachu")]
        [Display(Order = 31)]
        public string? RoofType { get; set; } //dach (kształt)

        [Description("Pokrycie dachu")]
        [Display(Order = 32)]
        public string? RoofingType { get; set; } //pokrycie dachu

        [Description("Stan wykończenia")]
        [Display(Order = 33)]
        public string? FinishCondition { get; set; } //stan wykończenia domu

        [Description("Typ okien")]
        [Display(Order = 34)]
        public string? WindowsType { get; set; } //rodzaj okien

        [Description("Lokalizacja")]
        [Display(Order = 35)]
        public string? Location { get; set; } //położenie

        [Description("Dostępne od")]
        [Display(Order = 36)]
        public string? AvailableFromDate { get; set; }

        [Description("Dom wypoczynkowy")]
        [Display(Order = 37)]
        public bool? IsARecreationHouse { get; set; }


        // ========== zabezpieczenia ===========

        public string GetSafety()
        {
            List<string> single = new();
            if (AntiBurglaryBlinds == true) single.Add("rolety anty-włamaniowe");
            if (AntiBurglaryWindowsOrDoors == true) single.Add("drzwi lub okna anty-włamaniowe");
            if (IntercomOrVideophone == true) single.Add("domofon");
            if (MonitoringOrSecurity == true) single.Add("monitoring");
            if (AlarmSystem == true) single.Add("system alarmowy");
            if (ClosedArea == true) single.Add("teren zamknięty");

            return string.Join(", ", single);
        }
        [Description("Rolety anty-włamaniowe")]
        [Display(Order = 67)]
        public bool? AntiBurglaryBlinds { get; set; }
        [Description("Drzwi lub okna anty-włamaniowe")]
        [Display(Order = 47)]
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        [Description("Domofon")]
        [Display(Order = 48)]
        public bool? IntercomOrVideophone { get; set; }
        [Description("Monitoring")]
        [Display(Order = 49)]
        public bool? MonitoringOrSecurity { get; set; }
        [Description("System alarmowy")]
        [Display(Order = 50)]
        public bool? AlarmSystem { get; set; }
        [Description("Teren zamknięty")]
        [Display(Order = 51)]
        public bool? ClosedArea { get; set; }

        // ======== ogrodzenie ========
        public string GetFencing()
        {
            List<string> single = new();
            if (BrickFence == true) single.Add("murowane");
            if (Net == true) single.Add("siatka");
            if (MetalFence == true) single.Add("metalowe");
            if (WoodenFence == true) single.Add("drewniane");
            if (ConcreteFence == true) single.Add("betonowe");
            if (Hedge == true) single.Add("żywopłot");
            if (FireplaceHeating == true) single.Add("kominkowe");

            return string.Join(", ", single);
        }
        [Description("Płot murowany")]
        [Display(Order = 51)]
        public bool? BrickFence { get; set; }
        [Description("Siatka")]
        [Display(Order = 51)]
        public bool? Net { get; set; }
        [Description("Płot metalowy")]
        [Display(Order = 51)]
        public bool? MetalFence { get; set; }
        [Description("Płot drewniany")]
        [Display(Order = 51)]
        public bool? WoodenFence { get; set; }
        [Description("Płot betonowy")]
        [Display(Order = 51)]
        public bool? ConcreteFence { get; set; }
        [Description("Żywopłot")]
        [Display(Order = 51)]
        public bool? Hedge { get; set; }
        [Description("Inny płot")]
        [Display(Order = 51)]
        public bool? FireplaceHeating { get; set; }


        // ========= ogrzewanie =========

        public string GetHeating()
        {
            List<string> single = new();
            if (Geothermics == true) single.Add("geotermika");
            if (OilHeating == true) single.Add("olejowe");
            if (ElectricHeating == true) single.Add("elektryczne");
            if (DistrictHeating == true) single.Add("miejskie");
            if (TileStoves == true) single.Add("piece kaflowe");
            if (GasHeating == true) single.Add("gazowe");
            if (CoalHeating == true) single.Add("węglowe");
            if (Biomass == true) single.Add("biomasa");
            if (HeatPump == true) single.Add("pompa ciepła");
            if (SolarCollector == true) single.Add("kolektor słoneczny");
            if (OtherHeating == true) single.Add("inne");

            return string.Join(", ", single);
        }

        [Description("Geotermika")]
        [Display(Order = 35)]
        public bool? Geothermics { get; set; }
        [Description("Olejowe")]
        [Display(Order = 35)]
        public bool? OilHeating { get; set; }
        [Description("Elektryczne")]
        [Display(Order = 35)]
        public bool? ElectricHeating { get; set; }
        [Description("Miejskie")]
        [Display(Order = 35)]
        public bool? DistrictHeating { get; set; }
        [Description("Piece kaflowe")]
        [Display(Order = 35)]
        public bool? TileStoves { get; set; }
        [Description("Gazowe")]
        [Display(Order = 35)]
        public bool? GasHeating { get; set; }
        [Description("Węglowe")]
        [Display(Order = 35)]
        public bool? CoalHeating { get; set; }
        [Description("Biomasa")]
        [Display(Order = 35)]
        public bool? Biomass { get; set; }
        [Description("Pompa ciepła")]
        [Display(Order = 35)]
        public bool? HeatPump { get; set; }
        [Description("Kolektor słoneczny")]
        [Display(Order = 35)]
        public bool? SolarCollector { get; set; }
        [Description("Kominkowe")]
        [Display(Order = 35)]
        public bool? OtherHeating { get; set; }


        // ======== media =========

        public string GetMedia()
        {
            List<string> single = new();
            if (Internet == true) single.Add("internet");
            if (CableTV == true) single.Add("telewizja kablowa");
            if (HomePhone == true) single.Add("telefon stacjonarny");
            if (Water == true) single.Add("woda");
            if (Electricity == true) single.Add("prąd");
            if (SewageSystem == true) single.Add("kanalizacja");
            if (Gas == true) single.Add("gaz");
            if (SepticTank == true) single.Add("szambo");
            if (SewageTreatmentPlant == true) single.Add("oczyszczalnia ścieków");

            return string.Join(", ", single);
        }

        [Description("Internet")]
        [Display(Order = 35)]
        public bool? Internet { get; set; }

        [Description("Telewizja kablowa")]
        [Display(Order = 36)]
        public bool? CableTV { get; set; }

        [Description("Telefon")]
        [Display(Order = 37)]
        public bool? HomePhone { get; set; }
        [Description("Woda")]
        [Display(Order = 38)]
        public bool? Water { get; set; }
        [Description("Prąd")]
        [Display(Order = 39)]
        public bool? Electricity { get; set; }
        [Description("Kanalizacja")]
        [Display(Order = 40)]
        public bool? SewageSystem { get; set; }
        [Description("Gaz")]
        [Display(Order = 41)]
        public bool? Gas { get; set; }
        [Description("Szambo")]
        [Display(Order = 42)]
        public bool? SepticTank { get; set; }
        [Description("Oczyszczalnia ścieków")]
        [Display(Order = 43)]
        public bool? SewageTreatmentPlant { get; set; }



        public string GetDriveway()
        {
            List<string> single = new();
            if (FieldDriveway == true) single.Add("nieutwardzony");
            if (PavedDriveway == true) single.Add("utwardzony");
            if (AsphaltDriveway == true) single.Add("asfaltowy");

            return string.Join(", ", single);
        }

        [Description("Dojazd nieutwardzony")]
        [Display(Order = 44)]
        public bool? FieldDriveway { get; set; }
        [Description("Dojazd utwardzony")]
        [Display(Order = 45)]
        public bool? PavedDriveway { get; set; }
        [Description("Dojazd asfaltowy")]
        [Display(Order = 46)]
        public bool? AsphaltDriveway { get; set; }

        public string GetAdditionalInfo()
        {
            List<string> single = new();
            if (ParkingSpace == true) single.Add("miejsce parkingowe");
            if (SwimmingPool == true) single.Add("basen");
            if (AirConditioning == true) single.Add("klimatyzacja");
            if (Furnishings == true) single.Add("umeblowanie");
            if (Basement == true) single.Add("piwnica");
            if (Garden == true) single.Add("ogród");
            if (Attic == true) single.Add("poddasze");

            return string.Join(", ", single);
        }

        [Description("Basen")]
        [Display(Order = 46)]
        public bool? SwimmingPool { get; set; }
        [Description("Miejsce parkingowe")]
        [Display(Order = 46)]
        public bool? ParkingSpace { get; set; }
        [Description("Piwnica")]
        [Display(Order = 46)]
        public bool? Basement { get; set; }
        [Description("Poddasze")]
        [Display(Order = 46)]
        public bool? Attic { get; set; }
        [Description("Ogród")]
        [Display(Order = 46)]
        public bool? Garden { get; set; }
        [Description("Umeblowanie")]
        [Display(Order = 46)]
        public bool? Furnishings { get; set; }
        [Description("Klimatyzacja")]
        [Display(Order = 46)]
        public bool? AirConditioning { get; set; }

        [Description("Rodzaj rynku")]
        [Display(Order = 99)]
        public string? TypeOfMarket { get; set; }

        public HouseSaleReadOfferDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            int? landArea, string? typeOfBuilding, int? floorCount, string? buildingMaterial,
            int? constructionYear, string? atticType, string? roofType, string? roofingType, string? finishCondition,
            string? windowsType, string? location, string? availableFromDate, bool? isARecreationHouse, bool? antiBurglaryBlinds,
            bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone, bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea,
            bool? brickFence, bool? net, bool? metalFence, bool? woodenFence, bool? concreteFence, bool? hedge, bool? otherFencing,
            bool? geothermics, bool? oilHeating, bool? electricHeating, bool? districtHeating, bool? tileStoves, bool? gasHeating,
            bool? coalHeating, bool? biomass, bool? heatPump, bool? solarCollector, bool? otherHeating, bool? internet, bool? cableTV,
            bool? homePhone, bool? water, bool? electricity, bool? sewageSystem, bool? gas, bool? septicTank, bool? sewageTreatmentPlant,
            bool? fieldDriveway, bool? pavedDriveway, bool? asphaltDriveway, bool? swimmingPool, bool? parkingSpace, bool? basement,
            bool? attic, bool? garden, bool? furnishings, bool? airConditioning, string? typeOfMarket)
        {
            OfferTitle = offerTitle;
            Price = price;
            SellerType = sellerType;
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
            FireplaceHeating = otherFencing;
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
            OtherHeating = otherHeating;
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

        public HouseSaleReadOfferDTO()
        {
        }
    }
}
