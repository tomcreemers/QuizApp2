using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            BindingContext = new AccountViewModel();
        }
    }
}
