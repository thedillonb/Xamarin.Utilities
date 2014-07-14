using System;
using System.Windows.Input;
using Xamarin.Utilities.Core.Services;
using System.Reactive.Linq;

namespace ReactiveUI
{
    public static class CommandExtensions
    {
        public static void ExecuteIfCan(this ICommand @this, object o)
        {
            if (@this.CanExecute(o))
                @this.Execute(o);
        }

        public static void ExecuteIfCan(this ICommand @this)
        {
            ExecuteIfCan(@this, null);
        }

        public static IDisposable TriggerNetworkActivity(this IReactiveCommand @this, INetworkActivityService networkActivity)
        {
            return @this.IsExecuting.Skip(1).Subscribe(x =>
            {
                if (x) networkActivity.PushNetworkActive();
                else networkActivity.PopNetworkActive();
            });
        }
    }
}