using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.DTOs.UserPreferenceForm
{
    public class UserPreferenceFormCreateDTO
    {
        public string? City { get; set; }

        public string? OfferType { get; set; } //Sale or rent

        public string EstateType { get; set; } //house, apartment etc.

        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }

        public int? RoomCount { get; set; }
        public int? MaxPrice { get; set; }

        public string? ClientEmail { get; set; }

        public string? ClientPhone { get; set; }

        public UserPreferenceFormCreateDTO(string? city, string? offerType, string estateType, int? minArea, int? maxArea, int? roomCount, string? clientEmail, string? clientPhone, int? price)
        {
            City = city;
            OfferType = offerType;
            EstateType = estateType;
            MinArea = minArea;
            MaxArea = maxArea;
            RoomCount = roomCount;
            ClientEmail = clientEmail;
            ClientPhone = clientPhone;
            MaxPrice = price;
        }

        public UserPreferenceFormCreateDTO()
        {
        }
    }
}
