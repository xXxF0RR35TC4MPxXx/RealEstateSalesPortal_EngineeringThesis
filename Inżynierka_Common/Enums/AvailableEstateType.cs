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
    public enum AvailableEstateType
    {
        [Description("Mieszkania")] APARTMENT = 1,
        [Description("Domy")] HOUSE = 2,
        [Description("Pokoje")] ROOM = 3,
        [Description("Działki")] PLOT = 4,
        [Description("Lokale użytkowe")] PREMISES = 5,
        [Description("Hale i magazyny")] HALL = 6,
        [Description("Garaże")] GARAGE = 7,
        [Description("Wszystkie")] ALL = 8
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/