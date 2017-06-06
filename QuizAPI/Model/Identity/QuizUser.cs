using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QuizAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Model.Identity
{
    public class QuizUser : IdentityUser
    {
        public QuizUser()
        {
            this.Sessions = new List<Session>();
        }

        public ICollection<Session> Sessions { get; set; }

        public string PhotoUrl { get; set; }
    }
}
