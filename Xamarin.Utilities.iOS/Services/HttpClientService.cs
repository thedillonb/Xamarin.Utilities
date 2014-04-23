using System.Net.Http;

namespace Xamarin.Utilities.Services
{
    public class HttpClientService : IHttpClientService
    {
        public HttpClient Create()
        {
            return new HttpClient(); //new ModernHttpClient.AFNetworkHandler()
        }
    }
}

