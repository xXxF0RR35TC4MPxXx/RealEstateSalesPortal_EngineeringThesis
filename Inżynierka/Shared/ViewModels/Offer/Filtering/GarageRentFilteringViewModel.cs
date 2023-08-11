
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class GarageRentFilteringViewModel : FilteringViewModel
    {        
        public int? minArea { get; set; }
        public int? maxArea { get; set; }
    }
}
