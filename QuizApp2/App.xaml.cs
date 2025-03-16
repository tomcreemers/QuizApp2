namespace QuizApp2
{
    public partial class App : Application
    {
        // Keep track of the user currently logged in
        public static Models.User LoggedInUser { get; set; }

        public App()
        {
            InitializeComponent();
            // Always start at LoginPage
            MainPage = new NavigationPage(new Views.LoginPage());
        }
    }
}
