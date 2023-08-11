
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class PlotOfferFilteringViewModel : FilteringViewModel
    {
        public AvailableOfferTypes? availableOfferTypes { get; set; }

        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }

        public IList<PlotType>? availablePlotTypes { get; set; }

        // ========== okolica ==========
        public bool? NearForest { get; set; }
        public bool? NearLake { get; set; }
        public bool? NearMountains { get; set; }
        public bool? NearSea { get; set; }
        public bool? NearOpenArea { get; set; }
    }
}
