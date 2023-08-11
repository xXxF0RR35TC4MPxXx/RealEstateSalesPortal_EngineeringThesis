using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Update
{
    public class RoomRentingOfferUpdateViewModel : OfferUpdateViewModel
    {
        [Range(0, Int32.MaxValue, ErrorMessage = "Kwota nie może być ujemna!")]
        public int? AdditionalFees { get; set; } //opłaty dodatkowe

        [Range(0, Int32.MaxValue, ErrorMessage = "Kwota nie może być ujemna")]
        public int? Deposit { get; set; } //kaucja

        [Range(0, Int32.MaxValue, ErrorMessage = "Liczba nie może być ujemna")]
        public int? NumberOfPeopleInTheRoom { get; set; }
        public Floor? Floor { get; set; }

        public TypeOfBuilding? TypeOfBuilding { get; set; } //rodzaj zabudowy

        public string? AvailableFromDate { get; set; }

        public bool? AvailableForStudents { get; set; }

        public bool? OnlyForNonsmoking { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; }

        // ======= wyposażenie =======
        public bool? Furniture { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? Dishwasher { get; set; }
        public bool? Fridge { get; set; }
        public bool? Stove { get; set; }
        public bool? Oven { get; set; }
        public bool? TV { get; set; }

        // ======= media ========
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }
    }
}
