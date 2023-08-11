using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class GarageRentFilteringDTO : OfferFilteringDTO
    {
        public int? minArea { get; set; }
        public int? maxArea { get; set; }
        public GarageRentFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, int? minArea, int? maxArea)
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
        }

        public GarageRentFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
