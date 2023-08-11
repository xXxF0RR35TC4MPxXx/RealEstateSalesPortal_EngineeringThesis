using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class GarageSaleFilteringDTO : OfferFilteringDTO
    {
        public TypesOfMarketInSearch typesOfMarketInSearch { get; set; }
        public UserRolesInSearch userRolesInSearch { get; set; }

        public int? minArea { get; set; }
        public int? maxArea { get; set; }
        public GarageSaleFilteringDTO(string? city, int? minPrice, int? maxPrice, HowRecent? howRecent, 
            int? offerId, string? descriptionFragment, bool remoteControl, TypesOfMarketInSearch typesOfMarketInSearch, 
            UserRolesInSearch userRolesInSearch, int? minArea, int? maxArea)
        {
            City = city;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            HowRecent = howRecent;
            OfferId = offerId;
            DescriptionFragment = descriptionFragment;
            RemoteControl = remoteControl;

            this.typesOfMarketInSearch = typesOfMarketInSearch;
            this.userRolesInSearch = userRolesInSearch;
            this.minArea = minArea;
            this.maxArea = maxArea;
        }

        public GarageSaleFilteringDTO()
        {
            RemoteControl = true;
        }
    }
}
