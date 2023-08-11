using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Listing
{
    public class UserOfferListing : OfferListing
    {
        public int UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public byte[]? UserAvatar { get; set; }
        public string? UserDescription { get; set; }

        public string? UserPhoneNumber { get; set; }
        public DateTime? UserCreatedAt { get; set; }

        public int? AgencyId { get; set; }
        public string? AgencyName { get; set; }
        public string? AgencyAddress { get; set; }
        public string? AgencyCity { get; set; }
        public string? AgencyVoivodeship { get; set; }
        public string? AgencyPhoneNumber { get; set; }
        public string? AgencyDescription { get; set; }
        public byte[]? AgencyLogo { get; set; }
    }
}
