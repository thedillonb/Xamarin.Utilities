using System;
using System.Diagnostics;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Utilities.Services
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
            _viewModelViewDictionary.Add(viewModelType, viewType);
        }

        public Type GetViewFor(Type viewModel)
        {
            return !_viewModelViewDictionary.ContainsKey(viewModel) ? null : _viewModelViewDictionary[viewModel.GetType()];
        }
    }
}

