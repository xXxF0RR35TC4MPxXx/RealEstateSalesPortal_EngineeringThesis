using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class PlotReadOfferDTO : ReadOfferDTO
    {
        [Description("Typ działki")]
        [Display(Order = 26)]
        public string PlotType { get; set; }

        [Description("Ogrodzenie")]
        [Display(Order = 27)]
        public bool? IsFenced { get; set; }

        [Description("Położenie")]
        [Display(Order = 28)]
        public string? Location { get; set; }

        [Description("Wymiary")]
        [Display(Order = 29)]
        public string? Dimensions { get; set; }



        // ======== dojazd =========
        public string GetDriveway()
        {
            List<string> single = new();
            if (FieldDriveway == true) single.Add("nieutwardzony");
            if (PavedDriveway == true) single.Add("utwardzony");
            if (AsphaltDriveway == true) single.Add("asfaltowy");

            return string.Join(", ", single);
        }

        [Description("Dojazd nieutwardzony")]
        [Display(Order = 44)]
        public bool? FieldDriveway { get; set; }
        [Description("Dojazd utwardzony")]
        [Display(Order = 45)]
        public bool? PavedDriveway { get; set; }
        [Description("Dojazd asfaltowy")]
        [Display(Order = 46)]
        public bool? AsphaltDriveway { get; set; }



        // ======== media =========
        public string GetMedia()
        {
            List<string> single = new();
            if (Water == true) single.Add("woda");
            if (Phone == true) single.Add("telefon");
            if (Electricity == true) single.Add("prąd");
            if (Gas == true) single.Add("gaz");
            if (SepticTank == true) single.Add("szambo");
            if (SewageTreatmentPlant == true) single.Add("oczyszczalnia ścieków");
            if (Sewerage == true) single.Add("kanalizacja");

            return string.Join(", ", single);
        }

        [Description("Woda")]
        [Display(Order = 38)]
        public bool? Water { get; set; }
        [Description("Prąd")]
        [Display(Order = 39)]
        public bool? Electricity { get; set; }
        [Description("Kanalizacja")]
        [Display(Order = 40)]
        public bool? Sewerage { get; set; }
        [Description("Gaz")]
        [Display(Order = 41)]
        public bool? Gas { get; set; }
        [Description("Szambo")]
        [Display(Order = 42)]
        public bool? SepticTank { get; set; }
        [Description("Oczyszczalnia ścieków")]
        [Display(Order = 43)]
        public bool? SewageTreatmentPlant { get; set; }

        [Description("Telefon")]
        [Display(Order = 44)]
        public bool? Phone { get; set; }




        // ========== okolica ==========
        public string GetSurroundings()
        {
            List<string> single = new();
            if (Forest == true) single.Add("las");
            if (Lake == true) single.Add("jezioro");
            if (Mountains == true) single.Add("góry");
            if (Sea == true) single.Add("morze");
            if (OpenArea == true) single.Add("otwarty teren");

            return string.Join(", ", single);
        }

        [Description("Las")]
        [Display(Order = 44)]
        public bool? Forest { get; set; }
        [Description("Jezioro")]
        [Display(Order = 44)]
        public bool? Lake { get; set; }
        [Description("Góry")]
        [Display(Order = 44)]
        public bool? Mountains { get; set; }
        [Description("Morze")]
        [Display(Order = 44)]
        public bool? Sea { get; set; }
        [Description("Otwarty teren")]
        [Display(Order = 44)]
        public bool? OpenArea { get; set; }


        public PlotReadOfferDTO(string offerTitle, int price, string sellerType, 
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus, string offerType,

            string plotType, bool? isFenced, string? location, string? dimensions, bool? fieldDriveway, 
            bool? pavedDriveway, bool? asphaltDriveway, bool? phone, bool? water, bool? electricity, bool? sewerage, 
            bool? gas, bool? septicTank, bool? sewageTreatmentPlant, bool? forest, bool? lake, bool? mountains, bool? sea, bool? openArea)
        {
            OfferTitle = offerTitle;
            Price = price;
            SellerType = sellerType;
            Voivodeship = voivodeship;
            City = city;
            Address = address;
            Description = description;
            RemoteControl = remoteControl;
            OfferStatus = offerStatus;
            OfferType = offerType;

            PlotType = plotType;
            IsFenced = isFenced;
            Location = location;
            Dimensions = dimensions;
            FieldDriveway = fieldDriveway;
            PavedDriveway = pavedDriveway;
            AsphaltDriveway = asphaltDriveway;
            Phone = phone;
            Water = water;
            Electricity = electricity;
            Sewerage = sewerage;
            Gas = gas;
            SepticTank = septicTank;
            SewageTreatmentPlant = sewageTreatmentPlant;
            Forest = forest;
            Lake = lake;
            Mountains = mountains;
            Sea = sea;
            OpenArea = openArea;
        }

        public PlotReadOfferDTO()
        {
        }
    }
}
