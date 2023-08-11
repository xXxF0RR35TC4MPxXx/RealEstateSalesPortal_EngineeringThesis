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
    public enum PUSTYENUM
    {
        [Description("")] xd = 1,
        [Description("")] xd2 = 2,
        [Description("")] xd3 = 3,
        [Description("")] xd4 = 4,
        [Description("")] xd5 = 5,
        [Description("")] xd6 = 6,
        [Description("")] xd7 = 7,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/