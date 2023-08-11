using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities.OfferTypes.Apartment
{
    public class ApartmentSaleOffer : Offer
    {
        [DataType(DataType.Text)]
        public string? TypeOfBuilding { get; set; } //rodzaj zabudowy

        [DataType(DataType.Text)]
        public string? ApartmentFinishCondition { get; set; }

        [DataType(DataType.Text)]
        public string? Floor { get; set; }

        public int? FloorCount { get; set; }

        [DataType(DataType.Text)]
        public string? BuildingMaterial { get; set; }

        [DataType(DataType.Text)]
        public string? WindowsType { get; set; }

        [DataType(DataType.Text)]
        public string? HeatingType { get; set; }

        public int? Rent { get; set; }

        public string? AvailableSinceDate { get; set; }


        public int? YearOfConstruction { get; set; }


        //======== media =============
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }

        //========== ============
        public bool? Balcony { get; set; }
        public bool? UtilityRoom { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? Basement { get; set; }
        public bool? Garden { get; set; }
        public bool? Terrace { get; set; }
        public bool? Elevator { get; set; }
        public bool? TwoLevel { get; set; }
        public bool? SeparateKitchen { get; set; }
        public bool? AirConditioning { get; set; }



        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }



        // ======= wyposażenie =======
        public bool? Furniture { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? Dishwasher { get; set; }
        public bool? Fridge { get; set; }
        public bool? Stove { get; set; }
        public bool? Oven { get; set; }
        public bool? TV { get; set; }


        [DataType(DataType.Text)]
        public string? TypeOfMarket { get; set; }

        [DataType(DataType.Text)]
        public string? FormOfProperty { get; set; }
    }
}
