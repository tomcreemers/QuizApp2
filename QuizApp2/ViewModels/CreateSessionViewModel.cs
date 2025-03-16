using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;
using Microsoft.Maui.ApplicationModel; // for Vibration

namespace QuizApp2.ViewModels
{
    public class CreateSessionViewModel : INotifyPropertyChanged
    {
        private readonly GenericRepository<Session> _sessionRepo;

        // Simple input for categories
        private string _categoriesInput;
        public string CategoriesInput
        {
            get => _categoriesInput;
            set { _categoriesInput = value; OnPropertyChanged(); }
        }

        // Difficulty list 1..5
        public ObservableCollection<int> Difficulties { get; set; }

        private int _selectedDifficulty;
        public int SelectedDifficulty
        {
            get => _selectedDifficulty;
            set { _selectedDifficulty = value; OnPropertyChanged(); }
        }

        public ICommand CreateSessionCommand { get; }
        public ICommand ReturnCommand { get; }

        public CreateSessionViewModel()
        {
            // Resolve the repository
            _sessionRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<Session>>();

            // Hardcode difficulties 1..5
            Difficulties = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

            // Default difficulty
            SelectedDifficulty = 1;

            CreateSessionCommand = new Command(async () => await CreateSession());
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private async Task CreateSession()
        {
            try
            {
                // Check if we have a logged-in user
                var user = App.LoggedInUser;
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No user logged in", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(CategoriesInput))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please enter categories", "OK");
                    return;
                }

                // Create new Session
                var newSession = new Session
                {
                    HostUserId = user.Id,
                    SelectedCategories = CategoriesInput, // e.g. "Economy,Tech"
                    Difficulty = SelectedDifficulty,
                    // Generate a random code (e.g. for QR)
                    SessionCode = Guid.NewGuid().ToString()
                };

                _sessionRepo.Save(newSession);

                await Application.Current.MainPage.DisplayAlert("Success", 
                    $"Session created with ID={newSession.Id}\nCode={newSession.SessionCode}", 
                    "OK");

                // Vibrate phone for 500ms after success
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));

                // Optionally navigate back or stay
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating session: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
