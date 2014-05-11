using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        public IReactiveCommand DismissCommand { get; private set; }

        public BaseViewModel()
        {
            DismissCommand = new ReactiveCommand();
        }
    }
}