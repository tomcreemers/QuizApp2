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

        // Add this property for creating a session
        public ICommand GoCreateSessionCommand { get; }

        public HomeViewModel()
        {
            // Account
            GoAccountCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AccountPage());
            });

            // Category selection
            GoCategoryCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CategorySelectionPage());
            });

            // Manage questions
            GoManageQuestionsCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ManageQuestionsPage());
            });

            // Create session
            GoCreateSessionCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CreateSessionPage());
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
