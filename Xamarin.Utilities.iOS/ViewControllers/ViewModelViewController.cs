using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using MonoTouch.Foundation;
using System;

namespace Xamarin.Utilities.ViewControllers
{
    public abstract class ViewModelViewController<TViewModel> : ReactiveUI.Cocoa.ReactiveViewController, IViewFor<TViewModel> where TViewModel : class, IBaseViewModel
    {
        public TViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }

        protected bool ManualLoad { get; set; }

        protected ViewModelViewController()
        {
        }

        protected ViewModelViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected ViewModelViewController(string nibNameOrNull, NSBundle nibBundleOrNull)
            : base(nibNameOrNull, nibBundleOrNull)
        {
        }

        private bool _isLoaded;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!ManualLoad && !_isLoaded)
            {
                var loadableViewModel = ViewModel as LoadableViewModel;
                if (loadableViewModel != null)
                    loadableViewModel.LoadCommand.ExecuteIfCan();
                _isLoaded = true;
            }
        }
    }
}

