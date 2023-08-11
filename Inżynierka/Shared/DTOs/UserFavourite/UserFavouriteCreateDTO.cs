using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.UserFavourite
{
    public class UserFavouriteCreateDTO
    {
        public int UserId { get; set; }

        public int OfferId { get; set; }

        public DateTime LikeDate { get; set; }
    }
}
