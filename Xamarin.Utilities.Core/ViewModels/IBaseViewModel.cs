using System;
using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public interface IBaseViewModel
    {
        IReactiveCommand<object> DismissCommand { get; }

        IReactiveCommand<object> GoToUrlCommand { get; }

        IViewFor View { get; set; }
    }
}

