using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.UserEvents
{
    public class UserEventCreateViewModel
    {
        public int OfferId { get; set; }

        public string? ClientName { get; set; }

        [RegularExpression("^[\\w-_]+(\\.[\\w!#$%'*+\\/=?\\^`{|}]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessage = "Podana wartość nie jest poprawnym adresem email!")]
        public string? ClientEmail { get; set; }

        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawidłowy numer telefonu.")]
        public string? ClientPhoneNumber { get; set; }

        public string EventName { get; set; }

        public DateTime DeadlineDate { get; set; }
    }
}
