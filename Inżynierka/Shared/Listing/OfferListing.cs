using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka_Common.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Listing
{
    public class OfferListing
    {
        public int TotalCount { get; set; }
        public IEnumerable<OfferListThumbnailDTO>? OfferDTOs { set; get; }
        public OfferFilteringDTO OfferFilteringDTO { set; get; }
        public SortOrder SortOrder { get; set; }
        public Paging Paging { set; get; }
    }
}
