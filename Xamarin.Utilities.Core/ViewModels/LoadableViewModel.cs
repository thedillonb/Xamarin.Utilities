using ReactiveUI;
using System.Threading.Tasks;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class LoadableViewModel : ReactiveObject
    {
        public IReactiveCommand LoadCommand { get; private set; }

        protected LoadableViewModel()
        {
            LoadCommand = new ReactiveCommand();
            LoadCommand.RegisterAsyncTask(x => Load());
        }

        protected abstract Task Load();
    }
}

