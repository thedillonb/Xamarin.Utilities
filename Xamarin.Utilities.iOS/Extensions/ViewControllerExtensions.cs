using ReactiveUI;
using MonoTouch.UIKit;
using Xamarin.Utilities.Core.Services;
using Xamarin.Utilities.Core.ViewModels;

public static class ViewControllerExtensions
{
    public static IViewFor<TViewModel> CreateView<TViewModel>(this UIViewController viewController) where TViewModel : BaseViewModel
    {
        var viewModel = IoC.Resolve<TViewModel>();
        var view = IoC.Resolve<IViewModelViewService>().GetViewFor(viewModel);
        viewModel.View = view;
        view.ViewModel = viewModel;
        return (IViewFor<TViewModel>)view;
    }

    public static UIView CreateTopBackground(this UITableViewController viewController, UIColor color)
    {
        var frame = viewController.TableView.Bounds;
        frame.Y = -frame.Size.Height;
        var view = new UIView(frame);
        view.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
        view.BackgroundColor = color;
        viewController.TableView.InsertSubview(view, 0);
        return view;
    }
}

