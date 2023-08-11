using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka_Common.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Listing
{
    public class UserFavouriteListing
    {
        public int TotalCount { get; set; }
        public IEnumerable<OfferListThumbnailDTO> FavouritesDTOs { set; get; }
        public Paging Paging { set; get; }
    }
}
