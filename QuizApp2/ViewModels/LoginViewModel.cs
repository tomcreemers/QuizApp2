using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;
using QuizApp2.Views;

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
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoRegisterCommand { get; }

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Resolve user repository from DI
            _userRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<User>>();

            LoginCommand = new Command(OnLogin);
            GoRegisterCommand = new Command(async () =>
            {
                // Navigate to the RegisterPage
                await _navigation.PushAsync(new RegisterPage());
            });
        }

        private async void OnLogin()
        {
            try
            {
                // Debug line to confirm OnLogin is called
                Console.WriteLine("OnLogin triggered!");

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    Console.WriteLine("Email or password is empty.");
                    await App.Current.MainPage.DisplayAlert("Error", "Fields cannot be empty", "OK");
                    return;
                }

                // Get all users from DB
                var users = _userRepo.GetAll();
                if (users == null)
                {
                    Console.WriteLine("Users is null (DB might be empty or not created).");
                    await App.Current.MainPage.DisplayAlert("Error", "No users in DB", "OK");
                    return;
                }

                Console.WriteLine($"Found {users.Count} users in DB.");

                // Check if any user matches Email + Password
                var foundUser = users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
                if (foundUser != null)
                {
                    Console.WriteLine($"User found: {foundUser.Email}. Logging in...");

                    // Store the logged-in user in a static property (App.xaml.cs)
                    App.LoggedInUser = foundUser;

                    await App.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");

                    // Navigate to HomePage
                    await _navigation.PushAsync(new HomePage());
                }
                else
                {
                    Console.WriteLine("No user matched the provided credentials.");
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnLogin: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
