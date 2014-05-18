namespace Xamarin.Utilities.Core.Services
{
    public interface IEnvironmentalService
    {
        string OSVersion { get; }

        string ApplicationVersion { get; }

        string DeviceName { get; }
    }
}

