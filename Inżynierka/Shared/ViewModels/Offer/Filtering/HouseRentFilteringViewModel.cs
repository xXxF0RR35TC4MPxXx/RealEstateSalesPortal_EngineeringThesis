
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class HouseRentFilteringViewModel : FilteringViewModel
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
    }
}
