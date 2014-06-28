using System;
using ReactiveUI;
using System.Threading.Tasks;
using Xamarin.Utilities.Core.Services;
using System.Reactive.Linq;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class LoadableViewModel : BaseViewModel
    {
        protected readonly INetworkActivityService NetworkActivityService = IoC.Resolve<INetworkActivityService>();

        public IReactiveCommand LoadCommand { get; private set; }

        protected LoadableViewModel()
        {
            LoadCommand = new ReactiveCommand();
            LoadCommand.IsExecuting.Skip(1).Subscribe(x =>
            {
                if (x)
                    NetworkActivityService.PushNetworkActive();
                else
                    NetworkActivityService.PopNetworkActive();
            });
        }
    }
}

