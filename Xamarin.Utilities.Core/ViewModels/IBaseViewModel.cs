using System;
using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public interface IBaseViewModel
    {
        IReactiveCommand DismissCommand { get; }

        IReactiveCommand GoToUrlCommand { get; }

        IViewFor View { get; set; }
    }
}

