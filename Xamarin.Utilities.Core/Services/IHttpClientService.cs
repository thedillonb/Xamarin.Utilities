using System.Net.Http;

namespace Xamarin.Utilities.Core.Services
{
    public interface IHttpClientService
    {
        HttpClient Create();
    }
}

