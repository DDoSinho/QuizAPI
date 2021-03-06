﻿using Microsoft.EntityFrameworkCore;
using QuizAPI.Entities;
using QuizAPI.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Model
{
    public class QuestionManager
    {
        private QuizDbContext Context { get; }

        public QuestionManager(QuizDbContext context)
        {
            Context = context;
        }

        public void AddQuestion(Question question, Theme theme, Quiz quiz)
        {
            QuizQuestion quizquestion = new QuizQuestion();

            var QuizInDb = Context.Quizs
                           .Where(q => q.Name == quiz.Name)
                           .FirstOrDefault();

            if (QuizInDb == null)
            {
                Context.Add(quiz);
                quizquestion.Quiz = quiz;
            }
            else
            {
                quizquestion.Quiz = QuizInDb;
            }

            var ThemeInDb = Context.Themes
                               .Where(t => t.Name == theme.Name)
                               .FirstOrDefault();

            if (ThemeInDb == null)
            {
                Context.Add(theme);
                theme.Questions.Add(question);
            }
            else
            {
                ThemeInDb.Questions.Add(question);
            }

            quizquestion.Question = question;

            Context.Add(quizquestion);

            Context.SaveChanges();
        }

        public void AddAnswer(Answer answer)
        {
            Context.Add(answer);
            Context.SaveChanges();
        }

        public void AddGivedAnswer(GivenAnswer givenanswer)
        {
            Context.Add(givenanswer);
            Context.SaveChanges();
        }

        public void AddGivedAnswer(IEnumerable<GivenAnswer> givenAnswers)
        {
            foreach (var givenAnswer in givenAnswers)
            {
                Context.Add(givenAnswer);
            }

            Context.SaveChanges();
        }

        public void AddSession(Session session)
        {
            Context.Add(session);
            Context.SaveChanges();
        }

        public void AddQuiz(Quiz quiz)
        {
            Context.Add(quiz);
            Context.SaveChanges();
        }

        public void AddQuizQuestion(QuizQuestion quizquestion)
        {
            Context.Add(quizquestion);
            Context.SaveChanges();
        }

        public List<Theme> GetThemes()
        {
            var query = from q in Context.Themes
                        select q;

            return query.ToList();
        }

        public List<Quiz> GetQuizs()
        {
            return Context.Quizs.ToList();
        }

        public List<int> GetQuestionsIdByThemeName(string themeName)
        {
            var query = Context.Questions
                        .Where(q => q.Theme.Name == themeName)
                        .Select(q => q.QuestionId);

            return query.ToList();
        }

        public List<int> GetQuestionsIdByQuizId(string quizName)
        {
            var query = Context.QuizQuestions
                       .Where(q => q.Quiz.Name == quizName)
                       .Select(q => q.Question.QuestionId)
                       .ToList();
            return query;
        }

        public Question GetQuestionById(int id)
        {
            return Context.Questions
                   .Include(q => q.Answers)
                   .Where(q => q.QuestionId == id)
                   .SingleOrDefault();
        }

        public string GetThemeNameByQuestionId(int questionId)
        {
            return Context.Questions
                   .Where(q => q.QuestionId == questionId)
                   .Select(q => q.Theme.Name)
                   .Single();
        }

        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            var query = from q in Context.Answers
                        where q.Question.QuestionId == questionId
                        select q;

            return query.ToList();
        }

        public bool IsItGoodAnswers(List<GivenAnswer> givenAnswerList)
        {
            foreach (var givenans in givenAnswerList)
            {
                var query = Context.Answers
                            .Where(a => a.AnswerId == givenans.AnswerId)
                            .Where(a => a.IsGoodAnswer == givenans.Correct)
                            .FirstOrDefault();

                if (query == null)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetPoint(int questionId)
        {
            decimal countCorrect = 0;
            decimal countRecord = 0;

            var givenAnswers = Context.GivenAnswers
                               .Where(g => g.QuestionId == questionId)
                               .ToList();

            var sessionIds = givenAnswers.Select(g => g.SessionId).Distinct();

            foreach (var session in sessionIds)
            {
                List<GivenAnswer> givenAnswerList = givenAnswers.Where(g => g.SessionId == session).ToList();

                if (IsItGoodAnswers(givenAnswerList))
                {
                    countCorrect++;
                }

                countRecord++;
            }


            int point = 0;

            if (countRecord != 0) //0-val nem tudunk osztani
                point = (int)(100 - Math.Floor((countCorrect / countRecord) * 100));

            return point > 0 ? point : 1;
        }

        public void SetPhotoUrl(QuizUser user, string url)
        {
            user.PhotoUrl = url;
            Context.SaveChanges();
        }

        public Quiz GetQuizByName(string quizName)
        {
            return Context.Quizs.Where(q => q.Name == quizName).First();
        }

        public void AddPoint(Session session, int point)
        {
            session.Point = point;
            Context.SaveChanges();
        }

        public Session GetSessionById(int id)
        {
            return Context.Sessions.Where(s => s.SessionId == id).Single();
        }

        public Dictionary<string, int> GetAllTopScore()
        {
            return (from s in Context.Sessions
                    group s by s.Quiz into q
                    select new { Name = q.Key.Name, Point = q.Max(m => m.Point) })
                   .ToDictionary(t => t.Name, t => t.Point);
        }

        public Dictionary<string, int> GetTopScore(string userId)
        {
            return (from s in Context.Sessions
                    where s.QuizUser.Equals(userId)
                    group s by s.Quiz into q
                    select new { Name = q.Key.Name, Point = q.Max(m => m.Point) })
                   .ToDictionary(t => t.Name, t => t.Point);

        }

        public string GetMostPopularQuizName()
        {
            var query = (from s in Context.Sessions
                         group s by s.Quiz into q
                         select new { Name = q.Key.Name, Count = q.Count() })
                         .ToList();

            int max = query.Select(q => q.Count).Max();

            return query.Where(q => q.Count == max).Single().Name;
        }

        private double GetDifficulty(int questionId)
        {
            double countCorrect = 0;
            double countRecord = 0;

            var givenAnswers = Context.GivenAnswers
                               .Where(g => g.QuestionId == questionId)
                               .ToList();

            var sessionIds = givenAnswers.Select(g => g.SessionId).Distinct();

            foreach (var session in sessionIds)
            {
                List<GivenAnswer> givenAnswerList = givenAnswers.Where(g => g.SessionId == session).ToList();

                if (IsItGoodAnswers(givenAnswerList))
                {
                    countCorrect++;
                }

                countRecord++;
            }

            return countRecord != 0 ? countCorrect / countRecord : 0.0;
        }

        public KeyValuePair<string, string> GetHardestAndEasiestQuestion()
        {
            Dictionary<string, double> questionIdPointPairs = new Dictionary<string, double>();

            foreach (var question in Context.Questions.ToList())
            {
                questionIdPointPairs.Add(question.Text, GetDifficulty(question.QuestionId));
            }

            return new KeyValuePair<string, string>(questionIdPointPairs.OrderByDescending(item => item.Value).Last().Key, questionIdPointPairs.OrderByDescending(item => item.Value).First().Key);
        }

        //ANGULAR

        public List<Question> GetQuestions(int quizId)
        {
            return Context.QuizQuestions.Where(q => q.Quiz.QuizID == quizId).Select(q => q.Question).ToList();
        }

        public int GetNumberOfGoodAnswers(IEnumerable<GivenAnswer> givenAnswers)
        {
            int numberOfGoodAnswers = 0;

            var questionIds = givenAnswers.Select(g => g.QuestionId).Distinct();

            foreach (var questionId in questionIds)
            {
                if (IsItGoodAnswers(givenAnswers.Where(g => g.QuestionId == questionId).ToList()))
                {
                    numberOfGoodAnswers++;
                }
            }

            return numberOfGoodAnswers;
        }

        public void AddAnswers(IEnumerable<Answer> answers, Question question)
        {
            foreach (var answer in answers)
            {
                answer.Question = question;
                Context.Add(answer);
            }
            Context.SaveChanges();
        }

    }
}

