using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;
using QRCoder; // QRCoder package
using QuizApp2.Views;

namespace QuizApp2.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
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

        public ICommand RegisterCommand { get; }
        public ICommand ReturnCommand { get; }

        public RegisterViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Resolve the user repository
            _userRepo = MauiProgram
                .CreateMauiApp()
                .Services
                .GetService<GenericRepository<User>>();

            RegisterCommand = new Command(OnRegister);
            ReturnCommand = new Command(async () =>
            {
                await _navigation.PopAsync();
            });
        }

        private async void OnRegister()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            var existingUser = _userRepo.GetAll().FirstOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email is already registered", "OK");
                return;
            }

            // Create a new user with an SVG-based QR code
            var newUser = new User
            {
                Email = Email,
                Password = Password,
                // Generate a unique QR code as an SVG string
                QrCode = GenerateQrCode($"{Email}-{Guid.NewGuid()}")
            };

            _userRepo.Save(newUser);

            await App.Current.MainPage.DisplayAlert("Success", "Registration complete", "OK");
            await _navigation.PopAsync(); // Return to login
        }

        // Using QRCoder's SvgQRCode to generate an SVG string (no System.Drawing)
        private string GenerateQrCode(string text)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            // Create an SVG
            var svg = new SvgQRCode(data);
            // GetGraphic(10) returns an SVG string with a scale factor of 10
            // Adjust scale as you like
            var svgString = svg.GetGraphic(10);

            // Return the raw <svg> ... </svg> text
            return svgString;
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
