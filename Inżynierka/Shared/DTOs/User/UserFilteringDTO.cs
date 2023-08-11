using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.User
{
    public class UserFilteringDTO
            {
        public string? Email { get; set; }
        public string? UserStatus { get; set; }
        public string? RoleName { get; set; }

        public UserFilteringDTO(string? email, string? userStatus, string? roleName)
        {
            Email = email;
            UserStatus = userStatus;
            RoleName = roleName;
        }
    }
}
