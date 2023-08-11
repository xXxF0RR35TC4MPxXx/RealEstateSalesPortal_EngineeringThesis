using Inżynierka.Shared.DTOs.Offers.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Listing
{
    public class AgencyOfferListing : OfferListing
    {
        public DateTime? AgencyCreatedAt { get; set; }
        public IEnumerable<AgencyOffersListThumbnailDTO>? AgencyOfferDTOs { set; get; }
        public string? AgencyName { get; set; }
        public string? AgencyAddress { get; set; }
        public string? AgencyCity { get; set; }
        public string? AgencyPhoneNumber { get; set; }
        public string? AgencyDescription { get; set; }
        public byte[]? AgencyLogo { get; set; }
        public Guid? AgencyInvitationGuid { get; set; }
    }
}
