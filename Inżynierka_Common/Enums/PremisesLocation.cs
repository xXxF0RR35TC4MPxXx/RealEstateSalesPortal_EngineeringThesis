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
    public enum PremisesLocation
    {
        [Description("W centrum handlowym")] SHOPPING_CENTER = 1,
        [Description("W biurowcu")] OFFICE_BUILDING = 2,
        [Description("W bloku")] APARTMENT_BLOCK = 3,
        [Description("W kamienicy")] TENEMENT_HOUSE = 4,
        [Description("W domu prywatnym")] PRIVATE_HOUSE = 5,
        [Description("W budynku zabytkowym")] HISTORIC_BUILDING = 6,
        [Description("Oddzielny obiekt")] SEPARATE_OBJECT = 7,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/