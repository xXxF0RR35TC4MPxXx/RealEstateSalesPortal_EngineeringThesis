
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class PremisesSaleFilteringViewModel : FilteringViewModel
    {
        public TypesOfMarketInSearch typesOfMarketInSearch { get; set; }
        public UserRolesInSearch userRolesInSearch { get; set; }

        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }

        public int? minRoomCount { get; set; }
        public int? maxRoomCount { get; set; }

        public IList<Floor>? acceptableFloors { get; set; }

        public bool? IsService { get; set; }
        public bool? IsGastronomic { get; set; }
        public bool? IsOffice { get; set; }
        public bool? IsIndustrial { get; set; }
        public bool? IsCommercial { get; set; }
        public bool? IsHotel { get; set; }

        public bool? HasShopwindow { get; set; }
        public bool? HasParkingSpace { get; set; }
        public bool? HasAsphaltDriveway { get; set; }
        public bool? HasHeating { get; set; }
        public bool? HasElevator { get; set; }
        public bool? HasFurnishings { get; set; }
        public bool? HasAirConditioning { get; set; }
    }
}
