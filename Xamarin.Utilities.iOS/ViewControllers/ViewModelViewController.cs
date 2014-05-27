using System;
using MonoTouch.UIKit;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;

namespace Xamarin.Utilities.ViewControllers
{
    public abstract class ViewModelViewController<TViewModel> : UIViewController where TViewModel : ReactiveObject
    {
        private readonly TViewModel _viewModel = IoC.Resolve<TViewModel>();

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public TViewModel ViewModel
        {
            get { return _viewModel; }
        }

        /// <summary>
        /// Gets or sets whether the super class should take care of loading
        /// </summary>
        /// <value><c>true</c> if manual load; otherwise, <c>false</c>.</value>
        protected bool ManualLoad { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (!ManualLoad)
            {
                var loadableViewModel = _viewModel as LoadableViewModel;
                if (loadableViewModel != null)
                    loadableViewModel.LoadCommand.Execute(null);
            }
        }
    }
}

