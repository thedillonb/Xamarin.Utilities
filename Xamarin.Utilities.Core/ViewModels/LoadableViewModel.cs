using System;
using ReactiveUI;
using System.Threading.Tasks;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class LoadableViewModel : BaseViewModel
    {
        protected readonly INetworkActivityService NetworkActivityService = IoC.Resolve<INetworkActivityService>();

        public IReactiveCommand LoadCommand { get; private set; }

        protected LoadableViewModel()
        {
            LoadCommand = new ReactiveCommand();
            LoadCommand.IsExecuting.Subscribe(x =>
            {
                if (x)
                    NetworkActivityService.PushNetworkActive();
                else
                    NetworkActivityService.PopNetworkActive();
            });
        }
    }
}

