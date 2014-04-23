using System.Threading.Tasks;
using MonoTouch.UIKit;

namespace Xamarin.Utilities.Services
{
    public class AlertDialogService : IAlertDialogService
    {
        public Task<bool> PromptYesNo(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();
            var alert = new UIAlertView { Title = title, Message = message };
            alert.CancelButtonIndex = alert.AddButton("No");
            var ok = alert.AddButton("Yes");
            alert.Clicked += (sender, e) => tcs.SetResult(e.ButtonIndex == ok);
            alert.Show();
            return tcs.Task;
        }

        public Task Alert(string title, string message)
        {
            var tcs = new TaskCompletionSource<object>();
            var alert = new UIAlertView { Title = title, Message = message };
            alert.DismissWithClickedButtonIndex(alert.AddButton("Ok"), true);
            alert.Dismissed += (sender, e) => tcs.SetResult(null);
            alert.Show();
            return tcs.Task;
        }

        public Task<string> PromptTextBox(string title, string message, string defaultValue, string okTitle)
        {
            var tcs = new TaskCompletionSource<string>();
            var alert = new UIAlertView();
            alert.Title = title;
            alert.Message = message;
            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            var cancelButton = alert.AddButton("Cancel");
            var okButton = alert.AddButton(okTitle);
            alert.CancelButtonIndex = cancelButton;
            alert.DismissWithClickedButtonIndex(cancelButton, true);
            alert.GetTextField(0).Text = defaultValue;
            alert.Clicked += (s, e) =>
            {
                if (e.ButtonIndex == okButton)
                    tcs.SetResult(alert.GetTextField(0).Text);
                else
                    tcs.SetCanceled();
            };
            alert.Show();
            return tcs.Task;
        }
    }
}

