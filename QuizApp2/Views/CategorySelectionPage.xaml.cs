using QuizApp2.ViewModels;

namespace QuizApp2.Views
{
    public partial class CategorySelectionPage : ContentPage
    {
        public CategorySelectionPage()
        {
            InitializeComponent();
            BindingContext = new CategorySelectionViewModel(Navigation);
        }

        private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedCategory = e.CurrentSelection[0] as string;
                if (BindingContext is CategorySelectionViewModel vm)
                {
                    vm.SelectCategoryCommand.Execute(selectedCategory);
                }
            }
        }
    }
}
