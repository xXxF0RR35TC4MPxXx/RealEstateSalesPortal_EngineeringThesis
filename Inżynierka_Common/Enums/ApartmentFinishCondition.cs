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
    public enum ApartmentFinishCondition
    {
        [Description("Do zamieszkania")] READY_TO_MOVE_IN = 1,
        [Description("Do wykończenia")] TO_FINISH = 2,
        [Description("Do remontu")] FOR_RENOVATION = 3,
        [Description("Inne")] OTHER = 4,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/