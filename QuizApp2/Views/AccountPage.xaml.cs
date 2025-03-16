using QuizApp2.Models;

namespace QuizApp2.Views
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadUser();
        }

        private void LoadUser()
        {
            // If you're using the "static user" approach:
            var user = App.LoggedInUser; 
            if (user == null)
            {
                EmailLabel.Text = "No user is logged in.";
                // Show a blank WebView
                QrWebView.Source = new HtmlWebViewSource { Html = "<html><body>No QR code</body></html>" };
                return;
            }

            EmailLabel.Text = $"Email: {user.Email}";

            if (!string.IsNullOrEmpty(user.QrCode))
            {
                // user.QrCode is raw <svg>..</svg>. Let's embed it in simple HTML:
                var svgHtml = $"<html><body>{user.QrCode}</body></html>";
                // Display it in the WebView
                QrWebView.Source = new HtmlWebViewSource { Html = svgHtml };
            }
            else
            {
                // No QR code found
                QrWebView.Source = new HtmlWebViewSource { Html = "<html><body>No QR code found.</body></html>" };
            }
        }

        private async void OnReturnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
