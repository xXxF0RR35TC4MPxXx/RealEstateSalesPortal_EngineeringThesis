using Inżynierka_Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Create
{
    public class RoomOfferCreateDTO : OfferCreateDTO
    {
        public int? AdditionalFees { get; set; } //opłaty dodatkowe

        public int? Deposit { get; set; } //kaucja

        public int? NumberOfPeopleInTheRoom { get; set; }
        public string? TypeOfBuilding { get; set; } //rodzaj zabudowy
        public string? Floor { get; set; }

        public string? AvailableFromDate { get; set; }

        public bool? AvailableForStudents { get; set; }

        public bool? OnlyForNonsmoking { get; set; }


        // ======= wyposażenie =======
        public bool? Furniture { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? Dishwasher { get; set; }
        public bool? Fridge { get; set; }
        public bool? Stove { get; set; }
        public bool? Oven { get; set; }
        public bool? TV { get; set; }

        // ======= media ========
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }

        public RoomOfferCreateDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus, string offerType,
            int? additionalFees, int? deposit, int? numberOfPeopleInTheRoom, string? typeOfBuilding,
            string? availableFromDate, bool? availableForStudents, bool? onlyForNonsmoking, bool? furniture, bool? washingMachine, 
            bool? dishwasher, bool? fridge, bool? stove, bool? oven, bool? tV, bool? internet, bool? cableTV, bool? homePhone, string? floor)
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

            AdditionalFees = additionalFees;
            Deposit = deposit;
            NumberOfPeopleInTheRoom = numberOfPeopleInTheRoom;
            TypeOfBuilding = typeOfBuilding;
            AvailableFromDate = availableFromDate;
            AvailableForStudents = availableForStudents;
            OnlyForNonsmoking = onlyForNonsmoking;
            Furniture = furniture;
            WashingMachine = washingMachine;
            Dishwasher = dishwasher;
            Fridge = fridge;
            Stove = stove;
            Oven = oven;
            TV = tV;
            Internet = internet;
            CableTV = cableTV;
            HomePhone = homePhone;
            Floor = floor;
        }

        public RoomOfferCreateDTO()
        {
        }
    }
}
