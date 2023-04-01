using GeoCoordinatePortable;

namespace BierBockBackend.Data;

public class Coordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public GeoCoordinate GetCoordinate => new GeoCoordinate(this.Latitude, this.Longitude, this.Altitude);

    public double GetDistance(Coordinate targetCoordinate)
    {
        return GetCoordinate.GetDistanceTo(targetCoordinate.GetCoordinate);
    }
}