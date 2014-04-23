using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Utilities.Core.Services
{
    public class JsonHttpClientService : IJsonHttpClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public JsonHttpClientService(IHttpClientService httpClientService, IJsonSerializationService jsonSerializationService)
        {
            _httpClientService = httpClientService;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<TMessage> Get<TMessage>(string url, Dictionary<string, string> headers = null)
        {
            var client = _httpClientService.Create();
            client.Timeout = new TimeSpan(0, 0, 30);
            if (headers != null)
                foreach (var kv in headers)
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
            var response = await client.GetAsync(url);
            var data = await response.Content.ReadAsStringAsync();
            return _jsonSerializationService.Deserialize<TMessage>(data);
        }

        public Task<TResponse> Post<TMessage, TResponse>(TMessage message, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }
    }
}