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
    public enum RoofingType
    {
        [Description("Blacha")] CORRUGATED_IRON = 1,
        [Description("Dachówka")] ROOFTILES = 2,
        [Description("Eternit")] ETERNIT = 3,
        [Description("Gont")] WOOD_SHINGLES = 4,
        [Description("Łupek")] SLATE = 5,
        [Description("Papa")] BITUMINOUS = 6,
        [Description("Strzecha")] THATCHING = 7,
        [Description("Inne")] OTHER = 8,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/