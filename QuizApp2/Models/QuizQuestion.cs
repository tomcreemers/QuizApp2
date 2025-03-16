using SQLite;

namespace QuizApp2.Models
{
    [Table("QuizQuestion")]
    public class QuizQuestion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Prompt { get; set; }

        public string Category { get; set; }

        public int Difficulty { get; set; }
    }
}
