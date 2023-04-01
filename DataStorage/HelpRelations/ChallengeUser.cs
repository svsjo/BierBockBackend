using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BierBockBackend.Data;

namespace DataStorage.HelpRelations
{
    public class ChallengeUser
    {
        [Key] public int Id { get; set; }
        public int UserId { get; set; }
        public int ChallengeId { get; set; }
        public virtual User User { get; set; }
        public virtual Challenge Challenge { get; set; }
    }
}
