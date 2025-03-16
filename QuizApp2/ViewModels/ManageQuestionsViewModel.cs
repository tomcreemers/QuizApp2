using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;

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
            // Hardcode categorieën (of haal ze ergens anders vandaan)
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

            // Haal de repository op
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

            // Eventuele melding
            await Application.Current.MainPage.DisplayAlert("Sukses", "Vraag opgeslagen!", "OK");

            // Reset velden
            Prompt = string.Empty;
            SelectedCategory = null;
            SelectedDifficulty = 1;
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
