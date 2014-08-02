using System.Net.Http;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Android.Services
{
    public class HttpClientService : IHttpClientService
    {
        public HttpClient Create()
        {
            return new HttpClient(); //new ModernHttpClient.AFNetworkHandler()
        }
    }
}

