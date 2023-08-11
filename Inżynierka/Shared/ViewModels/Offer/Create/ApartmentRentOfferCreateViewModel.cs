using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Create
{
    public class ApartmentRentOfferCreateViewModel : OfferCreateViewModel
    {
        public TypeOfBuilding? TypeOfBuilding { get; set; } //rodzaj budynku
        public Floor? Floor { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Liczba pięter nie może być mniejsza od 1!")]
        public int? FloorCount { get; set; }

        public BuildingMaterial? BuildingMaterial { get; set; }

        public WindowType? WindowsType { get; set; }

        public ApartmentHeating? HeatingType { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Kwota nie może być ujemna!")]
        public int? Rent { get; set; } //kaucja

        public string? AvailableSinceDate { get; set; }

        public int? YearOfConstruction { get; set; }

        public ApartmentFinishCondition? FinishCondition { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Liczba pomieszczeń nie może być mniejsza od 1!")]
        public int? RoomCount { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; } //powierzchnia domu

        // ======= media =========

        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }


        // ======== informacje dodatkowe ========

        public bool? Balcony { get; set; }
        public bool? UtilityRoom { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? Basement { get; set; }
        public bool? Garden { get; set; }
        public bool? Terrace { get; set; }
        public bool? Elevator { get; set; }
        public bool? TwoLevel { get; set; }
        public bool? SeparateKitchen { get; set; }
        public bool? AirConditioning { get; set; }


        public bool? AvailableForStudents { get; set; }
        public bool? OnlyForNonsmoking { get; set; }



        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }



        // ======= wyposażenie =======
        public bool? Furniture { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? Dishwasher { get; set; }
        public bool? Fridge { get; set; }
        public bool? Stove { get; set; }
        public bool? Oven { get; set; }
        public bool? TV { get; set; }
    }
}
