using QuizAPI.Entities;
using QuizAPI.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public interface IQuizService
    {
        IEnumerable<Quiz> GetQuizs();

        IEnumerable<Theme> GetThemes();

        IEnumerable<Question> GetQuestions(int quizId);

        IEnumerable<Answer> GetAnswers(int questionId);

        void PostGivedAnswers(IEnumerable<GivenAnswer> givenAnswers);

        void CreateNewSession(Session session);

        int GetNumberOfGoodAnswers(IEnumerable<GivenAnswer> givenAnswers);

        void AddQuestion(AddQuestionViewModel viewModel);
    }
}
