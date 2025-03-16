using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel; // for Flashlight

namespace QuizApp2.ViewModels
{
    public class FlashlightViewModel : INotifyPropertyChanged
    {
        private bool _isOn;
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleFlashlightCommand { get; }

        public FlashlightViewModel()
        {
            ToggleFlashlightCommand = new Command(async () => await ToggleFlashlight());
        }

        private async Task ToggleFlashlight()
        {
            try
            {
                if (!Flashlight.Default.IsSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No flashlight available.", "OK");
                    return;
                }

                if (!IsOn)
                {
                    // Turn on
                    await Flashlight.Default.TurnOnAsync();
                    IsOn = true;
                }
                else
                {
                    // Turn off
                    await Flashlight.Default.TurnOffAsync();
                    IsOn = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Flashlight error: {ex.Message}");
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
