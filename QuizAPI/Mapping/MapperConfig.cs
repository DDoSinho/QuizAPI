using AutoMapper;
using QuizAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Mapping
{
    public class MapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Quiz, Dtos.Quiz>();
                cfg.CreateMap<Dtos.Quiz, Quiz>();

                cfg.CreateMap<Question, Dtos.Question>();
                cfg.CreateMap<Dtos.Question, Question>();

                cfg.CreateMap<Answer, Dtos.Answer>();
                cfg.CreateMap<Dtos.Answer, Answer>();
            });

            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}
