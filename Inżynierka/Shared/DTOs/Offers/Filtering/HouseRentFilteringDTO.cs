using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class HouseRentFilteringDTO : OfferFilteringDTO
    {

        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minRoomCount { get; set; }
        public int? maxRoomCount { get; set; }

        public int? minPlotArea { get; set; }
        public int? maxPlotArea { get; set; }

        public IList<TypeOfBuilding>? availableTypesOfBuilding { get; set; }

        public int? oldestBuildingYear { get; set; }
        public int? newestBuildingYear { get; set; }


        public bool? HasInternet { get; set; }
        public bool? HasCableTV { get; set; }
        public bool? HasHomePhone { get; set; }
        public bool? HasWater { get; set; }
        public bool? HasElectricity { get; set; }
        public bool? HasSewageSystem { get; set; }
        public bool? HasGas { get; set; }
        public bool? HasSepticTank { get; set; }
        public bool? HasSewageTreatmentPlant { get; set; }
        public HouseRentFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, int? minArea, int? maxArea, int? minRoomCount, 
            int? maxRoomCount, int? minPlotArea, int? maxPlotArea, IList<TypeOfBuilding>? availableTypesOfBuilding, int? oldestBuildingYear,
            int? newestBuildingYear, bool? hasInternet, bool? hasCableTV, bool? hasHomePhone, bool? hasWater,
            bool? hasElectricity, bool? hasSewageSystem, bool? hasGas, bool? hasSepticTank, bool? hasSewageTreatmentPlant)
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
            this.minPlotArea = minPlotArea;
            this.maxPlotArea = maxPlotArea;
            this.availableTypesOfBuilding = availableTypesOfBuilding;
            this.oldestBuildingYear = oldestBuildingYear;
            this.newestBuildingYear = newestBuildingYear;
            HasInternet = hasInternet;
            HasCableTV = hasCableTV;
            HasHomePhone = hasHomePhone;
            HasWater = hasWater;
            HasElectricity = hasElectricity;
            HasSewageSystem = hasSewageSystem;
            HasGas = hasGas;
            HasSepticTank = hasSepticTank;
            HasSewageTreatmentPlant = hasSewageTreatmentPlant;
        }
        public HouseRentFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
