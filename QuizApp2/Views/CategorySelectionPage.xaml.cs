using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class CategorySelectionPage : ContentPage
    {
        private string _selectedCategory;

        public CategorySelectionPage()
        {
            InitializeComponent();
            BindingContext = new CategorySelectionViewModel(Navigation);
        }

        private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                _selectedCategory = e.CurrentSelection[0] as string;
            }
        }

        private void OnStartQuizClicked(object sender, EventArgs e)
        {
            // We pass both category + difficulty to the VM's command
            if (BindingContext is CategorySelectionViewModel vm)
            {
                vm.StartQuizCommand.Execute(_selectedCategory);
            }
        }
    }
}
