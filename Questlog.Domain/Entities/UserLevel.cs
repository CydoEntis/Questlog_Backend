using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Domain.Entities
{
    public class UserLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentExp { get; set; }
        [NotMapped]
        public int ExpForNextLevel => CalculateExpForLevel();

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public UserLevel()
        {
            CurrentLevel = 1;
            CurrentExp = 0;
        }


        public int CalculateExpForLevel()
        {
            int baseExp = 100;
            return baseExp * CurrentLevel;
        }


    }
}
