using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;
using Plugin.LocalNotification; // <-- For local notifications

namespace QuizApp2.ViewModels
{
    public class ManageQuestionsViewModel : INotifyPropertyChanged
    {
        private string _prompt;
        public string Prompt
        {
            get => _prompt;
            set
            {
                _prompt = value;
                OnPropertyChanged();
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        private int _selectedDifficulty;
        public int SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<int> Difficulties { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand ReturnCommand { get; }

        private readonly GenericRepository<QuizQuestion> _quizRepo;

        public ManageQuestionsViewModel()
        {
            // Hardcode categorieën
            Categories = new ObservableCollection<string>
            {
                "Economy",
                "Sports",
                "Tech"
            };

            // Hardcode difficulties 1..5
            Difficulties = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

            // Default
            SelectedDifficulty = 1;

            // Resolve the repository
            _quizRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<QuizQuestion>>();

            SaveCommand = new Command(async () => await SaveQuestion());
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private async Task SaveQuestion()
        {
            if (string.IsNullOrWhiteSpace(Prompt))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Prompt (vraag) mag niet leeg zijn.", "OK");
                return;
            }
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Kies een categorie.", "OK");
                return;
            }

            // Nieuwe vraag aanmaken
            var newQuestion = new QuizQuestion
            {
                Prompt = Prompt,
                Category = SelectedCategory,
                Difficulty = SelectedDifficulty
            };

            // Opslaan in DB
            _quizRepo.Save(newQuestion);

            // Toon melding (alert)
            await Application.Current.MainPage.DisplayAlert("Sukses", "Vraag opgeslagen!", "OK");

            // Trigger local notification
            ShowNewQuestionNotification(newQuestion);

            // Reset velden
            Prompt = string.Empty;
            SelectedCategory = null;
            SelectedDifficulty = 1;
        }

        private void ShowNewQuestionNotification(QuizQuestion question)
        {
            var request = new NotificationRequest
            {
                NotificationId = 9001, // unique ID
                Title = "New Question Added",
                Description = $"Prompt: {question.Prompt}",
                ReturningData = "Some data", // optional
                Schedule = null // show immediately
            };

            // Show the local notification
            LocalNotificationCenter.Current.Show(request);
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
