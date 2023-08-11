using Inżynierka_Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.UserPreferenceForm
{
    public class AddNewSuggestionViewModel
    {
        [Range(1, Int32.MaxValue, ErrorMessage = "Wartość nie może być ujemna!")]
        public int? OfferId { get; set; }
    }
}
