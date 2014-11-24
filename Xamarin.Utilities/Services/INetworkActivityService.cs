using System;

namespace Xamarin.Utilities.Services
{
    public interface INetworkActivityService
    {
        void PushNetworkActive();

        void PopNetworkActive();
    }
}
