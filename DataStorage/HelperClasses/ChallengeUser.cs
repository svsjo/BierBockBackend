#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#endregion

namespace DataStorage.HelperClasses;

public class ChallengeUser
{
    [Key] public int Id { get; set; }

    [JsonIgnore] [ForeignKey("UserId")] public virtual User User { get; set; }

    [JsonIgnore]
    [ForeignKey("ChallengeId")]
    public virtual Challenge Challenge { get; set; }
}