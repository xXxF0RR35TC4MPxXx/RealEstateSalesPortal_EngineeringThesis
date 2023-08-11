using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class PlotOfferFilteringDTO : OfferFilteringDTO
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

        public PlotOfferFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, AvailableOfferTypes? availableOfferTypes, int? minArea, 
            int? maxArea, int? minPricePerMeterSquared, int? maxPricePerMeterSquared, IList<PlotType>? availablePlotTypes, bool? nearForest, 
            bool? nearLake, bool? nearMountains, bool? nearSea, bool? nearOpenArea)
        {
            City = city;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            HowRecent = howRecent;
            OfferId = offerId;
            DescriptionFragment = descriptionFragment;
            RemoteControl = remoteControl;

            this.availableOfferTypes = availableOfferTypes;
            this.minArea = minArea;
            this.maxArea = maxArea;
            this.minPricePerMeterSquared = minPricePerMeterSquared;
            this.maxPricePerMeterSquared = maxPricePerMeterSquared;
            this.availablePlotTypes = availablePlotTypes;
            NearForest = nearForest;
            NearLake = nearLake;
            NearMountains = nearMountains;
            NearSea = nearSea;
            NearOpenArea = nearOpenArea;
        }

        public PlotOfferFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
