using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers
{
    public class HomepageOffersDTO
    {
        public byte[]? Photo { get; set; }
        public int Id { get; set; }
        public string OfferTitle { get; set; }
        public string OfferCategory { get; set; }
        public string ForSaleOrForRent { get; set; }
        public string Voivodeship { get; set; }
        public string City { get; set; }
        public int? Area { get; set; }
        public int? RoomCount { get; set; }
        public decimal Price { get; set; }
    }
}
