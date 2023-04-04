using System.ComponentModel.DataAnnotations;
using GeoCoordinatePortable;

namespace BierBockBackend.Data;

public class Coordinate
{
    [Key]
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public GeoCoordinate GetCoordinate => new GeoCoordinate(this.Latitude, this.Longitude, this.Altitude);

    public double GetDistance(Coordinate targetCoordinate)
    {
        return GetCoordinate.GetDistanceTo(targetCoordinate.GetCoordinate);
    }
}