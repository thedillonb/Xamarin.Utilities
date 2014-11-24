using ReactiveUI;

namespace Xamarin.Utilities.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        private readonly ViewModelActivator _viewModelActivator;

        private string _title;
        public string Title
        {
            get { return _title; }
            protected set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        protected BaseViewModel()
        {
            _viewModelActivator = new ViewModelActivator();
        }

        ViewModelActivator ISupportsActivation.Activator
        {
            get { return _viewModelActivator; }
        }
    }
}