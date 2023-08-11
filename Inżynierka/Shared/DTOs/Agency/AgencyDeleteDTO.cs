using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Delete
{
    public class AgencyDeleteDTO
    {
        public AgencyDeleteDTO(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
