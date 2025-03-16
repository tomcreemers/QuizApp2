using SQLite;

namespace QuizApp2.Models
{
    [Table("SessionParticipant")]
    public class SessionParticipant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int SessionId { get; set; }
        public int UserId { get; set; }
    }
}
