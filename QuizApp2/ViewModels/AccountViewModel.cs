using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;

namespace QuizApp2.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
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

        public ICommand ReturnCommand { get; }

        public AccountViewModel()
        {
            // Load user from SecureStorage / DB
            LoadUser();

            // Return to previous page
            ReturnCommand = new Command(async () => 
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private void LoadUser()
        {
            var storedEmail = SecureStorage.GetAsync("email").Result;
            if (string.IsNullOrEmpty(storedEmail))
            {
                Email = "No user found in SecureStorage";
                return;
            }

            // Resolve user repository
            var userRepo = MauiProgram.CreateMauiApp()
                .Services.GetService<GenericRepository<User>>();

            var user = userRepo.GetAll()
                .FirstOrDefault(u => u.Email == storedEmail);

            if (user == null)
            {
                Email = "User not found in DB.";
            }
            else
            {
                // Show user email
                Email = $"Email: {user.Email}";
                // If user.QrCode is an SVG or base64, 
                // you could display it in a WebView or Image
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
