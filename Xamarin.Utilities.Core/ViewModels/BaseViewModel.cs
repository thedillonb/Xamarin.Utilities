using ReactiveUI;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Core.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        public IReactiveCommand DismissCommand { get; private set; }

        public IViewFor View { get; set; }

        protected BaseViewModel()
        {
            DismissCommand = new ReactiveCommand();
        }

        protected TViewModel CreateViewModel<TViewModel>() where TViewModel : class
        {
            return IoC.Resolve<TViewModel>();
        }

        protected void ShowViewModel<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
        {
            var view = IoC.Resolve<IViewModelViewService>().GetViewFor(viewModel);
            viewModel.View = view;
            view.ViewModel = viewModel;
            IoC.Resolve<ITransitionOrchestrationService>().Transition(this.View, view);
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