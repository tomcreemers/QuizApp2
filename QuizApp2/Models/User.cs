using SQLite;

namespace QuizApp2.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string QrCode { get; set; }
    }
}
