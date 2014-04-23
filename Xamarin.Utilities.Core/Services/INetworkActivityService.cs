namespace Xamarin.Utilities.Core.Services
{
    public interface INetworkActivityService
    {
        void PushNetworkActive();

        void PopNetworkActive();
    }
}
