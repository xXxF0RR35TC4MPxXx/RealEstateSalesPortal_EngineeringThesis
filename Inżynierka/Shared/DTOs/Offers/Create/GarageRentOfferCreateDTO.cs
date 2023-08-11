using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Create
{
    public class GarageRentCreateOfferDTO : OfferCreateDTO
    {
        public string? Construction { get; set; }

        public string? Location { get; set; }

        public string? Lighting { get; set; }

        public string? Heating { get; set; }

        public GarageRentCreateOfferDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus, 
            string? construction, string? location, string? lighting, string? heating, string offerType)
        {
            OfferTitle = offerTitle;
            Price = price;
            SellerType = sellerType;
            Voivodeship = voivodeship;
            City = city;
            Address = address;
            Description = description;
            RemoteControl = remoteControl;
            OfferStatus = offerStatus;

            Construction = construction;
            Location = location;
            Lighting = lighting;
            Heating = heating;
            OfferType = offerType;
        }

        public GarageRentCreateOfferDTO()
        {
        }
    }
}
