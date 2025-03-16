using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Views;

namespace QuizApp2.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public ICommand GoAccountCommand { get; }
        public ICommand GoCategoryCommand { get; }
        public ICommand GoManageQuestionsCommand { get; }
        public ICommand GoCreateSessionCommand { get; }
        public ICommand GoJoinSessionCommand { get; }

        // New command: GoToSessionCommand (for an existing session ID)
        public ICommand GoToSessionCommand { get; }

        public HomeViewModel()
        {
            // Example commands you already have
            GoAccountCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AccountPage());
            });

            GoCategoryCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CategorySelectionPage());
            });

            GoManageQuestionsCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ManageQuestionsPage());
            });

            GoCreateSessionCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CreateSessionPage());
            });

            GoJoinSessionCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new JoinSessionPage());
            });

            // Command to directly navigate to SessionPage (for demonstration)
            GoToSessionCommand = new Command<int>(async (sessionId) =>
            {
                // If you know the session ID
                await Application.Current.MainPage.Navigation.PushAsync(new SessionPage(sessionId));
            });
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
