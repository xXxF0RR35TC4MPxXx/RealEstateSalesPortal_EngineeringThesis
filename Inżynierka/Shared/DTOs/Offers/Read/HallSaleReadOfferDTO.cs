using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Common.Enums;
using System.ComponentModel;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class HallSaleReadOfferDTO : ReadOfferDTO
    {
        [Description("Wysokość")]
        [Display(Order = 26)]
        public int? Height { get; set; } //wysokość

        [Description("Konstrukcja")]
        [Display(Order = 27)]
        public string? Construction { get; set; } //konstrukcja

        [Description("Parking hali")]
        [Display(Order = 28)]
        public string? ParkingSpace { get; set; } //parking

        [Description("Stan wykończenia")]
        [Display(Order = 29)]
        public string? FinishCondition { get; set; } //stan wykończenia

        [Description("Posadzka")]
        [Display(Order = 30)]
        public string? Flooring { get; set; } //posadzka

        [Description("Ogrzewanie hali")]
        [Display(Order = 31)]
        public bool? Heating { get; set; } //ogrzewanie

        [Description("Ogrodzenie hali")]
        [Display(Order = 32)]
        public bool? Fencing { get; set; } //ogrodzenie

        [Description("Pomieszczenia biurowe")]
        [Display(Order = 33)]
        public bool? HasOfficeRooms { get; set; } //pomieszczenia biurowe

        [Description("Zaplecze socjalne")]
        [Display(Order = 34)]
        public bool? HasSocialFacilities { get; set; } //zaplecze socjalne
        [Description("Rampa")]
        [Display(Order = 35)]
        public bool? HasRamp { get; set; } //rampa


        // ======== przeznaczenie hali =========
        public string GetDesignation() //przeznaczenie
        {
            List<string> single = new();
            if (Storage == true) single.Add("magazynowe");
            if (Production == true) single.Add("produkcyjne");
            if (Office == true) single.Add("biurowe");
            if (Commercial == true) single.Add("handlowe");

            return string.Join(", ", single);
        }

        [Description("magazynowe")]
        [Display(Order = 36)]
        public bool? Storage { get; set; }
        [Description("produkcyjne")]
        [Display(Order = 37)]
        public bool? Production { get; set; }
        [Description("biurowe")]
        [Display(Order = 38)]
        public bool? Office { get; set; }
        [Description("handlowe")]
        [Display(Order = 39)]
        public bool? Commercial { get; set; }



        // ========== zabezpieczenia ===========
        public string GetSafety()
        {
            List<string> single = new();
            if (AntiBurglaryBlinds == true) single.Add("rolety anty-włamaniowe");
            if (AntiBurglaryWindowsOrDoors == true) single.Add("drzwi lub okna anty-włamaniowe");
            if (IntercomOrVideophone == true) single.Add("domofon");
            if (MonitoringOrSecurity == true) single.Add("monitoring");
            if (AlarmSystem == true) single.Add("system alarmowy");
            if (ClosedArea == true) single.Add("teren zamknięty");

            return string.Join(", ", single);
        }

        [Description("Rolety anty-włamaniowe")]
        [Display(Order = 67)]
        public bool? AntiBurglaryBlinds { get; set; }
        [Description("Drzwi lub okna anty-włamaniowe")]
        [Display(Order = 47)]
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        [Description("Domofon")]
        [Display(Order = 48)]
        public bool? IntercomOrVideophone { get; set; }
        [Description("Monitoring")]
        [Display(Order = 49)]
        public bool? MonitoringOrSecurity { get; set; }
        [Description("System alarmowy")]
        [Display(Order = 50)]
        public bool? AlarmSystem { get; set; }
        [Description("Teren zamknięty")]
        [Display(Order = 51)]
        public bool? ClosedArea { get; set; }



        // ======= media ========
        public string GetMedia()
        {
            List<string> single = new();
            if (Internet == true) single.Add("internet");
            if (Phone == true) single.Add("telefon stacjonarny");
            if (Water == true) single.Add("woda");
            if (Electricity == true) single.Add("prąd");
            if (Sewerage == true) single.Add("kanalizacja");
            if (Gas == true) single.Add("gaz");
            if (SepticTank == true) single.Add("szambo");
            if (SewageTreatmentPlant == true) single.Add("oczyszczalnia ścieków");
            if (ThreePhaseElectricPower == true) single.Add("siła");

            return string.Join(", ", single);
        }

        [Description("Internet")]
        [Display(Order = 35)]
        public bool? Internet { get; set; }

        [Description("Telefon")]
        [Display(Order = 37)]
        public bool? Phone { get; set; }
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

        [Description("Siła")]
        [Display(Order = 44)]
        public bool? ThreePhaseElectricPower { get; set; } //siła

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
        [Description("Dojazd asf.")]
        [Display(Order = 46)]
        public bool? AsphaltDriveway { get; set; }

        [Description("Rodzaj rynku")]
        [Display(Order = 99)]
        public string? TypeOfMarket { get; set; }

        public HallSaleReadOfferDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            int? height, string? construction, string? parkingSpace, string? finishCondition, string? flooring,
            bool? heating, bool? fencing, bool? hasOfficeRooms, bool? hasSocialFacilities, bool? hasRamp, bool? storage, bool? production,
            bool? office, bool? commercial, bool? antiBurglaryBlinds, bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone,
            bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea, bool? internet, bool? threePhaseElectricPower, bool? phone,
            bool? water, bool? electricity, bool? sewerage, bool? gas, bool? septicTank, bool? sewageTreatmentPlant, bool? fieldDriveway,
            bool? pavedDriveway, bool? asphaltDriveway, string offerType, string? typeOfMarket, int? area)
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

            Height = height;
            Construction = construction;
            ParkingSpace = parkingSpace;
            FinishCondition = finishCondition;
            Flooring = flooring;
            Heating = heating;
            Fencing = fencing;
            HasOfficeRooms = hasOfficeRooms;
            HasSocialFacilities = hasSocialFacilities;
            HasRamp = hasRamp;
            Storage = storage;
            Production = production;
            Office = office;
            Commercial = commercial;
            AntiBurglaryBlinds = antiBurglaryBlinds;
            AntiBurglaryWindowsOrDoors = antiBurglaryWindowsOrDoors;
            IntercomOrVideophone = intercomOrVideophone;
            MonitoringOrSecurity = monitoringOrSecurity;
            AlarmSystem = alarmSystem;
            ClosedArea = closedArea;
            Internet = internet;
            ThreePhaseElectricPower = threePhaseElectricPower;
            Phone = phone;
            Water = water;
            Electricity = electricity;
            Sewerage = sewerage;
            Gas = gas;
            SepticTank = septicTank;
            SewageTreatmentPlant = sewageTreatmentPlant;
            FieldDriveway = fieldDriveway;
            PavedDriveway = pavedDriveway;
            AsphaltDriveway = asphaltDriveway;
            OfferType = offerType;
            TypeOfMarket = typeOfMarket;
            Area = area;
        }

        public HallSaleReadOfferDTO()
        {
        }
    }
}
