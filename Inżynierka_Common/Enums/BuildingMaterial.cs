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
    public enum BuildingMaterial
    {
        [Description("Cegła")] BRICK = 1,
        [Description("Drewno")] WOOD = 2,
        [Description("Pustak")] AIRBRICK = 3,
        [Description("Keramzyt")] EXCLAY = 4,
        [Description("Wielka płyta")] PLATTENBAU = 5,
        [Description("Beton")] CONCRETE = 6,
        [Description("Silikat")] CALCIUM_SILICATE_BRICK = 7,
        [Description("Beton komórkowy")] CELLULAR_CONCRETE = 8,
        [Description("Żelbet")] REINFORCED_CONCRETE = 9,
        [Description("Inne")] OTHER = 10,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/