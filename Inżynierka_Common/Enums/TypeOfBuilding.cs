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
    public enum TypeOfBuilding
    {
        [Description("Blok mieszkalny")] RESIDENTIAL_BLOCK = 1,
        [Description("Kamienica")] TENEMENT = 2,
        [Description("Dom wolnostojący")] DETACHED_HOUSE = 3,
        [Description("Plomba")] INFILL = 4,
        [Description("Szeregowiec")] TERRACED_HOUSE = 5,
        [Description("Apartamentowiec")] APARTMENT_BUILDING = 6,
        [Description("Loft")] LOFT = 7,
        [Description("Inny")] OTHER = 8,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/