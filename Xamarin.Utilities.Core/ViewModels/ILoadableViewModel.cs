using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public interface ILoadableViewModel
    {
        IReactiveCommand LoadCommand { get; }
    }
}

