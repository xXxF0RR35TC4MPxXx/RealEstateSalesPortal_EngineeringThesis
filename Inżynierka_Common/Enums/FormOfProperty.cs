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
    public enum FormOfProperty
    {
        [Description("Spółdzielcze własnościowe")] COOPERATIVE_OWNERSHIP = 1,
        [Description("Spółdzielcze wł. z KW")] COOPERATIVE_OWNERSHIP_WITH_LAND_REGISTER = 2, // z księgą wieczystą
        [Description("Pełna własność")] FULL_OWNERSHIP = 3,
        [Description("Udział")] PARTIAL_OWNERSHIP = 4
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/