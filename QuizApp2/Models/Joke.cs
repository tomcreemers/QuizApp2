using System.Text.Json.Serialization;

namespace QuizApp2.Models
{
    public class Joke
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
