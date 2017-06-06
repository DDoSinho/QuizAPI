using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Entities
{
    [Table("GivenAnswers")]
    public class GivenAnswer
    {
        public int GivenAnswerId { get; set; }

        public Nullable<int> QuestionId { get; set; }

        public Nullable<int> AnswerId { get; set; }

        public Nullable<int> SessionId { get; set; }

        public bool Correct { get; set; }

        public virtual Question Question { get; set; }

        public virtual Answer Answer { get; set; }

        public virtual Session Session { get; set; }

        public DateTime AnswerDate { get; set; } = DateTime.Now;
    }
}
