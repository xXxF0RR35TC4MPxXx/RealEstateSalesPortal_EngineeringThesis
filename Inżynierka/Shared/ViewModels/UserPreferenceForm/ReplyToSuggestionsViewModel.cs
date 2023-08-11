using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inżynierka.Shared.ViewModels.UserPreferenceForm
{
    public class ReplyToSuggestionsViewModel
    {
        [Required(ErrorMessage = "Wiadomość nie może być pusta")]
        [StringLength(1000, ErrorMessage="Maksymalna długość wiadomości 1000 znaków.")]
        public string ClientComment { get; set; }
    }
}
