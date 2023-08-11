using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.User
{
    public class NewUserViewModel
    {
        [Required(ErrorMessage = "Pole \"Imię\" jest wymagane!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole \"Nazwisko\" jest wymagane!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole \"Email\" jest wymagane!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[\\w-_]+(\\.[\\w!#$%'*+\\/=?\\^`{|}]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessage = "Podana wartość nie jest poprawnym adresem email!\"")]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public string? City { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression("[0-9]{2}-[0-9]{3}", ErrorMessage = "Podany ciąg nie jest poprawnym kodem pocztowym (nie brakuje myślnika?)!")]
        public string? PostalCode { get; set; }

        [DataType(DataType.Text)]
        public string? Street { get; set; }


        [Required(ErrorMessage = "Pole \"numer telefonu\" jest wymagane")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawdziwy numer telefonu.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Podaj hasło drugi raz.")]
        [CompareProperty(nameof(Password), ErrorMessage="Podane hasła nie są takie same!")]
        [DataType(DataType.Password)]
        public string SecondPassword { get; set; }

        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", ErrorMessage = "Podany ciąg nie jest poprawnym kodem.")]
        public string? AgencyInvitationGuid { get;set; }    
    }
}
