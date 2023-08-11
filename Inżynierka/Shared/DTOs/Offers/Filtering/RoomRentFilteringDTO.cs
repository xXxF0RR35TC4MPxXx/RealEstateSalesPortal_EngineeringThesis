using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class RoomRentFilteringDTO : OfferFilteringDTO
    {
        public int? RoommateCount { get; set; }
        public bool? HasInternet { get; set; }
        public bool? HasPhone { get; set; }
        public bool? HasCableTV { get; set; }

        public RoomRentFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl,
            int? roommateCount, bool? hasInternet, bool? hasPhone, bool? hasCableTV)
        {
            City = city;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            HowRecent = howRecent;
            OfferId = offerId;
            DescriptionFragment = descriptionFragment;
            RemoteControl = remoteControl;

            RoommateCount = roommateCount;
            HasInternet = hasInternet;
            HasPhone = hasPhone;
            HasCableTV = hasCableTV;
        }

        public RoomRentFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
