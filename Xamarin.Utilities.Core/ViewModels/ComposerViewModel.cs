using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public class ComposerViewModel : BaseViewModel
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { this.RaiseAndSetIfChanged(ref _text, value); }
        }

        public IReactiveCommand<object> SaveCommand { get; private set; }

        public ComposerViewModel()
        {
            SaveCommand = ReactiveCommand.Create();
        }
    }
}