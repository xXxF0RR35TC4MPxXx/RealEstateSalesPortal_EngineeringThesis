using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inżynierka_Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EventCompletionStatus
    {
        [Display(Description = "Nowy")][Description("Nowy")] NEW,
        [Display(Description = "W trakcie")][Description("W trakcie")] DURING,
        [Display(Description = "Zakończony")][Description("Zakończony")] DONE,
        [Display(Description = "Odrzucony")][Description("Odrzucony")] REJECTED
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/