using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using QuizApp2.Services;
using QuizApp2.Repositories;
using QuizApp2.Models;

namespace QuizApp2.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private readonly JokeApiService _jokeApiService;
        private readonly GenericRepository<QuizQuestion> _quizRepo;

        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        private int _difficulty;
        public int Difficulty
        {
            get => _difficulty;
            set { _difficulty = value; OnPropertyChanged(); }
        }

        public string DifficultyLabel => $"Difficulty: {Difficulty}";

        private List<QuizQuestion> _questions;
        private int _currentIndex;

        private string _currentQuestion;
        public string CurrentQuestion
        {
            get => _currentQuestion;
            set { _currentQuestion = value; OnPropertyChanged(); }
        }

        private string _jokeText;
        public string JokeText
        {
            get => _jokeText;
            set { _jokeText = value; OnPropertyChanged(); }
        }

        public ICommand NextQuestionCommand { get; }
        public ICommand FetchJokeCommand { get; }

        public QuizViewModel(string category, int difficulty)
        {
            try
            {
                // Debug line
                Console.WriteLine($"QuizViewModel created with category='{category}' difficulty={difficulty}");

                Category = category;
                Difficulty = difficulty;

                // Create the joke service
                _jokeApiService = new JokeApiService();

                // Resolve the quiz repo
                _quizRepo = MauiProgram.CreateMauiApp()
                    .Services.GetService<GenericRepository<QuizQuestion>>();

                // Commands
                NextQuestionCommand = new Command(OnNextQuestion);
                FetchJokeCommand = new Command(async () => await OnFetchJoke());

                // Seed if needed
                Console.WriteLine("Seeding questions if needed...");
                _quizRepo.SeedQuestions();

                // Then get all questions
                var allQuestions = _quizRepo.GetAll();
                if (allQuestions == null)
                {
                    Console.WriteLine("ERROR: allQuestions is null. Possibly DB connection problem.");
                    _questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Prompt = "Error: Database returned null. Check DB connection.",
                            Category = category,
                            Difficulty = difficulty
                        }
                    };
                }
                else
                {
                    Console.WriteLine($"Found {allQuestions.Count} total quiz questions in DB.");

                    // Filter by category + difficulty
                    var filtered = allQuestions
                        .Where(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase)
                                    && q.Difficulty == difficulty)
                        .ToList();

                    if (filtered.Count == 0)
                    {
                        // fallback if no matches
                        filtered.Add(new QuizQuestion
                        {
                            Prompt = "No questions found for this category/difficulty.",
                            Category = category,
                            Difficulty = difficulty
                        });
                    }

                    _questions = filtered;
                }

                // Initialize
                _currentIndex = 0;
                CurrentQuestion = _questions[_currentIndex].Prompt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in QuizViewModel constructor: {ex.Message}");
                // Fallback
                _questions = new List<QuizQuestion>
                {
                    new QuizQuestion
                    {
                        Prompt = "Exception occurred. Check console for details.",
                        Category = category,
                        Difficulty = difficulty
                    }
                };
                _currentIndex = 0;
                CurrentQuestion = _questions[_currentIndex].Prompt;
            }
        }

        private void OnNextQuestion()
        {
            if (_questions == null || _questions.Count == 0)
            {
                Console.WriteLine("OnNextQuestion called, but no questions loaded.");
                CurrentQuestion = "No questions available.";
                return;
            }

            _currentIndex = (_currentIndex + 1) % _questions.Count;
            CurrentQuestion = _questions[_currentIndex].Prompt;
            Console.WriteLine($"Next question index: {_currentIndex} => {CurrentQuestion}");
        }

        private async Task OnFetchJoke()
        {
            try
            {
                Console.WriteLine("Fetching Chuck Norris joke...");
                JokeText = await _jokeApiService.GetRandomJokeAsync();
                Console.WriteLine($"Joke fetched: {JokeText}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching joke: {ex.Message}");
                JokeText = "Error fetching joke. Check console.";
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
