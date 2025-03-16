using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class JoinSessionPage : ContentPage
    {
        public JoinSessionPage()
        {
            InitializeComponent();
            BindingContext = new JoinSessionViewModel();
        }
    }
}
