using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.ViewModels
{
    public class HomepageSearchQueryViewModel
    {
        [Required(ErrorMessage = "Typ oferty jest wymagany przy wyszukiwaniu!")]
        public OfferType offerType { get; set; }
        [Required(ErrorMessage = "Typ nieruchomości jest wymagany przy wyszukiwaniu!")]
        public EstateType estateType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cena musi być liczbą dodatnią!")]
        public int? minPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Cena musi być liczbą dodatnią!")]
        public int? maxPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Powierzchnia musi być liczbą dodatnią!")]
        public int? minArea { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Powierzchnia musi być liczbą dodatnią!")]
        public int? maxArea { get; set; }
        public string? City { get; set; }
    }
}
