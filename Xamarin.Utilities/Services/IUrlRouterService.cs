using Xamarin.Utilities.ViewModels;

namespace Xamarin.Utilities.Services
{
    public interface IUrlRouterService
    {
        IBaseViewModel Handle(string url);
    }
}

