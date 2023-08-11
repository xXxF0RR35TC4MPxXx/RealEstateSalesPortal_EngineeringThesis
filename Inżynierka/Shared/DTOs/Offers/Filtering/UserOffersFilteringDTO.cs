using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class UserOffersFilteringDTO : OfferFilteringDTO
    {

        public bool? hasPhotos { get; set; }
        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minPricePerMeterSquared { get; set; }
        public int? maxPricePerMeterSquared { get; set; }

        public AvailableOfferTypes availableOfferTypes { get; set; }
        public AvailableEstateType availableEstateType { get; set; }

        public UserOffersFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent,
            int? offerId, string? descriptionFragment, bool remoteControl, 
            bool? hasPhotos, int? minArea, int? maxArea, int? minPricePerMeterSquared, int? maxPricePerMeterSquared, 
            AvailableOfferTypes availableOfferTypes, AvailableEstateType availableEstateType)
        {
            City = city;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            HowRecent = howRecent;
            OfferId = offerId;
            DescriptionFragment = descriptionFragment;
            RemoteControl = remoteControl;

            this.hasPhotos = hasPhotos;
            this.minArea = minArea;
            this.maxArea = maxArea;
            this.minPricePerMeterSquared = minPricePerMeterSquared;
            this.maxPricePerMeterSquared = maxPricePerMeterSquared;
            this.availableOfferTypes = availableOfferTypes;
            this.availableEstateType = availableEstateType;
        }

        public UserOffersFilteringDTO()
        {

        }
    }
}
