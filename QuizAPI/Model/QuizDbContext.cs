using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Entities;
using QuizAPI.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Model
{
    public class QuizDbContext : IdentityDbContext<QuizUser>
    {
        public QuizDbContext() { }

        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {

        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Theme> Themes { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<GivenAnswer> GivenAnswers { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Quiz> Quizs { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=QuizAPI;Trusted_Connection=True;");
        }
    }
}
