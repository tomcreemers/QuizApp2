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
            _category = category;
            _difficulty = difficulty;

            _quizRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<QuizQuestion>>();

            _jokeApiService = new JokeApiService();

            // Seed if needed
            _quizRepo.SeedQuestions();

            // Filter questions by category & difficulty
            var allQuestions = _quizRepo.GetAll();
            var filtered = allQuestions
                .FindAll(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase)
                              && q.Difficulty == difficulty);

            if (filtered.Count == 0)
            {
                // Fallback question if no matches
                filtered.Add(new QuizQuestion
                {
                    Prompt = "No questions found for this category/difficulty.",
                    Category = category,
                    Difficulty = difficulty
                });
            }

            _questions = filtered;
            _currentIndex = 0;
            CurrentQuestion = _questions[_currentIndex].Prompt;

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
