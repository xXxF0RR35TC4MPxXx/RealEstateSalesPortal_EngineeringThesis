using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Update
{
    public class HouseSaleOfferUpdateViewModel : OfferUpdateViewModel
    {
        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? LandArea { get; set; }

        public TypeOfBuilding? TypeOfBuilding { get; set; } //rodzaj zabudowy

        [Range(1, Int32.MaxValue, ErrorMessage = "Liczba nie może być ujemna")]
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

        [Range(1, Int32.MaxValue, ErrorMessage = "Liczba pomieszczeń nie może być ujemna")]
        public int? RoomCount { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; } //powierzchnia domu

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
    }
}
