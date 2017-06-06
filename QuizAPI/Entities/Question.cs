using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Entities
{
    [Table("Questions")]
    public class Question
    {
        public Question()
        {
            this.Answers = new List<Answer>();
            this.GivenAnswers = new List<GivenAnswer>();
            this.QuizQuestions = new List<QuizQuestion>();
        }

        public int QuestionId { get; set; }

        public string Text { get; set; }

        public Nullable<int> ThemeId { get; set; }

        public virtual Theme Theme { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public ICollection<GivenAnswer> GivenAnswers { get; set; }

        public ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}
