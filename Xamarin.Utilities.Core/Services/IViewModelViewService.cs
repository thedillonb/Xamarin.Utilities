using System;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;

namespace Xamarin.Utilities.Core.Services
{
    public interface IViewModelViewService
    {
        void Register(Type viewModelType, Type viewType);

        IViewFor<TViewModel> GetViewFor<TViewModel>(TViewModel viewModel) where TViewModel : class;

        void RegisterViewModels(System.Reflection.Assembly asm);
    }
}

