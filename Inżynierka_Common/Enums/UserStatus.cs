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
    public enum UserStatus
    {
        [Description("NOT_VERIFIED")] NOT_VERIFIED = 1,
        [Description("ACTIVE")] ACTIVE = 2,
        [Description("ARCHIVED")] ARCHIVED = 3,
        [Description("DELTED")] DELETED = 4
    }
}
