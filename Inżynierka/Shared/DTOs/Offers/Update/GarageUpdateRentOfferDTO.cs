using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Update
{
    public class GarageUpdateRentOfferDTO: OfferUpdateDTO
    {
        public GarageConstruction? Construction { get; set; }

        public GarageLocation? Location { get; set; }

        public GarageLighting? Lighting { get; set; }

        public GarageHeating? Heating { get; set; }

        public GarageUpdateRentOfferDTO(string offerTitle, int price,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            GarageConstruction? construction, GarageLocation? location, GarageLighting? lighting, GarageHeating? heating)
        {
            OfferTitle = offerTitle;
            Price = price;
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
        }

        public GarageUpdateRentOfferDTO()
        {
        }
    }
}
