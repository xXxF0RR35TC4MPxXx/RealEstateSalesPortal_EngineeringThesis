using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class ApartmentSaleFilteringViewModel : FilteringViewModel
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
    }
}
