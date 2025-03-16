using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class SessionPage : ContentPage
    {
        public SessionPage(int sessionId)
        {
            InitializeComponent();
            BindingContext = new SessionViewModel(sessionId);
        }
    }
}
