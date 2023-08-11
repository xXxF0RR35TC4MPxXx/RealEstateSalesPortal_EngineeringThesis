
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class HallSaleFilteringViewModel : FilteringViewModel
    {
        public TypesOfMarketInSearch typesOfMarketInSearch { get; set; }
        public UserRolesInSearch userRolesInSearch { get; set; }

        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }

        public int? minHeight { get; set; }
        public int? maxHeight { get; set; }

        public HallConstruction? HallConstruction { get; set; }
        public bool? Heating { get; set; }

        public bool? IsStorage { get; set; }
        public bool? IsProduction { get; set; }
        public bool? IsOffice { get; set; }
        public bool? IsCommercial { get; set; }


    }
}
