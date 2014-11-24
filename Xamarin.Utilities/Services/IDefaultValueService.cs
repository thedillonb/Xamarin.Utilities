namespace Xamarin.Utilities.Services
{
    public interface IDefaultValueService
    {
        T Get<T>(string key);

        void Set(string key, object value);
    }
}