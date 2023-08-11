using Inżynierka.Shared.Entities.OfferTypes.Garage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities.OfferTypes.Garage
{
    public class GarageRentOffer : Offer
    {
        [DataType(DataType.Text)]
        public string? Construction { get; set; }

        [DataType(DataType.Text)]
        public string? Location { get; set; }

        [DataType(DataType.Text)]
        public string? Lighting { get; set; }

        [DataType(DataType.Text)]
        public string? Heating { get; set; }
    }
}
