using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class CreateSessionPage : ContentPage
    {
        public CreateSessionPage()
        {
            InitializeComponent();
            BindingContext = new CreateSessionViewModel();
        }
    }
}
