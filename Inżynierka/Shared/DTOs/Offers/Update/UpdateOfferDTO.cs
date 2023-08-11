using Inżynierka.Shared.Entities;
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
    public class OfferUpdateDTO
    {
        public string OfferTitle { get; set; }

        public int Price { get; set; }

        public int? Area { get; set; } //powierzchnia domu

        public int? RoomCount { get; set; }

        public string Voivodeship { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public bool? RemoteControl { get; set; } //zdalna obsługa

        public string OfferStatus { get; set; }

        
    }
}
