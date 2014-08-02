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
    public class EnvironmentalService : IEnvironmentalService
    {
        public string OSVersion { get; private set; }
        public string ApplicationVersion { get; private set; }
        public string DeviceName { get; private set; }
    }
}