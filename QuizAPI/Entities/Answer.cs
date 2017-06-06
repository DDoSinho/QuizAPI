using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Entities
{
    [Table("Answers")]
    public class Answer
    {
        public Answer()
        {
            this.GivenAnswers = new List<GivenAnswer>();
        }

        public int AnswerId { get; set; }

        public Nullable<int> QuestionId { get; set; }

        public string Text { get; set; }

        public bool IsGoodAnswer { get; set; }

        public virtual Question Question { get; set; }

        public ICollection<GivenAnswer> GivenAnswers { get; set; }
    }
}
