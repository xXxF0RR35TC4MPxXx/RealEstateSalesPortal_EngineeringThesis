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
    public enum ApartmentHeating
    {
        [Description("Miejskie")] DISTRICT_HEATING = 1,
        [Description("Gazowe")] GAS = 2,
        [Description("Piece kaflowe")] TILED_STOVE = 3,
        [Description("Elektryczne")] ELECTRIC = 4,
        [Description("Kotłownia")] BOILER = 5,
        [Description("Inne")] OTHER = 6
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/