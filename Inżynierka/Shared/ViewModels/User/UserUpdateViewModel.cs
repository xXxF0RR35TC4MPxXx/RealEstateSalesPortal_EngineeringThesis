using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.User
{
    public class UserUpdateViewModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }


        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawdziwy numer telefonu.")]
        public string? PhoneNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Maksymalna długość opisu to 100 znaków!")]
        public string? Description { get; set; }

        public string? Avatar { get; set; }
        public string? City { get; set; }

        [RegularExpression("[0-9]{2}-[0-9]{3}", ErrorMessage = "Podany ciąg nie jest poprawnym kodem pocztowym (nie brakuje myślnika?)!")]
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
    }
}
