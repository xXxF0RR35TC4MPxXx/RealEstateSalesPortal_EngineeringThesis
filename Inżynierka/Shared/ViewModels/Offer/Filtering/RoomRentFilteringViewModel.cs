
using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class RoomRentFilteringViewModel : FilteringViewModel
    {
        public int? RoommateCount { get; set; }
        public bool? HasInternet { get; set; }
        public bool? HasPhone { get; set; }
        public bool? HasCableTV { get; set; }
    }
}
