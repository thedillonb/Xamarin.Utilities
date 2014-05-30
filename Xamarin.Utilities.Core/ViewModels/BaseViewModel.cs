using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        public IReactiveCommand DismissCommand { get; private set; }

        public IReactiveCommand GoToViewCommand { get; private set; }

        protected BaseViewModel()
        {
            DismissCommand = new ReactiveCommand();
            GoToViewCommand = new ReactiveCommand();
        }
    }
}