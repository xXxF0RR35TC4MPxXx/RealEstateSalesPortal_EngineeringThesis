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
    public enum ParkingType
    {
        [Description("Brak")] NONE = 1,
        [Description("Utwardzony")] PAVED = 2,
        [Description("Betonowy")] CONCRETE = 3,
        [Description("Asfaltowy")] ASPHALT = 4,
        [Description("Nieutwardzony")] UNPAVED = 5,
        [Description("Kostka brukowa")] SETT = 6
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/