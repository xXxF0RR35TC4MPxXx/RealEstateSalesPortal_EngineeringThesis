
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class HouseSaleFilteringViewModel : FilteringViewModel
    {
        public TypesOfMarketInSearch typesOfMarketInSearch { get; set; }
        public UserRolesInSearch userRolesInSearch { get; set; }

        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }

        public int? minRoomCount { get; set; }
        public int? maxRoomCount { get; set; }

        public int? minPlotArea { get; set; }
        public int? maxPlotArea { get; set; }

        public IList<TypeOfBuilding>? availableTypesOfBuilding { get; set; }
        public IList<BuildingMaterial>? availableBuildingMaterials { get; set; }
        public IList<RoofType>? availableRoofTypes { get; set; }

        public int? oldestBuildingYear { get; set; }
        public int? newestBuildingYear { get; set; }
    }
}
