using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        public IReactiveCommand DismissCommand { get; private set; }

        protected BaseViewModel()
        {
            DismissCommand = new ReactiveCommand();
        }
    }
}