using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.Agency
{
    public class AgencyUpdateViewModel
    {
        //używa enumów, bez konstruktorów

        [Required(ErrorMessage = "Pole \"Nazwa agencji\" jest wymagane!")]
        public string AgencyName { get; set; }

        [Required(ErrorMessage = "Pole \"Email\" jest wymagane!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[\\w-_]+(\\.[\\w!#$%'*+\\/=?\\^`{|}]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessage = "Podana wartość nie jest poprawnym adresem email!\"")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole \"Numer telefonu\" jest wymagane")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawdziwy numer telefonu.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole \"Miasto\" jest wymagane")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole \"Adres\" jest wymagane")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Pole \"Kod pocztowy\" jest wymagane")]
        [RegularExpression("[0-9]{2}-[0-9]{3}", ErrorMessage = "Podany ciąg nie jest poprawnym kodem pocztowym (nie brakuje myślnika?)!")]
        public string PostalCode { get; set; }

        [MaxLength(1000, ErrorMessage ="Maksymalna długość opisu to 1000 znaków")]
        public string? Description { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawidłowy numer NIP.")]
        public string? NIP { get; set; }

        [MinLength(9, ErrorMessage = "Podany numer musi mieć długość 9 lub 14 znaków")]
        [MaxLength(14, ErrorMessage = "Podany numer musi mieć długość 9 lub 14 znaków")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawidłowy numer REGON.")]
        public string? REGON { get; set; }

        public string? LicenceNumber { get; set; }

        public string? Logo { get; set; }
    }
}
