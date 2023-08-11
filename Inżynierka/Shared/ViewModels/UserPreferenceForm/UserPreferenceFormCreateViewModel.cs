using Inżynierka_Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.UserPreferenceForm
{
    public class UserPreferenceFormCreateViewModel
    {
        //używa enumów, bez konstruktorów

        public string? City { get; set; }

        public OfferType? OfferType { get; set; } //Sale or rent

        public EstateType? EstateType { get; set; } //house, apartment etc.

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna!")]
        public int? MinArea { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna!")]
        public int? MaxArea { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Liczba nie może być ujemna!")]
        public int? RoomCount { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Cena nie może być ujemna!")]
        public int? MaxPrice { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[\\w-_]+(\\.[\\w!#$%'*+\\/=?\\^`{|}]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessage = "Podana wartość nie jest poprawnym adresem email!")]
        public string ClientEmail { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawidłowy numer telefonu.")]
        public string ClientPhone { get; set; }

    }
}
