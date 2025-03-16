using SQLite;

namespace QuizApp2.Models
{
    [Table("SessionMessage")]
    public class SessionMessage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int SessionId { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
