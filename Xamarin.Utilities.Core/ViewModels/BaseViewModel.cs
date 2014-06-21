using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Utilities.Core.Services;
using System;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        public IReactiveCommand DismissCommand { get; private set; }

        public IReactiveCommand GoToUrlCommand { get; private set; }

        public IViewFor View { get; set; }

        protected BaseViewModel()
        {
            DismissCommand = new ReactiveCommand();

            GoToUrlCommand = new ReactiveCommand();
            GoToUrlCommand.OfType<string>().Subscribe(x =>
            {
                var vm = CreateViewModel<WebBrowserViewModel>();
                vm.Url = x;
                ShowViewModel(vm);
            });
        }

        public IBaseViewModel CreateViewModel(Type type)
        {
            return IoC.Resolve(type) as IBaseViewModel;
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : class
        {
            return GetService<TViewModel>();
        }

        public void ShowViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, IBaseViewModel
        {
            var view = GetService<IViewModelViewService>().GetViewFor(viewModel);
            viewModel.View = view;
            view.ViewModel = viewModel;
            GetService<ITransitionOrchestrationService>().Transition(View, view);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>An instance of the service.</returns>
        protected TService GetService<TService>() where TService : class
        {
            return IoC.Resolve<TService>();
        }
    }
}