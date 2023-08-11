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
    public enum UserRoles
    {
        [Description("Niezalogowany")] ANONYMOUS = 1,
        [Description("Użytkownik prywatny")] PRIVATE = 2,
        [Description("Agent nieruchomości")] AGENCY = 3,
        [Description("Admin")] ADMIN = 4
    }
}
