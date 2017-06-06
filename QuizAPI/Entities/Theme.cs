using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Entities
{
    [Table("Themes")]
    public class Theme
    {
        public Theme()
        {
            this.Questions = new List<Question>();
        }

        public int ThemeId { get; set; }

        public string Name { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
