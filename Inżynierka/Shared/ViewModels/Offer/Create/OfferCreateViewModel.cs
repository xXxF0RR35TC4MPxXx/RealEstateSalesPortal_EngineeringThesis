using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Inżynierka.Shared.ViewModels.Offer.Create
{
    public abstract class OfferCreateViewModel
    {

        [Required(ErrorMessage = "Pole tytułu ogłoszenia jest wymagane!")]
        [MaxLength(100, ErrorMessage = "Tytuł ogłoszenia może mieć długość maksymalnie 100 znaków!")]
        public string OfferTitle { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Cena jest wymagana!")]
        public int Price { get; set; }

        public List<string>? Photos { get; set; }

        [Required(ErrorMessage = "Pole nazwy województwa jest wymagane!")]
        public Voivodeships Voivodeship { get; set; }

        [Required(ErrorMessage = "Pole nazwy miasta jest wymagane")]
        [MaxLength(100, ErrorMessage = "Maksymalna długość nazwy miejscowości to 100 znaków!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole adresu jest wymagane")]
        [MaxLength(100, ErrorMessage = "Maksymalna długość adresu to 100 znaków!")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Pole opisu oferty jest wymagane")]
        [MaxLength(2000, ErrorMessage = "Maksymalna długość opisu to 2000 znaków!")]
        public string Description { get; set; }

        public bool? RemoteControl { get; set; } //zdalna obsługa
    }
}
