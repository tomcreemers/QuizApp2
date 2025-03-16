using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        // The chosen category (e.g. "Economy", "Sports", "Tech")
        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private List<QuizQuestion> _questions;
        private int _currentIndex;

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

        public ICommand NextQuestionCommand { get; }
        public ICommand FetchJokeCommand { get; }

        public QuizViewModel(string category)
        {
            _category = category;

            // 1. Initialize repositories/services
            _quizRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<QuizQuestion>>();

            _jokeApiService = new JokeApiService();

            // 2. Seed if needed
            _quizRepo.SeedQuestions();

            // 3. Load questions for the chosen category
            var allQuestions = _quizRepo.GetAll();
            _questions = allQuestions
                .FindAll(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (_questions.Count == 0)
            {
                // If no questions found, put a fallback
                _questions.Add(new QuizQuestion
                {
                    Prompt = "No questions found for this category",
                    Category = category,
                    Difficulty = "N/A"
                });
            }

            // Start with index 0
            _currentIndex = 0;
            CurrentQuestion = _questions[_currentIndex].Prompt;

            // 4. Commands
            NextQuestionCommand = new Command(OnNextQuestion);
            FetchJokeCommand = new Command(async () => await OnFetchJoke());
        }

        private void OnNextQuestion()
        {
            _currentIndex = (_currentIndex + 1) % _questions.Count;
            CurrentQuestion = _questions[_currentIndex].Prompt;
        }

        private async Task OnFetchJoke()
        {
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
