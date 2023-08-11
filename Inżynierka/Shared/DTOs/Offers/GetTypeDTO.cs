using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers
{
    public class GetTypeDTO
    {
        public string? type { get; set; }

        public GetTypeDTO(string? type)
        {
            this.type = type;
        }
    }
}
