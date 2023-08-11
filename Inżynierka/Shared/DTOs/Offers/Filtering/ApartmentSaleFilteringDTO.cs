using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class ApartmentSaleFilteringDTO : OfferFilteringDTO
    {
        public TypesOfMarketInSearch typesOfMarketInSearch { get; set; }
        public UserRolesInSearch userRolesInSearch { get; set; }

        public int? minArea { get; set; }
        public int? maxArea { get; set; }


        public int? minRoomCount { get; set; }
        public int? maxRoomCount { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }


        public IList<TypeOfBuilding>? availableTypesOfBuilding { get; set; }
        public IList<BuildingMaterial>? availableBuildingMaterials { get; set; }
        public IList<Floor>? availableFloors { get; set; }

        public int? minFloorCount { get; set; }
        public int? maxFloorCount { get; set; }


        public int? oldestBuildingYear { get; set; }
        public int? newestBuildingYear { get; set; }


        //========== info dodatkowe ============
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

        public ApartmentSaleFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, TypesOfMarketInSearch typesOfMarketInSearch, UserRolesInSearch userRolesInSearch, 
            int? minArea, int? maxArea, int? minRoomCount, int? maxRoomCount, int? minPricePerMeterSquared, int? maxPricePerMeterSquared, 
            IList<TypeOfBuilding>? availableTypesOfBuilding, IList<BuildingMaterial>? availableBuildingMaterials, IList<Floor>? availableFloors, 
            int? minFloorCount, int? maxFloorCount, int? oldestBuildingYear, int? newestBuildingYear, bool? hasBalcony, bool? hasUtilityRoom, 
            bool? hasParkingSpace, bool? hasBasement, bool? hasGarden, bool? hasTerrace, bool? hasElevator, bool? hasTwoLevel, bool? hasSeparateKitchen, 
            bool? hasAirConditioning)
        {
            City = city;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            HowRecent = howRecent;
            OfferId = offerId;
            DescriptionFragment = descriptionFragment;
            RemoteControl = remoteControl;
            this.typesOfMarketInSearch = typesOfMarketInSearch;
            this.userRolesInSearch = userRolesInSearch;
            this.minArea = minArea;
            this.maxArea = maxArea;
            this.minRoomCount = minRoomCount;
            this.maxRoomCount = maxRoomCount;
            this.minPricePerMeterSquared = minPricePerMeterSquared;
            this.maxPricePerMeterSquared = maxPricePerMeterSquared;
            this.availableTypesOfBuilding = availableTypesOfBuilding;
            this.availableBuildingMaterials = availableBuildingMaterials;
            this.availableFloors = availableFloors;
            this.minFloorCount = minFloorCount;
            this.maxFloorCount = maxFloorCount;
            this.oldestBuildingYear = oldestBuildingYear;
            this.newestBuildingYear = newestBuildingYear;
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

        public ApartmentSaleFilteringDTO()
        {
            RemoteControl = true;
            typesOfMarketInSearch = TypesOfMarketInSearch.BOTH;
            userRolesInSearch = UserRolesInSearch.ALL;
        }
    }
}
