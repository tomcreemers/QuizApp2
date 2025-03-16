using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;

namespace QuizApp2.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly INavigation _navigation;
        private readonly GenericRepository<User> _userRepo;

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoRegisterCommand { get; }

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Resolve the user repository
            _userRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<User>>();

            LoginCommand = new Command(OnLogin);
            GoRegisterCommand = new Command(async () =>
            {
                await _navigation.PushAsync(new Views.RegisterPage());
            });
        }

        private async void OnLogin()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Fields cannot be empty", "OK");
                return;
            }

            var users = _userRepo.GetAll();
            var foundUser = users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (foundUser != null)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");

                // Go to CategorySelectionPage instead of QuizPage
                await _navigation.PushAsync(new Views.CategorySelectionPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
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
