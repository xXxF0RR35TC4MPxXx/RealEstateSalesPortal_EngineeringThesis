using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class HouseSaleFilteringDTO : OfferFilteringDTO
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
        public HouseSaleFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, TypesOfMarketInSearch typesOfMarketInSearch, 
            UserRolesInSearch userRolesInSearch, int? minArea, int? maxArea, int? minPricePerMeterSquared, int? maxPricePerMeterSquared,
            int? minRoomCount, int? maxRoomCount, int? minPlotArea, int? maxPlotArea, IList<TypeOfBuilding>? availableTypesOfBuilding, 
            IList<BuildingMaterial>? availableBuildingMaterials, 
            IList<RoofType>? availableRoofTypes, int? oldestBuildingYear, int? newestBuildingYear)
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
            this.minPricePerMeterSquared = minPricePerMeterSquared;
            this.maxPricePerMeterSquared = maxPricePerMeterSquared;
            this.minRoomCount = minRoomCount;
            this.maxRoomCount = maxRoomCount;
            this.minPlotArea = minPlotArea;
            this.maxPlotArea = maxPlotArea;
            this.availableTypesOfBuilding = availableTypesOfBuilding;
            this.availableBuildingMaterials = availableBuildingMaterials;
            this.availableRoofTypes = availableRoofTypes;
            this.oldestBuildingYear = oldestBuildingYear;
            this.newestBuildingYear = newestBuildingYear;
        }

        public HouseSaleFilteringDTO()
        {
            RemoteControl = true;
            typesOfMarketInSearch = TypesOfMarketInSearch.BOTH;
            userRolesInSearch = UserRolesInSearch.ALL;
        }
    }
}