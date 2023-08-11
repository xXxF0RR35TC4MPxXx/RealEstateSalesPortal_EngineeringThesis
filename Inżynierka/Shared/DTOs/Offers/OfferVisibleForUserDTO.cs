using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers
{
    public class OfferVisibleForUserDTO
    {
        public OfferVisibleForUserDTO() { }

        public OfferVisibleForUserDTO(int ofId, int usId, string email) 
        {
            offerId = ofId;
            userId = usId;
            userEmail = email;
        }

        public int offerId { get; set; }
        public int userId { get; set; }
        public string userEmail { get; set; }

    }
}
