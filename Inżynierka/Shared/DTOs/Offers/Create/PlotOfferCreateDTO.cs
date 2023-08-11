using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Create
{
    public class PlotCreateOfferDTO : OfferCreateDTO
    {
        public string PlotType { get; set; }

        public bool? IsFenced { get; set; }

        public string? Location { get; set; }

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

        public PlotCreateOfferDTO(string offerTitle, int price, string sellerType, 
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

        public PlotCreateOfferDTO()
        {
        }
    }
}
