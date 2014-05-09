using System;
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

        public abstract Task Load();
    }
}

