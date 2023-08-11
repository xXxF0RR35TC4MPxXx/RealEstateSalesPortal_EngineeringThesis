using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.User;
using Inżynierka_Common.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Listing
{
    public class AgentsListing
    {
        public int TotalCount { get; set; }
        public IEnumerable<AgentsDTO> AgentsDTOs { set; get; }
    }
}
