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
    public enum HallConstruction
    {
        [Description("Stalowa")] STEEL = 1,
        [Description("Murowana")] BRICK = 2,
        [Description("Wiata")] SHED = 3,
        [Description("Namiotowa")] TENT = 4,
        [Description("Drewniana")] WOOD = 5,
        [Description("Szklana")] GLASS = 6
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/