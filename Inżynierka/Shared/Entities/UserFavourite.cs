using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities
{
    [Table("UserFavourite")]
    public class UserFavourite
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int OfferId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime LikeDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }

    }
}
