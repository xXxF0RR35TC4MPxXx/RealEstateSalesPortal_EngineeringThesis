using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.DTOs.Offers.Filtering
{
    public class OfferFilteringDTO
    {
        public string? City { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public HowRecent? HowRecent { get; set; }

        public int? OfferId { get; set; }

        public string? DescriptionFragment { get; set; }

        public bool RemoteControl { get; set; }


    }
}
