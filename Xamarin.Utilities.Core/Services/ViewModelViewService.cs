using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Utilities.Core.ViewModels;

namespace Xamarin.Utilities.Core.Services
{
    public class ViewModelViewService : IViewModelViewService
    {
        private readonly Dictionary<Type, Type> _viewModelViewDictionary = new Dictionary<Type, Type>();

        public void RegisterViewModels(System.Reflection.Assembly asm)
        {
            foreach (var type in asm.GetTypes().Where(x => x.GetInterfaces().Any(y => y == typeof(IViewFor))))
            {
                var viewForType = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IViewFor<>));
                Register(viewForType.GetGenericArguments()[0], type);
            }
        }

        public void Register(Type viewModelType, Type viewType)
        {
            #if DEBUG
            Console.WriteLine("Registering ViewModel-To-View: {0} to {1}", viewModelType.Name, viewType.Name); 
            #endif 

            _viewModelViewDictionary.Add(viewModelType, viewType);
        }

        public IViewFor<TViewModel> GetViewFor<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var viewType = _viewModelViewDictionary[viewModel.GetType()];
            return viewType == null ? null : (IViewFor<TViewModel>)IoC.Resolve(viewType);

        }
    }
}

