using System;

namespace Xamarin.Utilities.Services
{
    public interface IViewModelViewService
    {
        void Register(Type viewModelType, Type viewType);

        Type GetViewFor(Type viewModel);

        void RegisterViewModels(System.Reflection.Assembly asm);
    }
}

