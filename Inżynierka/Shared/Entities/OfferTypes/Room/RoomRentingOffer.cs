using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities.OfferTypes.Room
{
    public class RoomRentingOffer : Offer
    {
        public int? AdditionalFees { get; set; } //opłaty dodatkowe

        public string? Floor { get; set; } //piętro

        public int? Deposit { get; set; } //kaucja

        public int? NumberOfPeopleInTheRoom { get; set; }

        [DataType(DataType.Text)]
        public string? TypeOfBuilding { get; set; } //rodzaj zabudowy

        public string? AvailableFromDate { get; set; }

        public bool? AvailableForStudents { get; set; }

        public bool? OnlyForNonsmoking { get; set; }

        // ======= wyposażenie =======
        public bool? Furniture { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? Dishwasher { get; set; }
        public bool? Fridge { get; set; }
        public bool? Stove { get; set; }
        public bool? Oven { get; set; }
        public bool? TV { get; set; }

        // ======= media ========
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }
    }
}
