using Newtonsoft.Json;

namespace Xamarin.Utilities.Core.Services
{
    public class JsonSerializationService : IJsonSerializationService
    {
        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public TData Deserialize<TData>(string data)
        {
            return JsonConvert.DeserializeObject<TData>(data);
        }
    }
}