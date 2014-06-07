using System;
using ReactiveUI;

namespace Xamarin.Utilities.Core.Services
{
    public interface IViewModelViewService
    {
        void Register(Type viewModelType, Type viewType);

        IViewFor GetViewFor<TViewModel>(TViewModel viewModel) where TViewModel : class;

        void RegisterViewModels(System.Reflection.Assembly asm);
    }
}

