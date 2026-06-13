namespace PeopleBuy.Models.Calculos
{
    /// <summary>
    /// Utilitário para cálculo de distância geográfica usando a fórmula de Haversine.
    /// </summary>
    public class GeoLocalizacao
    {
        /// <summary>
        /// Calcula a distância em quilômetros entre dois pontos geográficos.
        /// </summary>
        /// <param name="sLatitude">Latitude de origem (graus)</param>
        /// <param name="sLongitude">Longitude de origem (graus)</param>
        /// <param name="eLatitude">Latitude de destino (graus)</param>
        /// <param name="eLongitude">Longitude de destino (graus)</param>
        /// <returns>Distância em quilômetros</returns>
        public static double Calculate(double sLatitude, double sLongitude,
                                       double eLatitude, double eLongitude)
        {
            const double EarthRadiusKm = 6371.0;
            var radiansPerDegree = Math.PI / 180.0;

            var dLat = (eLatitude - sLatitude) * radiansPerDegree;
            var dLon = (eLongitude - sLongitude) * radiansPerDegree;

            var sLatRad = sLatitude * radiansPerDegree;
            var eLatRad = eLatitude * radiansPerDegree;

            var a = Math.Pow(Math.Sin(dLat / 2.0), 2.0)
                  + Math.Cos(sLatRad) * Math.Cos(eLatRad)
                  * Math.Pow(Math.Sin(dLon / 2.0), 2.0);

            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            return EarthRadiusKm * c;
        }
    }
}
