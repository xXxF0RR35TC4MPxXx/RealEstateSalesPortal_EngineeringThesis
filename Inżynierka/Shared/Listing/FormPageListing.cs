using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Listing
{
    public class FormPageListing
    {
        public List<SingleFormProposalReadDTO>? FormResponses { set; get; }
        public string? City { get; set; }

        public string? OfferType { get; set; } //Sale or rent

        public string EstateType { get; set; } //house, apartment etc.

        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }

        public int? RoomCount { get; set; }
        public int? MaxPrice { get; set; }

        public string? ClientEmail { get; set; }

        public string? ClientPhone { get; set; }
        public string? ClientComment { get; set; }

        public Guid guid { get; set; }
    }
}
