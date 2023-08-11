using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class ApartmentRentFilteringDTO : OfferFilteringDTO
    {
        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minRoomCount { get; set; }
        public int? maxRoomCount { get; set; }

        public int? minFloorCount { get; set; }
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
        public ApartmentRentFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, int? minArea, int? maxArea, int? minRoomCount, 
            int? maxRoomCount, int? minFloorCount, 
            int? maxFloorCount, int? oldestBuildingYear, int? newestBuildingYear, 
            bool? availableForStudents, bool? hasInternet, bool? hasCableTV, bool? hasHomePhone, bool? hasBalcony, bool? hasUtilityRoom, 
            bool? hasParkingSpace, bool? hasBasement, bool? hasGarden, 
            bool? hasTerrace, bool? hasElevator, bool? hasTwoLevel, bool? hasSeparateKitchen, bool? hasAirConditioning)
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
            this.minRoomCount = minRoomCount;
            this.maxRoomCount = maxRoomCount;
            this.minFloorCount = minFloorCount;
            this.maxFloorCount = maxFloorCount;
            this.oldestBuildingYear = oldestBuildingYear;
            this.newestBuildingYear = newestBuildingYear;
            AvailableForStudents = availableForStudents;
            HasInternet = hasInternet;
            HasCableTV = hasCableTV;
            HasHomePhone = hasHomePhone;
            HasBalcony = hasBalcony;
            HasUtilityRoom = hasUtilityRoom;
            HasParkingSpace = hasParkingSpace;
            HasBasement = hasBasement;
            HasGarden = hasGarden;
            HasTerrace = hasTerrace;
            HasElevator = hasElevator;
            HasTwoLevel = hasTwoLevel;
            HasSeparateKitchen = hasSeparateKitchen;
            HasAirConditioning = hasAirConditioning;
        }

        public ApartmentRentFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
