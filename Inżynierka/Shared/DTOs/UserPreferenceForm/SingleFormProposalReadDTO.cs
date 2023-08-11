using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.DTOs.UserPreferenceForm
{
    public class SingleFormProposalReadDTO
    {
        public int OfferId { get; set; }
        public string OfferTitle { get; set; }
        public int OfferPrice { get; set; }
        public string OfferCity { get; set; }
        public string OfferType { get; set; }
        public byte[]? OfferThumbnail { get; set; }

        public SingleFormProposalReadDTO(int offerId, string offerTitle, int offerPrice, string offerCity, byte[]? offerThumbnail, string offerType)
        {
            OfferId = offerId;
            OfferTitle = offerTitle;
            OfferPrice = offerPrice;
            OfferCity = offerCity;
            OfferThumbnail = offerThumbnail;
            OfferType = offerType;
        }

        public SingleFormProposalReadDTO()
        {
        }
    }
}
