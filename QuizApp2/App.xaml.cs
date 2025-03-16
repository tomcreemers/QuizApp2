namespace QuizApp2
{
    public partial class App : Application
    {
        public static int LoggedInUserId { get; set; } // default 0 if none

        public App()
        {
            InitializeComponent();

            // Always start with the Login page
            MainPage = new NavigationPage(new Views.LoginPage());
        }
    }
}
