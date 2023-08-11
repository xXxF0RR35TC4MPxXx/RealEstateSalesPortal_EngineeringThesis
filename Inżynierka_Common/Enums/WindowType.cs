using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inżynierka_Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WindowType
    {
        [Description("Brak")] NONE = 1,
        [Description("Plastikowe")] PLASTIC = 2,
        [Description("Aluminiowe")] METAL = 3,
        [Description("Drewniane")] WOOD = 4,
        [Description("Inne")] OTHER = 5,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/