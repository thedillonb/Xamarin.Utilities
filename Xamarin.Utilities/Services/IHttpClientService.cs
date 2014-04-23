using System.Net.Http;

namespace Xamarin.Utilities.Services
{
    public interface IHttpClientService
    {
        HttpClient Create();
    }
}

