namespace Xamarin.Utilities.Core.Services
{
    public interface IEnvironmentService
    {
        string OSVersion { get; }

        string ApplicationVersion { get; }

        string DeviceName { get; }
    }
}

