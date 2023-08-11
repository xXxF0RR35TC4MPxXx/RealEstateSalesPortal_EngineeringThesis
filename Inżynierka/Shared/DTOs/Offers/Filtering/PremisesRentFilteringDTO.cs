using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class PremisesRentFilteringDTO : OfferFilteringDTO
    {
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
        public PremisesRentFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, int? minArea, int? maxArea, int? minPricePerMeterSquared, 
            int? maxPricePerMeterSquared, int? minRoomCount, int? maxRoomCount, IList<Floor>? acceptableFloors, bool? isService, bool? isGastronomic,
            bool? isOffice, bool? isIndustrial, bool? isCommercial, bool? isHotel, bool? hasShopwindow, bool? hasParkingSpace, 
            bool? hasAsphaltDriveway, bool? hasHeating, bool? hasElevator, bool? hasFurnishings, bool? hasAirConditioning)
        {
            City = city;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            HowRecent = howRecent;
            OfferId = offerId;
            DescriptionFragment = descriptionFragment;
            RemoteControl = remoteControl;

            this.minArea = minArea;
            this.maxArea = maxArea;
            this.minPricePerMeterSquared = minPricePerMeterSquared;
            this.maxPricePerMeterSquared = maxPricePerMeterSquared;
            this.minRoomCount = minRoomCount;
            this.maxRoomCount = maxRoomCount;
            this.acceptableFloors = acceptableFloors;
            IsService = isService;
            IsGastronomic = isGastronomic;
            IsOffice = isOffice;
            IsIndustrial = isIndustrial;
            IsCommercial = isCommercial;
            IsHotel = isHotel;
            HasShopwindow = hasShopwindow;
            HasParkingSpace = hasParkingSpace;
            HasAsphaltDriveway = hasAsphaltDriveway;
            HasHeating = hasHeating;
            HasElevator = hasElevator;
            HasFurnishings = hasFurnishings;
            HasAirConditioning = hasAirConditioning;
        }

        public PremisesRentFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
