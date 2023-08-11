namespace Inżynierka.Shared.DTOs.UserPreferenceForm
{
    public class UserPreferenceFormThumbnailDTO
    {

        public int Id { get; set; }

        public string? City { get; set; }

        public string? OfferType { get; set; } //Sale or rent

        public string EstateType { get; set; } //house, apartment etc.

        public string? ClientEmail { get; set; }

        public UserPreferenceFormThumbnailDTO(string? city, string? offerType, string estateType, string? clientEmail, int id)
        {
            City = city;
            OfferType = offerType;
            EstateType = estateType;
            ClientEmail = clientEmail;
            Id = id;
        }

        public UserPreferenceFormThumbnailDTO()
        {
        }
    }
}
