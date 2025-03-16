using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class ManageQuestionsPage : ContentPage
    {
        public ManageQuestionsPage()
        {
            InitializeComponent();
            BindingContext = new ManageQuestionsViewModel();
        }
    }
}
