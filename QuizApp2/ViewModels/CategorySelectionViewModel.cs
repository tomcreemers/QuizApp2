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
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectCategoryCommand { get; }

        public CategorySelectionViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Hardcode categories for simplicity
            Categories = new ObservableCollection<string>
            {
                "Economy",
                "Sports",
                "Tech"
            };

            // Command to handle category selection
            SelectCategoryCommand = new Command<string>(OnSelectCategory);
        }

        private async void OnSelectCategory(string category)
        {
            // Navigate to QuizPage with the chosen category
            await _navigation.PushAsync(new Views.QuizPage(category));
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
