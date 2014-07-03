using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using Xamarin.Utilities.DialogElements;
using Xamarin.Utilities.Core.ViewModels;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Xamarin.Utilities.ViewControllers
{
    public class ViewModelDialogViewController<TViewModel> : ReactiveUI.Cocoa.ReactiveTableViewController, IViewFor<TViewModel> where TViewModel : class, IBaseViewModel
	{
        protected readonly INetworkActivityService NetworkActivityService = IoC.Resolve<INetworkActivityService>();
        private UIRefreshControl _refreshControl;
        private bool _loaded;
        private readonly RootElement _root;
        private UITableView _tableView;
        private Source _tableSource;
        private readonly bool _unevenRows;
        private Subject<PointF> _scrolledSubject = new Subject<PointF>();

        public IObservable<PointF> Scrolled { get { return _scrolledSubject; } }

        public RootElement Root 
        { 
            get { return _root; } 
        }

        public ViewModelDialogViewController (bool unevenRows = false, UITableViewStyle style = UITableViewStyle.Grouped) 
        {
            _unevenRows = unevenRows;
            _tableView = new UITableView(UIScreen.MainScreen.Bounds, style);
            _root = new RootElement(_tableView);
            NavigationItem.BackBarButtonItem = new UIBarButtonItem() { Title = string.Empty };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var loadableViewModel = ViewModel as LoadableViewModel;
            if (loadableViewModel != null)
            {
                _refreshControl = new UIRefreshControl();
                RefreshControl = _refreshControl;
                _refreshControl.ValueChanged += (s, e) => loadableViewModel.LoadCommand.ExecuteIfCan();
                loadableViewModel.LoadCommand.IsExecuting.Where(x => !x).Subscribe(x => _refreshControl.EndRefreshing());
            }
        }

		public class Source : UITableViewSource 
        {
            protected ViewModelDialogViewController<TViewModel> Container;
			protected RootElement Root;

            public Source (ViewModelDialogViewController<TViewModel> container)
			{
				this.Container = container;
                Root = container.Root;
			}

			public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
			{
				var section = Root [indexPath.Section];
				var element = (section[indexPath.Row] as StyledStringElement);
				if (element != null)
					element.AccessoryTap ();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
                return Root[section].Count;
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return Root.Count;
			}

			public override string TitleForHeader (UITableView tableView, int section)
			{
				return Root[section].Header;
			}

			public override string TitleForFooter (UITableView tableView, int section)
			{
				return Root[section].Footer;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var section = Root[indexPath.Section];
				var element = section[indexPath.Row];
				return element.GetCell (tableView);
			}

			public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			{
//				if (Root.NeedColorUpdate){
//					var section = Root[indexPath.Section];
//					var element = section [indexPath.Row];
//					var colorized = element as IColorizeBackground;
//					if (colorized != null)
//						colorized.WillDisplay (tableView, cell, indexPath);
//				}
			}

			public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
			{
                var section = Root[indexPath.Section];
                var element = section[indexPath.Row];
                element.Deselected (tableView, indexPath);
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
                var section = Root[indexPath.Section];
                var element = section[indexPath.Row];
                element.Selected (tableView, indexPath);
			}			

			public override UIView GetViewForHeader (UITableView tableView, int sectionIdx)
			{
				var section = Root[sectionIdx];
				return section.HeaderView;
			}

			public override float GetHeightForHeader (UITableView tableView, int sectionIdx)
			{
				var section = Root[sectionIdx];
				return section.HeaderView == null ? -1 : section.HeaderView.Frame.Height;
			}

			public override UIView GetViewForFooter (UITableView tableView, int sectionIdx)
			{
				var section = Root[sectionIdx];
				return section.FooterView;
			}

			public override float GetHeightForFooter (UITableView tableView, int sectionIdx)
			{
				var section = Root[sectionIdx];
				return section.FooterView == null ? -1 : section.FooterView.Frame.Height;
			}

			public override void Scrolled (UIScrollView scrollView)
			{
                Container._scrolledSubject.OnNext(Container._tableView.ContentOffset);
			}

            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                if (Container._unevenRows)
                {
                    var section = Root[indexPath.Section];
                    var element = section[indexPath.Row];
                    var sizable = element as IElementSizing;
                    return sizable == null ? tableView.RowHeight : sizable.GetHeight(tableView, indexPath);
                }

                return tableView.RowHeight;
            }

            public override float EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
            {
                return Container._unevenRows ? UITableView.AutomaticDimension : -1;
            }
		}

		public override void LoadView ()
		{
            base.LoadView();

            _tableSource = CreateSizingSource(_unevenRows);

            _tableView.Bounds = View.Bounds;
            _tableView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			_tableView.AutosizesSubviews = true;
            _tableView.Source = _tableSource;
            TableView = _tableView;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

            var activatable = ViewModel as ISupportsActivation;
            if (activatable != null)
                activatable.Activator.Activate();

            if (!_loaded)
            {
                _loaded = true;
                var loadableViewModel = ViewModel as LoadableViewModel;
                if (loadableViewModel != null)
                    loadableViewModel.LoadCommand.ExecuteIfCan();
            }
		}

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            var activatable = ViewModel as ISupportsActivation;
            if (activatable != null)
                activatable.Activator.Deactivate();
        }

		public virtual Source CreateSizingSource (bool unevenRows)
		{
			return new Source (this);
		}

		public void ReloadData()
		{
			_tableView.ReloadData();
		}

        private TViewModel _viewModel;
        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set { RaiseAndSetIfChanged(ref _viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }
	}
}