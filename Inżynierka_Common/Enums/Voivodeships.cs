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
    public enum Voivodeships
    {
        [Description("dolnośląskie")] DOLNOŚLĄSKIE = 1,
        [Description("kujawsko-pomorskie")] KUJAWSKO_POMORSKIE = 2,
        [Description("lubelskie")] LUBELSKIE = 3,
        [Description("lubuskie")] LUBUSKIE = 4,
        [Description("łódzkie")] ŁÓDZKIE = 5,
        [Description("małopolskie")] MAŁOPOLSKIE = 6,
        [Description("mazowieckie")] MAZOWIECKIE = 7,
        [Description("opolskie")] OPOLSKIE = 8,
        [Description("podkarpackie")] PODKARPACKIE = 9,
        [Description("podlaskie")] PODLASKIE = 10,
        [Description("pomorskie")] POMORSKIE = 11,
        [Description("śląskie")] ŚLĄSKIE = 12,
        [Description("świętokrzyskie")] ŚWIĘTOKRZYSKIE = 13,
        [Description("warmińsko-mazurskie")] WARMIŃSKO_MAZURSKIE = 14,
        [Description("wielkopolskie")] WIELKOPOLSKIE = 15,
        [Description("zachodniopomorskie")] ZACHODNIOPOMORSKIE = 16,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/