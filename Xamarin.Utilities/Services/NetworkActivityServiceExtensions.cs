using System;
using System.Reactive.Disposables;

namespace Xamarin.Utilities.Services
{
    public static class NetworkActivityServiceExtensions
    {
        public static IDisposable ActivateNetwork(this INetworkActivityService @this)
        {
            @this.PushNetworkActive();
            return Disposable.Create(@this.PopNetworkActive);
        }
    }
}

