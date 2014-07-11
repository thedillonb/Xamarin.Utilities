using System;
using Xamarin.Utilities.Views;
using Xamarin.Utilities.Core.ViewModels;
using MonoTouch.UIKit;
using System.Reactive.Linq;

namespace Xamarin.Utilities.ViewControllers
{
    public abstract class ViewModelPrettyDialogViewController<TViewModel> : ViewModelDialogViewController<TViewModel> where TViewModel : class, IBaseViewModel
    {
        protected SlideUpTitleView SlideUpTitle;

        protected ImageAndTitleHeaderView HeaderView;

        public override string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                base.Title = value;
                if (HeaderView != null) HeaderView.Text = base.Title;
                if (SlideUpTitle != null) SlideUpTitle.Text = base.Title;
            }
        }

        protected ViewModelPrettyDialogViewController()
            : base(true)
        {
            Scrolled.Where(x => x.Y > 0)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = null);
            Scrolled.Where(x => x.Y <= 0)
                .Where(x => NavigationController.NavigationBar.ShadowImage == null)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = new UIImage());
            Scrolled.Where(_ => SlideUpTitle != null).Subscribe(x => SlideUpTitle.Offset = 108 + 28f - x.Y);
        }

        public override void ViewWillAppear(bool animated)
        {
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(false, animated);
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.ShadowImage = new UIImage();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.NavigationBar.ShadowImage = null;
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(true, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.TitleView = SlideUpTitle = new SlideUpTitleView(NavigationController.NavigationBar.Bounds.Height) { Text = base.Title };
            SlideUpTitle.Offset = 100f;

            TableView.SectionHeaderHeight = 0;

            if (RefreshControl != null)
                RefreshControl.TintColor = UIColor.LightGray;

            HeaderView = new ImageAndTitleHeaderView 
            { 
                BackgroundColor = NavigationController.NavigationBar.BackgroundColor,
                TextColor = UIColor.White,
                SubTextColor = UIColor.LightGray,
                Text = base.Title
            };

            var topBackgroundView = this.CreateTopBackground(HeaderView.BackgroundColor);
            var loadableViewModel = ViewModel as LoadableViewModel;
            if (loadableViewModel != null)
            {
                topBackgroundView.Hidden = true;
                loadableViewModel.LoadCommand.IsExecuting.Where(x => !x).Skip(1).Take(1).Subscribe(_ => topBackgroundView.Hidden = false);
            }
        }
    }
}

