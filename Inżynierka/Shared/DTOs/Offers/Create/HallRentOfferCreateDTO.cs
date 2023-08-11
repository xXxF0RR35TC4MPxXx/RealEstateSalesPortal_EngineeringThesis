using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Create
{
    public class HallRentOfferCreateDTO:OfferCreateDTO
    {
        public int? Height { get; set; } //wysokość

        public string? Construction { get; set; } //konstrukcja

        public string? ParkingSpace { get; set; } //parking

        public string? FinishCondition { get; set; } //stan wykończenia

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


        public string? AvailableFromDate { get; set; }

        public HallRentOfferCreateDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus, 
            int? height, string? construction, string? parkingSpace, string? finishCondition, string? flooring, 
            bool? heating, bool? fencing, bool? hasOfficeRooms, bool? hasSocialFacilities, bool? hasRamp, string offerType, bool? storage, 
            bool? production, bool? office, bool? commercial, bool? antiBurglaryBlinds, bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone, 
            bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea, bool? internet, bool? threePhaseElectricPower, bool? phone, 
            bool? water, bool? electricity, bool? sewerage, bool? gas, bool? septicTank, bool? sewageTreatmentPlant, bool? fieldDriveway, 
            bool? pavedDriveway, bool? asphaltDriveway, string? availableFromDate)
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
            OfferType = offerType;
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
            AvailableFromDate = availableFromDate;
        }

        public HallRentOfferCreateDTO()
        {
        }
    }
}
