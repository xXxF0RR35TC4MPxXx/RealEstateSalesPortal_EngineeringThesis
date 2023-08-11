using Inżynierka_Common.Enums;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class HallRentFilteringDTO : OfferFilteringDTO
    {

        public int? minArea { get; set; }
        public int? maxArea { get; set; }

        public int? minHeight { get; set; }
        public int? maxHeight { get; set; }

        public HallConstruction? HallConstruction { get; set; }
        public bool? Heating { get; set; }

        public bool? IsStorage { get; set; }
        public bool? IsProduction { get; set; }
        public bool? IsOffice { get; set; }
        public bool? IsCommercial { get; set; }
        public HallRentFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, int? minArea, int? maxArea, int? minHeight, int? maxHeight, HallConstruction? hallConstruction, 
            bool? heating, bool? isStorage, bool? isProduction, bool? isOffice, bool? isCommercial)
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
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            HallConstruction = hallConstruction;
            Heating = heating;
            IsStorage = isStorage;
            IsProduction = isProduction;
            IsOffice = isOffice;
            IsCommercial = isCommercial;
        }

        public HallRentFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
