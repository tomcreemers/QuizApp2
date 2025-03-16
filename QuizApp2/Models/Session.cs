using SQLite;

namespace QuizApp2.Models
{
    [Table("Session")]
    public class Session
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // The user who created this session
        public int HostUserId { get; set; }

        // e.g. "Economy,Sports" or just "Tech"
        public string SelectedCategories { get; set; }

        // Difficulty 1..5
        public int Difficulty { get; set; }

        // Unique code for joining via QR
        public string SessionCode { get; set; }
    }
}
