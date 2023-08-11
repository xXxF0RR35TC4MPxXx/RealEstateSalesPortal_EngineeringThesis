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
    public enum Floor
    {
        [Description("Suterena")] BASEMENT = 1,
        [Description("Parter")] GROUND_FLOOR = 2,
        [Description("1")] FIRST_FLOOR = 3,
        [Description("2")] SECOND_FLOOR = 4,
        [Description("3")] THIRD_FLOOR = 5,
        [Description("4")] FOURTH_FLOOR = 6,
        [Description("5")] FIFTH_FLOOR = 7,
        [Description("6")] SIXTH_FLOOR = 8,
        [Description("7")] SEVENTH_FLOOR = 9,
        [Description("8")] EIGHTH_FLOOR = 10,
        [Description("9")] NINTH_FLOOR = 11,
        [Description("10")] TENTH_FLOOR = 12,
        [Description(">10")] OVER_TENTH_FLOOR = 13,
        [Description("Poddasze")] ATTIC = 14,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/