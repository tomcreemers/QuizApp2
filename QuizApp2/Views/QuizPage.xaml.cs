using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class QuizPage : ContentPage
    {
        public QuizPage(string category)
        {
            InitializeComponent();
            BindingContext = new QuizViewModel(category);
        }
    }
}
