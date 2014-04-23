namespace Xamarin.Utilities.Services
{
    public interface IEnvironmentService
    {
        string OSVersion { get; }

        string ApplicationVersion { get; }

        string DeviceName { get; }
    }
}

