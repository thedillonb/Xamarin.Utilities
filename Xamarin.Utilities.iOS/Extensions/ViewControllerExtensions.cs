using ReactiveUI;
using MonoTouch.UIKit;

public static class ViewControllerExtensions
{
    public static TView CreateView<TView>(this UIViewController viewController, object viewModel) where TView : class, IViewFor
    {
        var view = IoC.Resolve<TView>();
        view.ViewModel = viewModel;
        return view;
    }
}

