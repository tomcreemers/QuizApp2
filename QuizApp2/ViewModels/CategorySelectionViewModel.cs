using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QuizApp2.ViewModels
{
    public class CategorySelectionViewModel : INotifyPropertyChanged
    {
        private readonly INavigation _navigation;

        private ObservableCollection<string> _categories;
        public ObservableCollection<string> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        private ObservableCollection<int> _difficulties;
        public ObservableCollection<int> Difficulties
        {
            get => _difficulties;
            set { _difficulties = value; OnPropertyChanged(); }
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

        // Command triggered by "Start Quiz" button
        public ICommand StartQuizCommand { get; }

        public CategorySelectionViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Hardcode categories
            Categories = new ObservableCollection<string>
            {
                "Economy",
                "Sports",
                "Tech"
            };

            // Hardcode difficulties 1..5
            Difficulties = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

            // Remove the forced default (or set it to 0) so user picks:
            // SelectedDifficulty = 1;  // <--- removed

            StartQuizCommand = new Command<string>(OnStartQuiz);
        }

        private async void OnStartQuiz(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            if (SelectedDifficulty < 1 || SelectedDifficulty > 5)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a difficulty (1-5).", "OK");
                return;
            }

            // Navigate to QuizPage with chosen category + difficulty
            await _navigation.PushAsync(new Views.QuizPage(category, SelectedDifficulty));
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
