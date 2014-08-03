using System;
using System.Diagnostics;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Utilities.Core.Services
{
    public class ViewModelViewService : IViewModelViewService
    {
        private readonly Dictionary<Type, Type> _viewModelViewDictionary = new Dictionary<Type, Type>();

        public void RegisterViewModels(System.Reflection.Assembly asm)
        {
            foreach (var type in asm.DefinedTypes.Where(x => !x.IsAbstract && x.ImplementedInterfaces.Any(y => y == typeof(IViewFor))))
            {
                var viewForType = type.ImplementedInterfaces.FirstOrDefault(
                                      x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(IViewFor<>));
                Register(viewForType.GenericTypeArguments[0], type.AsType());
            }
        }

        public void Register(Type viewModelType, Type viewType)
        {
            #if DEBUG
            Debug.WriteLine("Registering ViewModel-To-View: {0} to {1}", viewModelType.Name, viewType.Name); 
            #endif 

            _viewModelViewDictionary.Add(viewModelType, viewType);
        }

        public IViewFor GetViewFor<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var viewType = _viewModelViewDictionary[viewModel.GetType()];
            return viewType == null ? null : (IViewFor)IoC.Resolve(viewType);

        }
    }
}

