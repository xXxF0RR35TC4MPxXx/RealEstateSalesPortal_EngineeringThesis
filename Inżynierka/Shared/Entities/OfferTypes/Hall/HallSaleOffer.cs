using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities.OfferTypes.Hall
{
    public class HallSaleOffer: Offer
    {
        public int? Height { get; set; } //wysokość

        [DataType(DataType.Text)]
        public string? Construction { get; set; } //konstrukcja

        [DataType(DataType.Text)]
        public string? ParkingSpace { get; set; } //parking

        [DataType(DataType.Text)]
        public string? FinishCondition { get; set; } //stan wykończenia

        [DataType(DataType.Text)]
        public string? Flooring { get; set; } //posadzka

        public bool? Heating { get; set; } //ogrzewanie
        public bool? Fencing { get; set; } //ogrodzenie
        public bool? HasOfficeRooms { get; set; } //pomieszczenia biurowe
        public bool? HasSocialFacilities { get; set; } //zaplecze socjalne
        public bool? HasRamp { get; set; } //rampa



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

        [DataType(DataType.Text)]
        public string? TypeOfMarket { get; set; }
    }
}
