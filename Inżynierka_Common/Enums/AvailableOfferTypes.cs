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
    public enum AvailableOfferTypes
    {
        [Description("Na sprzedaż")] SALE = 1,
        [Description("Na wynajem")] RENT = 2,
        [Description("Na sprzedaż i wynajem")] SALE_AND_RENT = 3,
    }

}
