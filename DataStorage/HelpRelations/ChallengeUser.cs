using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BierBockBackend.Data;


namespace DataStorage.HelpRelations
{
    public class ChallengeUser
    {
        [Key] public int Id { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }
    }
}
