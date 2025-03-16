namespace QuizApp2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Start with a LoginPage
            MainPage = new NavigationPage(new Views.LoginPage());
        }
    }
}
