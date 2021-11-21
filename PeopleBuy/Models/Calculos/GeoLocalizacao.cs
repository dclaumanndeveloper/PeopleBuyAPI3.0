namespace PeopleBuy.Models.Calculos
{
    public class GeoLocalizacao
    {
        public static double Calculate(double sLatitude, double sLongitude, double eLatitude,
                            double eLongitude)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var sLatitudeRadians = sLatitude /* radiansOverDegrees*/;
            var sLongitudeRadians = sLongitude /* radiansOverDegrees*/;
            var eLatitudeRadians = eLatitude /* radiansOverDegrees*/;
            var eLongitudeRadians = eLongitude /* radiansOverDegrees*/;
            //var dLatitude = Math.Sin((eLatitude - sLatitude) / 2);
            //  var dLongitude = Math.Sin((eLongitude - sLongitude) / 2);
            var dLongitude = (eLongitudeRadians - sLongitudeRadians) * radiansOverDegrees;
            var dLatitude = (eLatitudeRadians - sLatitudeRadians) * radiansOverDegrees;

            // var result1 = dLatitude * dLatitude + Math.Cos(sLatitude) * Math.Cos(eLatitude) * dLongitude * dLongitude;
            var result1 = Math.Pow(Math.Sin(dLatitude), 2.0) +
                           Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                           Math.Pow(Math.Sin(dLongitude), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = (6357 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1))) * 1000;



            return result2;
            /*const double r = 6371e3; // meters
            var dlat = (eLatitude - sLatitude)/2;
            var dlon = (eLongitude - sLongitude)/2;

            var q = Math.Pow(Math.Sin(dlat), 2) + Math.Cos(sLatitude) * Math.Cos(eLatitude) * Math.Pow(Math.Sin(dlon), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(q), Math.Sqrt(1 - q));

            var d = r * c;
            return d/1000 ;*/
            /* var radiansOverDegrees = (Math.PI / 180.0);

             var sLatitudeRadians = sLatitude * radiansOverDegrees;
             var sLongitudeRadians = sLongitude * radiansOverDegrees;
             var eLatitudeRadians = eLatitude * radiansOverDegrees;
             var eLongitudeRadians = eLongitude * radiansOverDegrees;

             var dLongitude = eLongitudeRadians - sLongitudeRadians;
             var dLatitude = eLatitudeRadians - sLatitudeRadians;

             var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) + 
                           Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) * 
                           Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

             // Using 3956 as the number of miles around the earth
             var result2 = 3956.0 * 2.0 * 
                           Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));
              result2 = (result2 *1609344);
             return result2;*/
        }
    }
}
