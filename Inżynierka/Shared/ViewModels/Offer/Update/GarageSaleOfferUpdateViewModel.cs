using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Update
{
    public class GarageSaleOfferUpdateViewModel : OfferUpdateViewModel
    {
        public GarageConstruction? Construction { get; set; }

        public GarageLocation? Location { get; set; }

        public GarageLighting? Lighting { get; set; }

        public GarageHeating? Heating { get; set; }

        public TypeOfMarket? TypeOfMarket { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; }
    }
}
