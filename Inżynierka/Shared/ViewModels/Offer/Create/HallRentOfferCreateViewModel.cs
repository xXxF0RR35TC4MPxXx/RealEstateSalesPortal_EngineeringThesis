using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inżynierka_Common.Enums;

namespace Inżynierka.Shared.ViewModels.Offer.Create
{
    public class HallRentOfferCreateViewModel : OfferCreateViewModel
    {
        [Range(1, Int32.MaxValue, ErrorMessage = "Wysokość nie może być ujemna")]
        public int? Height { get; set; } //wysokość

        public HallConstruction? Construction { get; set; } //konstrukcja

        public ParkingType? ParkingSpace { get; set; } //parking

        public HallFinishCondition? FinishCondition { get; set; } //stan wykończenia

        public Flooring? Flooring { get; set; } //posadzka

        public bool? Heating { get; set; } //ogrzewanie
        public bool? Fencing { get; set; } //ogrodzenie
        public bool? HasOfficeRooms { get; set; } //pomieszczenia biurowe
        public bool? HasSocialFacilities { get; set; } //zaplecze socjalne
        public bool? HasRamp { get; set; } //rampa

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; }


        // ======== przeznaczenie hali =========
        public bool? Storage { get; set; }
        public bool? Production { get; set; }
        public bool? Office { get; set; }
        public bool? Commercial { get; set; }


        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }


        // ======= media ========
        public bool? Internet { get; set; }
        public bool? ThreePhaseElectricPower { get; set; } //siła
        public bool? Phone { get; set; }
        public bool? Water { get; set; }
        public bool? Electricity { get; set; }
        public bool? Sewerage { get; set; }
        public bool? Gas { get; set; }
        public bool? SepticTank { get; set; }
        public bool? SewageTreatmentPlant { get; set; }



        // ======== dojazd =========
        public bool? FieldDriveway { get; set; }
        public bool? PavedDriveway { get; set; }
        public bool? AsphaltDriveway { get; set; }

    }
}
