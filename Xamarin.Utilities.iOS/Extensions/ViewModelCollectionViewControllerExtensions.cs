using System;
using Xamarin.Utilities.ViewControllers;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using System.Linq;
using Xamarin.Utilities.Core.ViewModels;

namespace ReactiveUI
{
    public static class ViewModelCollectionViewControllerExtensions
    {
        public static void BindList<T, TItem>(this ViewModelCollectionViewController<T> @this, IReadOnlyReactiveList<TItem> list, Func<TItem, Element> selector) where T : class, IBaseViewModel
        {
            list.Changed
                .Select(_ => list)
                .Subscribe(languages => @this.Root.Reset(new Section
                {
                    languages.Select(selector)
                }));
        }
    }
}

