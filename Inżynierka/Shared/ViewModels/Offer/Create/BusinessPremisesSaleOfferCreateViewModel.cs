using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Create
{
    public class BusinessPremisesSaleOfferCreateViewModel : OfferCreateViewModel
    {
        public PremisesLocation? Location { get; set; }

        public Floor? Floor { get; set; }

        public int? YearOfConstruction { get; set; }

        public PremisesFinishCondition? FinishCondition { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Liczba pomieszczeń nie może być ujemna")]
        public int? RoomCount { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; } //powierzchnia domu

        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }


        // ========== przeznaczenie lokalu ===========
        public bool? Service { get; set; }
        public bool? Gastronomic { get; set; }
        public bool? Office { get; set; }
        public bool? Industrial { get; set; }
        public bool? Commercial { get; set; }
        public bool? Hotel { get; set; }


        // ======== media =========
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }
        public bool? Water { get; set; }
        public bool? Electricity { get; set; }
        public bool? SewageSystem { get; set; }
        public bool? Gas { get; set; }
        public bool? SepticTank { get; set; }
        public bool? SewageTreatmentPlant { get; set; }


        // ======== dodatkowe info ==========
        public bool? Shopwindow { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? AsphaltDriveway { get; set; }
        public bool? Heating { get; set; }
        public bool? Elevator { get; set; }
        public bool? Furnishings { get; set; }
        public bool? AirConditioning { get; set; }

        public TypeOfMarket? TypeOfMarket { get; set; }
    }
}
