using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GeoCoordinatePortable;

namespace DataStorage;

public class Coordinate
{
    [JsonIgnore] [Key] public int Id { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }

    [JsonIgnore] public GeoCoordinate GetCoordinate => new(Latitude, Longitude, Altitude);

    public double GetDistance(Coordinate targetCoordinate)
    {
        return GetCoordinate.GetDistanceTo(targetCoordinate.GetCoordinate);
    }
}