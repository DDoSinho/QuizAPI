using AutoMapper;
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

        private readonly IMapper _mapper;

        public QuizApiController(IQuizService quizService, IMapper mapper)
        {
            _quizService = quizService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Quizs()
        {
            return Ok(_mapper.Map<List<Dtos.Quiz>>(_quizService.GetQuizs()));
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

            return Ok(_mapper.Map<List<Dtos.Question>>(_quizService.GetQuestions(id)));
        }

        [HttpGet]
        public IActionResult Answers(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<List<Dtos.Answer>>(_quizService.GetAnswers(id)));
        }

        [HttpPost]
        public IActionResult GivedAnswers([FromBody] List<GivenAnswer> givenAnswers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _quizService.PostGivedAnswers(givenAnswers);

            return Ok(_quizService.GetNumberOfGoodAnswers(givenAnswers));
        }

        [HttpPost]
        public IActionResult Session([FromBody] Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _quizService.CreateNewSession(session);

            return Ok(new { sessionId = session.SessionId, quizId = session.QuizId, point = session.Point });
        }

        [HttpPost]
        public IActionResult AddQuestion([FromBody] AddQuestionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _quizService.AddQuestion(viewModel);
            _quizService.AddAnswers(viewModel.Answers, viewModel.Question);

            return Ok();
        }
    }
}
