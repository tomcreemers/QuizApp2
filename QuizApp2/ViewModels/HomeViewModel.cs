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
        
        // Nieuwe command voor ManageQuestionsPage
        public ICommand GoManageQuestionsCommand { get; }

        public HomeViewModel()
        {
            GoAccountCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AccountPage());
            });

            GoCategoryCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CategorySelectionPage());
            });

            // Command om naar pagina te gaan waar je vragen kunt toevoegen
            GoManageQuestionsCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ManageQuestionsPage());
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
