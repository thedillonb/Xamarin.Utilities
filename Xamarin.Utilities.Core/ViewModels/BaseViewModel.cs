using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Utilities.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel, ISupportsActivation
    {
        private readonly ViewModelActivator _viewModelActivator;

        public IReactiveCommand<object> DismissCommand { get; private set; }

        public IReactiveCommand<object> GoToUrlCommand { get; private set; }

        public IViewFor View { get; set; }

        protected BaseViewModel()
        {
            _viewModelActivator = new ViewModelActivator();

            DismissCommand = ReactiveCommand.Create();

            GoToUrlCommand = ReactiveCommand.Create();
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


        public TViewModel CreateViewModel<TViewModel>(object navObject) where TViewModel : class
        {
            var d = new Dictionary<string, object> {{"navObject", navObject}};
            return IoC.Resolve<TViewModel>(d);
        }

        public void ShowViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, IBaseViewModel
        {
            var view = GetService<IViewModelViewService>().GetViewFor(viewModel);
            viewModel.View = view;
            view.ViewModel = viewModel;
            GetService<ITransitionOrchestrationService>().Transition(View, view);
        }

        public void CreateAndShowViewModel<TViewModel>() where TViewModel : class, IBaseViewModel
        {
            ShowViewModel<TViewModel>(CreateViewModel<TViewModel>());
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

        ViewModelActivator ISupportsActivation.Activator
        {
            get { return _viewModelActivator; }
        }
    }
}