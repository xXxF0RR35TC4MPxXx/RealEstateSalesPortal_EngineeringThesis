using Inżynierka.Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities
{
    [Table("OfferVisibleForUser")]
    public class OfferVisibleForUser
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int OfferId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }
    }
}
