using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Utilities.Core.Services
{
    public interface IJsonHttpClientService
    {
        Task<TMessage> Get<TMessage>(string url, Dictionary<string, string> headers = null);

        Task<TResponse> Post<TMessage, TResponse>(TMessage message, Dictionary<string, string> headers = null);
    }
}

