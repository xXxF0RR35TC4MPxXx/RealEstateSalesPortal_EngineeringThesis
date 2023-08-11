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
    public enum ContractTypes
    {
        [Description("Umowa sprzedaży mieszkania z zadatkiem")] APARTMENT_SALE_WITH_DEPOSIT_CONTRACT = 1,
        [Description("Umowa sprzedaży mieszkania z zaliczką")] APARTMENT_SALE_WITH_ADVANCE_CONTRACT = 2,
        [Description("Umowa sprzedaży mieszkania ze spółdzielczym prawem własnościowym")] APARTMENT_SALE_WITH_COOPERATIVE_OWNERSHIP_RIGHT_CONTRACT = 3,
        [Description("Umowa najmu mieszkania na czas nieokreślony")] APARTMENT_RENT_FOR_INDEFINITE_PERIOD = 4,

        [Description("Umowa sprzedaży lokalu użytkowego")] PREMISES_SALE_CONTRACT = 5,
        [Description("Umowa wynajmu lokalu użytkowego")] PREMISES_RENT_CONTRACT = 6,

        [Description("Umowa sprzedaży garażu")] GARAGE_SALE_CONTRACT = 7,
        [Description("Umowa wynajmu garażu")] GARAGE_RENT_CONTRACT = 8,

        [Description("Umowa sprzedaży domu")] HOUSE_SALE_CONTRACT = 9,
        [Description("Umowa wynajmu domu jednorodzinnego")] HOUSE_RENT_CONTRACT = 10,

        [Description("Umowa sprzedaży gruntu")] PLOT_SALE_CONTRACT = 11,
        [Description("Umowa wynajmu gruntu")] PLOT_RENT_CONTRACT = 12,

        [Description("Umowa wynajmu pokoju")] ROOM_RENT_CONTRACT = 13,

        [Description("Protokół zdawczo-odbiorczy najmu")] DELIVERY_AND_ACCEPTANCE_REPORT_FOR_RENT = 14,
        [Description("Protokół zdawczo-odbiorczy sprzedaży")] DELIVERY_AND_ACCEPTANCE_REPORT_FOR_SALE = 15,
        [Description("Umowa pośrednictwa sprzedaży nieruchomości")] ESTATE_SALE_MEDIATION_CONTRACT = 16,

        [Description("Umowa sprzedaży hali lub magazynu")] HALL_SALE_CONTRACT = 17,
        [Description("Umowa wynajmu hali lub magazynu")] HALL_RENT_CONTRACT = 18,
    }
}

//wyciąganie opisu z enum'a: https://www.dotnetstuffs.com/string-enum-values-with-spaces-description-attribute-csharp/