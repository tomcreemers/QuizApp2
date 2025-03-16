using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Services;

namespace QuizApp2.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private readonly JokeApiService _jokeApiService;

        // Seed a few quiz questions in-memory
        private readonly List<string> _quizQuestions = new List<string>
        {
            "What is 2 + 2?",
            "What is the capital of France?",
            "Name the largest planet in our solar system."
        };

        private int _currentQuestionIndex = 0;

        private string _currentQuestion;
        public string CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        // For the random Chuck Norris joke
        private string _jokeText;
        public string JokeText
        {
            get => _jokeText;
            set
            {
                _jokeText = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand NextQuestionCommand { get; }
        public ICommand FetchJokeCommand { get; }

        public QuizViewModel()
        {
            // Initialize the joke service
            _jokeApiService = new JokeApiService();

            // Start with the first question
            _currentQuestionIndex = 0;
            CurrentQuestion = _quizQuestions[_currentQuestionIndex];

            // Create commands
            NextQuestionCommand = new Command(OnNextQuestion);
            FetchJokeCommand = new Command(async () => await OnFetchJoke());
        }

        private void OnNextQuestion()
        {
            // Cycle to the next question
            _currentQuestionIndex = (_currentQuestionIndex + 1) % _quizQuestions.Count;
            CurrentQuestion = _quizQuestions[_currentQuestionIndex];
        }

        private async Task OnFetchJoke()
        {
            // Fetch a random Chuck Norris joke
            JokeText = await _jokeApiService.GetRandomJokeAsync();
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
