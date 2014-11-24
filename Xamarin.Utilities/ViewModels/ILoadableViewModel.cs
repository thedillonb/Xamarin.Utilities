using ReactiveUI;
using System.Reactive;

namespace Xamarin.Utilities.ViewModels
{
    public interface ILoadableViewModel : IBaseViewModel
    {
        IReactiveCommand<Unit> LoadCommand { get; }
    }
}

