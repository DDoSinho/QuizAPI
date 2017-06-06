using QuizAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Model.ViewModels
{
    public class AddQuestionViewModel
    {
        public Quiz Quiz { get; set; }

        public Theme Theme { get; set; }

        public Question Question { get; set; }

        //public IEnumerable<Answer> Answers { get; set; }
    }
}
