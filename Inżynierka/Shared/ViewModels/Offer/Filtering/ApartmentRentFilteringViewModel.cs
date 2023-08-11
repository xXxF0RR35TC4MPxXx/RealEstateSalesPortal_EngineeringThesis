
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class ApartmentRentFilteringViewModel : FilteringViewModel
    {
        [Range(1, Int32.MaxValue, ErrorMessage="Wartość nie może być ujemna")]
        public int? minArea { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Wartość nie może być ujemna")]
        public int? maxArea { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Wartość nie może być ujemna")]
        public int? minRoomCount { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Wartość nie może być ujemna")]
        public int? maxRoomCount { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Wartość nie może być ujemna")]
        public int? minFloorCount { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Wartość nie może być ujemna")]
        public int? maxFloorCount { get; set; }

        public int? oldestBuildingYear { get; set; }
        public int? newestBuildingYear { get; set; }

        public bool? AvailableForStudents { get; set; }

        // ======= media =========

        public bool? HasInternet { get; set; }
        public bool? HasCableTV { get; set; }
        public bool? HasHomePhone { get; set; }


        // ======== informacje dodatkowe ========
        public bool? HasBalcony { get; set; }
        public bool? HasUtilityRoom { get; set; }
        public bool? HasParkingSpace { get; set; }
        public bool? HasBasement { get; set; }
        public bool? HasGarden { get; set; }
        public bool? HasTerrace { get; set; }
        public bool? HasElevator { get; set; }
        public bool? HasTwoLevel { get; set; }
        public bool? HasSeparateKitchen { get; set; }
        public bool? HasAirConditioning { get; set; }
    }
}
