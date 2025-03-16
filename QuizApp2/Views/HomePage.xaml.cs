using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            BindingContext = new HomeViewModel();
        }
    }
}
