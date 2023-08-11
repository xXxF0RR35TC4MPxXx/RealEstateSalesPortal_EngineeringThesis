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
    public enum GarageLocation
    {
        [Description("W budynku")] IN_THE_BUILDING = 1,
        [Description("Przy domu")] BY_THE_HOUSE = 2,
        [Description("Samodzielny")] SEPARATE = 3
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/