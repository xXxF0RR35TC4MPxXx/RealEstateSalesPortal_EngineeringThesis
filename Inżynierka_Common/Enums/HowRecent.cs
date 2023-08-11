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
    public enum HowRecent
    {
        [Description("Ostatnie 7 dni")] LAST_WEEK = 1,
        [Description("Ostatnie 30 dni")] LAST_MONTH = 2,
        [Description("Ostatnie 90 dni")] LAST_3_MONTHS = 3,
        [Description("Wszystkie")] ALL = 4
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/