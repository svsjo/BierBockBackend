using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BierBockBackend.Data;

namespace DataStorage.HelpRelations
{
    public class ChallengeUser
    {
        [Key] public int Id { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }
    }
}
