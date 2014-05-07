using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ReactiveUI
{
    public static class ReactiveListExtensions
    {
        public static void Reset<T>(this ReactiveList<T> @this, IEnumerable<T> items)
        {
            using (@this.SuppressChangeNotifications())
            {
                @this.Clear();
                @this.AddRange(items);
            }
        }
        
    }
}