namespace Xamarin.Utilities.Services
{
    public interface IJsonSerializationService
    {
        string Serialize(object o);

        TData Deserialize<TData>(string data);
    }
}

