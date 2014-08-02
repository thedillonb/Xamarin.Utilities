using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Android.Services
{
    public class AlertDialogService : IAlertDialogService
    {
        private readonly Context _context;

        public AlertDialogService(Context context)
        {
            _context = context;
        }

        public Task<bool> PromptYesNo(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();
            var alert = new AlertDialog.Builder(_context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton("Yes", (sender, args) => tcs.SetResult(true))
                .SetNegativeButton("No", (sender, args) => tcs.SetResult(false));
            alert.Create().Show();
            return tcs.Task;
        }

        public Task Alert(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();
            var alert = new AlertDialog.Builder(_context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton("Ok", (sender, args) => tcs.SetResult(true));
            alert.Create().Show();
            return tcs.Task;
        }

        public Task<string> PromptTextBox(string title, string message, string defaultValue, string okTitle)
        {
            throw new NotImplementedException();
        }
    }
}