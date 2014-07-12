using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public interface ILoadableViewModel : IBaseViewModel
    {
        IReactiveCommand LoadCommand { get; }
    }
}

