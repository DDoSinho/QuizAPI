using Microsoft.AspNetCore.Mvc;
using QuizAPI.Entities;
using QuizAPI.Model.ViewModels;
using QuizAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Controllers
{
    public class QuizApiController : Controller
    {
        private readonly IQuizService _quizService;

        public QuizApiController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public IActionResult Quizs()
        {
            return Ok(_quizService.GetQuizs().Select(s => new { s.QuizID, s.Name }));
        }

        [HttpGet]
        public IActionResult Themes()
        {
            return Ok(_quizService.GetThemes());
        }

        [HttpGet]
        public IActionResult Questions(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_quizService.GetQuestions(id).Select(s => new { s.QuestionId, s.Text }));
        }

        [HttpGet]
        public IActionResult Answers(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_quizService.GetAnswers(id).Select(s => new { s.AnswerId, s.Text, s.IsGoodAnswer }));
        }

        [HttpPost]
        public IActionResult GivedAnswers([FromBody] List<GivenAnswer> givenAnswers)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            _quizService.PostGivedAnswers(givenAnswers);

            return Ok(_quizService.GetNumberOfGoodAnswers(givenAnswers));
        }

        [HttpPost]
        public IActionResult Session([FromBody] Session session)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            _quizService.CreateNewSession(session);
            return Ok(new { sessionId = session.SessionId, quizId = session.QuizId, point = session.Point });
        }

        [HttpPost]
        public IActionResult AddQuestion([FromBody] AddQuestionViewModel viewModel)
        {
            Console.WriteLine();
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            _quizService.AddQuestion(viewModel);

            return Ok();
        }
    }
}
