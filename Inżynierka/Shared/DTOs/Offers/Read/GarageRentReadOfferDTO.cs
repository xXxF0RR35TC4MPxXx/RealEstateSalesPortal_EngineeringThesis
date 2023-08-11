using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Common.Enums;
using System.ComponentModel;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class GarageRentReadOfferDTO : ReadOfferDTO
    {
        [Description("Konstrukcja")]
        [Display(Order = 26)]
        public string? Construction { get; set; }

        [Description("Lokalizacja")]
        [Display(Order = 27)]
        public string? Location { get; set; }

        [Description("Oświetlenie garażu")]
        [Display(Order = 28)]
        public string? Lighting { get; set; }

        [Description("Ogrzewanie garażu")]
        [Display(Order = 29)]
        public string? Heating { get; set; }

        public GarageRentReadOfferDTO(string offerTitle, int price, string sellerType,
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

        public GarageRentReadOfferDTO()
        {
        }
    }
}
