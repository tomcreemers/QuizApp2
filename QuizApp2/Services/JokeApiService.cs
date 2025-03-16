using System.Text.Json;
using QuizApp2.Models;

namespace QuizApp2.Services
{
    public class JokeApiService
    {
        private readonly HttpClient _httpClient;

        public JokeApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetRandomJokeAsync()
        {
            try
            {
                // Chuck Norris API endpoint
                var json = await _httpClient.GetStringAsync("https://api.chucknorris.io/jokes/random");

                // Deserialize into our Joke model
                var joke = JsonSerializer.Deserialize<Joke>(json);

                // Return the joke text
                return joke?.Value ?? "No joke found.";
            }
            catch
            {
                return "Error fetching joke.";
            }
        }
    }
}
