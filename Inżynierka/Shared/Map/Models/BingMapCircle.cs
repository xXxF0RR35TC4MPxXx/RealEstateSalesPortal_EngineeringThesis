using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Map.Models
{
    public class MapCircle
    {

        private static double ToRadian(double degrees) { return degrees * (Math.PI / 180); }
        private static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
        public static List<Location> DrawMapCircle(Location location, int radiusInMeters)
        {
            double earthRadiusInMeters = 6367.0 * 1000.0;
            var lat = ToRadian(location.latitude);
            var lng = ToRadian(location.longitude);
            var d = radiusInMeters / earthRadiusInMeters;

            var locations = new List<Location>();

            for (var x = 0; x <= 360; x += 3)
            {
                var brng = ToRadian(x);
                var latRadians = Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(brng));
                var lngRadians = lng + Math.Atan2(Math.Sin(brng) * Math.Sin(d) * Math.Cos(lat), Math.Cos(d) - Math.Sin(lat) * Math.Sin(latRadians));

                locations.Add(new Location(ToDegrees(latRadians), ToDegrees(lngRadians)));
            }

            return locations;
        }
    }

    public class PolygonOptions
    {
        public string fillColor;
        public string strokeColor;
        public int strokeThickness;
    }

    public class Polygon
    {
        public Location[][] rings;
        public PolygonOptions options;
    }
}
