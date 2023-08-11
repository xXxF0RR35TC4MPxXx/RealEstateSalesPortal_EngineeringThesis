﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inżynierka_Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeOfMarket
    {
        [Description("Rynek pierwotny")] PRIMARY_MARKET = 1,
        [Description("Rynek wtórny")] AFTERMARKET = 2
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/