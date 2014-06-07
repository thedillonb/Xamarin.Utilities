using System;
using ReactiveUI;

namespace Xamarin.Utilities.Core.ViewModels
{
    public interface IBaseViewModel
    {
        IReactiveCommand DismissCommand { get; }

        IViewFor View { get; set; }
    }
}

