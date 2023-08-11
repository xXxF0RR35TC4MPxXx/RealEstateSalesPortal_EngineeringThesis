using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Map.Models
{
    public class Location
    {
        public double latitude;
        public double longitude;

        public Location(double lat, double lon)
        {
            latitude = lat;
            longitude = lon;
        }
    }

    public class Pushpin
    {
        public string color;
        public Location location;
    }
}
