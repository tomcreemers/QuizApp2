using QuizApp2.ViewModels;
using QuizApp2.Views;

namespace QuizApp2.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }

        private async void OnGoToSessionClicked(object sender, EventArgs e)
        {
            // Example: Hard-coded sessionId = 1
            await Application.Current.MainPage.Navigation.PushAsync(new SessionPage(1));
        }
    }
}
