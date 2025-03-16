using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;
using QuizApp2.Views;

namespace QuizApp2.ViewModels
{
    public class JoinSessionViewModel : INotifyPropertyChanged
    {
        private readonly GenericRepository<Session> _sessionRepo;
        private readonly GenericRepository<SessionParticipant> _spRepo;

        private string _sessionCode;
        public string SessionCode
        {
            get => _sessionCode;
            set { _sessionCode = value; OnPropertyChanged(); }
        }

        public ICommand JoinCommand { get; }
        public ICommand ReturnCommand { get; }

        public JoinSessionViewModel()
        {
            _sessionRepo = MauiProgram.CreateMauiApp().Services.GetService<GenericRepository<Session>>();
            _spRepo = MauiProgram.CreateMauiApp().Services.GetService<GenericRepository<SessionParticipant>>();

            JoinCommand = new Command(async () => await JoinSession());
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private async Task JoinSession()
        {
            try
            {
                var user = App.LoggedInUser;
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No user logged in", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(SessionCode))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Enter a session code", "OK");
                    return;
                }

                var sessions = _sessionRepo.GetAll();
                if (sessions == null || sessions.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No sessions in DB", "OK");
                    return;
                }

                var foundSession = sessions.FirstOrDefault(s => s.SessionCode == SessionCode);
                if (foundSession == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Session not found", "OK");
                    return;
                }

                // bridging record
                var newParticipant = new SessionParticipant
                {
                    SessionId = foundSession.Id,
                    UserId = user.Id
                };
                _spRepo.Save(newParticipant);

                await Application.Current.MainPage.DisplayAlert("Success", 
                    $"Joined session {foundSession.Id}!", 
                    "OK");

                // Immediately go to the session
                await Application.Current.MainPage.Navigation.PushAsync(new SessionPage(foundSession.Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JoinSession error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
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
