using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class AgencyPageFilteringViewModel : FilteringViewModel
    {
        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }

        public AvailableOfferTypes availableOfferTypes { get; set; }
        public AvailableEstateType availableEstateType { get; set; }
    }
}
