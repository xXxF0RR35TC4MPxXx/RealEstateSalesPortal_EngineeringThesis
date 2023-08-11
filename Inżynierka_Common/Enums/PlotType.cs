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
    public enum PlotType
    {
        [Description("Budowlana")] BUILDING = 1,
        [Description("Rolna")] AGRICULTURAL = 2,
        [Description("Rekreacyjna")] RECREATIONAL = 3,
        [Description("Pod inwestycję")] FOR_INVESTMENT = 4,
        [Description("Leśna")] FOREST = 5,
        [Description("Rolno-budowlana")] AGRICULTURAL_AND_CONSTRUCTION = 6,
        [Description("Siedliskowa")] SETTLEMENT = 7,
        [Description("Inna")] OTHER = 8
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/