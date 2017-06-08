using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizAPI.Entities;
using QuizAPI.Model;
using QuizAPI.Model.ViewModels;

namespace QuizAPI.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuestionManager _questionManager;

        public QuizService(QuestionManager questionManager)
        {
            _questionManager = questionManager;
        }

        public IEnumerable<Quiz> GetQuizs()
        {
            return _questionManager.GetQuizs();
        }

        public IEnumerable<Question> GetQuestions(int quizId)
        {
            return _questionManager.GetQuestions(quizId);
        }

        public IEnumerable<Answer> GetAnswers(int questionId)
        {
            return _questionManager.GetAnswersByQuestionId(questionId);
        }

        public void PostGivedAnswers(IEnumerable<GivenAnswer> givenAnswers)
        {
            _questionManager.AddGivedAnswer(givenAnswers);
        }

        public void CreateNewSession(Session session)
        {
            _questionManager.AddSession(session);
        }

        public int GetNumberOfGoodAnswers(IEnumerable<GivenAnswer> givenAnswers)
        {
            return _questionManager.GetNumberOfGoodAnswers(givenAnswers);
        }

        public void AddQuestion(AddQuestionViewModel viewModel)
        {
            _questionManager.AddQuestion(viewModel.Question, viewModel.Theme, viewModel.Quiz);
        }

        public IEnumerable<Theme> GetThemes()
        {
            return _questionManager.GetThemes();
        }


        public void AddAnswers(IEnumerable<Answer> answers, Question question)
        {
            _questionManager.AddAnswers(answers, question);
        }
    }
}
