using Inżynierka_Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.ViewModels.Offer.Update
{
    public class PlotOfferUpdateViewModel : OfferUpdateViewModel
    {
        [Required]
        public PlotType PlotType { get; set; }

        public bool? IsFenced { get; set; }

        public Location? Location { get; set; }

        public string? Dimensions { get; set; }

        [Required]
        public OfferType OfferType { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Powierzchnia nie może być ujemna")]
        public int? Area { get; set; }


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
