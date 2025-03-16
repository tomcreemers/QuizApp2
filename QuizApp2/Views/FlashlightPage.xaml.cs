using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class FlashlightPage : ContentPage
    {
        public FlashlightPage()
        {
            InitializeComponent();
            BindingContext = new FlashlightViewModel();
        }
    }
}
