using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizApp2.Models;
using QuizApp2.Repositories;

namespace QuizApp2.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        private readonly GenericRepository<Session> _sessionRepo;
        private readonly GenericRepository<SessionMessage> _messageRepo;
        private readonly GenericRepository<QuizQuestion> _questionRepo;

        private Session _session;

        private ObservableCollection<SessionMessageView> _messages;
        public ObservableCollection<SessionMessageView> Messages
        {
            get => _messages;
            set { _messages = value; OnPropertyChanged(); }
        }

        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set { _newMessage = value; OnPropertyChanged(); }
        }

        private string _currentQuestion;
        public string CurrentQuestion
        {
            get => _currentQuestion;
            set { _currentQuestion = value; OnPropertyChanged(); }
        }

        private string _customQuestionPrompt;
        public string CustomQuestionPrompt
        {
            get => _customQuestionPrompt;
            set { _customQuestionPrompt = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand { get; }
        public ICommand GetRandomQuestionCommand { get; }
        public ICommand AddCustomQuestionCommand { get; }
        public ICommand ReturnCommand { get; }

        public SessionViewModel(int sessionId)
        {
            try
            {
                _sessionRepo = MauiProgram.CreateMauiApp().Services.GetService<GenericRepository<Session>>();
                _messageRepo = MauiProgram.CreateMauiApp().Services.GetService<GenericRepository<SessionMessage>>();
                _questionRepo = MauiProgram.CreateMauiApp().Services.GetService<GenericRepository<QuizQuestion>>();

                // Load session from DB
                var allSessions = _sessionRepo.GetAll() ?? new List<Session>();
                _session = allSessions.FirstOrDefault(s => s.Id == sessionId)
                           ?? new Session
                           {
                               Id = sessionId,
                               SelectedCategories = "NoSessionFound",
                               Difficulty = 1
                           };

                // Load existing messages, convert them to a "view" object
                var allMsgs = _messageRepo.GetAll() ?? new List<SessionMessage>();
                var filtered = allMsgs
                    .Where(m => m.SessionId == sessionId)
                    .OrderBy(m => m.Timestamp)
                    .Select(m => new SessionMessageView
                    {
                        DisplayText = $"{m.UserName}: {m.Text}"
                    })
                    .ToList();

                Messages = new ObservableCollection<SessionMessageView>(filtered);

                // Commands
                SendMessageCommand = new Command(async () => await SendMessage());
                GetRandomQuestionCommand = new Command(async () => await GetRandomQuestion());
                AddCustomQuestionCommand = new Command(async () => await AddCustomQuestion());
                ReturnCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PopAsync();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SessionViewModel error: {ex.Message}");
                Messages = new ObservableCollection<SessionMessageView>
                {
                    new SessionMessageView { DisplayText = "Error loading session." }
                };
            }
        }

        private async Task SendMessage()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewMessage))
                    return;

                var user = App.LoggedInUser;
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No user logged in", "OK");
                    return;
                }

                var newMsg = new SessionMessage
                {
                    SessionId = _session.Id,
                    UserId = user.Id,
                    UserName = user.Email, // store user's name or email
                    Text = NewMessage,
                    Timestamp = DateTime.Now
                };

                _messageRepo.Save(newMsg);

                // add to local list
                Messages.Add(new SessionMessageView
                {
                    DisplayText = $"{newMsg.UserName}: {newMsg.Text}"
                });

                // Clear input
                NewMessage = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendMessage error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task GetRandomQuestion()
        {
            try
            {
                var cats = _session.SelectedCategories.Split(',');
                var allQ = _questionRepo.GetAll();
                if (allQ == null || allQ.Count == 0)
                {
                    CurrentQuestion = "No questions in DB";
                    return;
                }

                var filtered = allQ.Where(q =>
                {
                    var c = q.Category.Trim();
                    return cats.Any(cat => 
                        c.Equals(cat.Trim(), StringComparison.OrdinalIgnoreCase))
                        && q.Difficulty == _session.Difficulty;
                }).ToList();

                if (filtered.Count == 0)
                {
                    CurrentQuestion = "No matching questions found.";
                    return;
                }

                var rand = new Random();
                var index = rand.Next(filtered.Count);
                CurrentQuestion = filtered[index].Prompt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRandomQuestion error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task AddCustomQuestion()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustomQuestionPrompt))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Question prompt cannot be empty", "OK");
                    return;
                }

                // Use the first category from session
                var firstCat = _session.SelectedCategories
                    .Split(',')
                    .FirstOrDefault()?
                    .Trim() ?? "CustomCat";

                var newQ = new QuizQuestion
                {
                    Prompt = CustomQuestionPrompt,
                    Category = firstCat,
                    Difficulty = _session.Difficulty
                };
                _questionRepo.Save(newQ);

                await Application.Current.MainPage.DisplayAlert("Success", "Custom question added!", "OK");
                CustomQuestionPrompt = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddCustomQuestion error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }

    // A small "view" object for messages
    public class SessionMessageView
    {
        public string DisplayText { get; set; }
    }
}
