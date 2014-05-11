using System.Windows.Input;

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
    }
}