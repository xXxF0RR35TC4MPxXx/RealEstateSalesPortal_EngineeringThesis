using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Update
{
    public class RoomOfferUpdateDTO : OfferUpdateDTO
    {
        public int? AdditionalFees { get; set; } //opłaty dodatkowe

        public int? Deposit { get; set; } //kaucja

        public int? NumberOfPeopleInTheRoom { get; set; }
        public Floor? Floor { get; set; }
        public TypeOfBuilding? TypeOfBuilding { get; set; } //rodzaj zabudowy

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

        public RoomOfferUpdateDTO(string offerTitle, int price,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus, 
            int? additionalFees, int? deposit, int? numberOfPeopleInTheRoom, TypeOfBuilding? typeOfBuilding,
            string? availableFromDate, bool? availableForStudents, bool? onlyForNonsmoking, bool? furniture, bool? washingMachine,
            bool? dishwasher, bool? fridge, bool? stove, bool? oven, bool? tV, bool? internet, bool? cableTV, bool? homePhone, Floor? floor)
        {
            OfferTitle = offerTitle;
            Price = price;
            Voivodeship = voivodeship;
            City = city;
            Address = address;
            Description = description;
            RemoteControl = remoteControl;
            OfferStatus = offerStatus;

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

        public RoomOfferUpdateDTO()
        {
        }


    }
}
