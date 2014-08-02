using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Android.Services
{
    public class StatusIndicatorService : IStatusIndicatorService
    {
        public void Show(string text)
        {
            throw new NotImplementedException();
        }

        public void ShowSuccess(string text)
        {
            throw new NotImplementedException();
        }

        public void ShowError(string text)
        {
            throw new NotImplementedException();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }
    }
}