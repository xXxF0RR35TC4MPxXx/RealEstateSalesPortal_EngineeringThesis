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
    public enum UserRolesInSearch
    {
        [Description("Użytkownik prywatny")] PRIVATE = 1,
        [Description("Agent nieruchomości")] AGENCY = 2,
        [Description("Wszyscy ogłoszeniodawcy")] ALL = 3
    }
}
