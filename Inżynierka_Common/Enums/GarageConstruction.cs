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
    public enum GarageConstruction
    {
        [Description("Murowany")] BRICK = 1,
        [Description("Blaszak")] TINKER = 2,
        [Description("Drewniany")] WOOD = 3,
        [Description("Wiata")] SHED = 4
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/