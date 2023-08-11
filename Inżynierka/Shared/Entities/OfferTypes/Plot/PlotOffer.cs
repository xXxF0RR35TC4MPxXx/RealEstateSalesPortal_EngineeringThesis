using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities.OfferTypes.Plot
{
    public class PlotOffer:Offer
    {
        [Required]
        [DataType(DataType.Text)]
        public string PlotType { get; set; }

        public bool? IsFenced { get; set; }

        [DataType(DataType.Text)]
        public string? Location { get; set; }

        [DataType(DataType.Text)]
        public string? Dimensions { get; set; }


        // ======== dojazd =========
        public bool? FieldDriveway { get; set; }
        public bool? PavedDriveway { get; set; }
        public bool? AsphaltDriveway { get; set; }



        // ======== media =========
        public bool? Phone { get; set; }
        public bool? Water { get; set; }
        public bool? Electricity { get; set; }
        public bool? Sewerage { get; set; }
        public bool? Gas { get; set; }
        public bool? SepticTank { get; set; }
        public bool? SewageTreatmentPlant { get; set; }


        // ========== okolica ==========
        public bool? Forest { get; set; }
        public bool? Lake { get; set; }
        public bool? Mountains { get; set; }
        public bool? Sea { get; set; }
        public bool? OpenArea { get; set; }
    }
}
