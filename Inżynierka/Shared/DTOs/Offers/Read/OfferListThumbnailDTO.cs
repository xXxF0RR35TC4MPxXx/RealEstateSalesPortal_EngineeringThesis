using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class OfferListThumbnailDTO
    {
        public int Id { get; set; }
        public string OfferTitle { get; set; }
        public string? City { get; set; }
        public string? Voivodeship { get; set; }
        public int Price { get; set; } //cena całkowita
        public int PriceForOneSquareMeter { get; set; } //cena za m2
        public int? RoomCount { get; set; } //liczba pokoi
        public int? Area { get; set; } //powierzchnia domu
       
        public int? AgencyId { get; set; } //id agencji
        public string? AgencyName { get; set; } //nazwa agencji
        public byte[]? AgencyLogo { get; set; } //logo agencji

        public string SellerType { get; set; } //rodzaj oferty (Priv / agent)
        public string? OfferType { get; set; } // sell / rent
        public string? EstateType { get; set; } // House/Hall/Apartment

        public byte[]? Photo { get; set; }
    }
}
