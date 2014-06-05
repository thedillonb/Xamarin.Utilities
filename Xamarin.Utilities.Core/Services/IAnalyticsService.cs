namespace Xamarin.Utilities.Core.Services
{
    public interface IAnalyticsService
    {
        bool Enabled { get; set; }

        void Init(string tracker, string name);
    }
}

