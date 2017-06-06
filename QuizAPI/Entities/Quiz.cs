using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Entities
{
    [Table("Quizs")]
    public class Quiz
    {
        public Quiz()
        {
            Sessions = new List<Session>();
            this.QuizQuestions = new List<QuizQuestion>();
        }

        public int QuizID { get; set; }

        public string Name { get; set; }

        public ICollection<Session> Sessions { get; set; }

        public ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}
