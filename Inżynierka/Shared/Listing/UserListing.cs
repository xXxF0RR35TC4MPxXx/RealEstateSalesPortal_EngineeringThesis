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
    public class UserListing
    {
        public int TotalCount { get; set; }
        public IEnumerable<UserDTO> UserDTOs { set; get; }
        public UserFilteringDTO UserFilteringDTO { set; get; }
        public SortOrder SortOrder { get; set; }
        public Paging Paging { set; get; }
    }
}
