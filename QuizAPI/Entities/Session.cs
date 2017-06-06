using QuizAPI.Model.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Entities
{
    [Table("Sessions")]
    public class Session
    {
        public Session()
        {
            this.GivenAnswers = new List<GivenAnswer>();
        }

        public int SessionId { get; set; }

        public ICollection<GivenAnswer> GivenAnswers { get; set; }

        public virtual QuizUser QuizUser { get; set; }

        public virtual Quiz Quiz { get; set; }

        public Nullable<int> QuizId { get; set; }

        public int Point { get; set; }
    }
}
