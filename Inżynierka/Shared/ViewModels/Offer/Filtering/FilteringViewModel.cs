using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.Offer.Filtering
{
    public class FilteringViewModel
    {
        public Paging Paging { get; set; }
        public SortOrder? SortOrder { get; set; }

        public string? City { get; set; }

        public string? Voivodeship { get; set; }
        
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public HowRecent? HowRecent { get; set; }

        public int? OfferId { get; set; }

        public string? DescriptionFragment { get; set; }

        public bool? RemoteControl { get; set; }

    }
}
