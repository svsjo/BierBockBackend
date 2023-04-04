namespace BierBockBackend.Data;

public class UserBasicData
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BirthDate { get; set; }
    public Coordinate Location { get; set; }
}