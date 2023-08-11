using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities
{
    [Table("SingleFormProposal")]
    public class SingleFormProposal
    {
        [Key]
        public int FormId { get; set; }

        [ForeignKey("FormId")]
        public virtual UserPreferenceForm Form { get; set; }

        [Key]
        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }
    }
}
