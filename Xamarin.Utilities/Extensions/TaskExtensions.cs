// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static Task ContinueInBackground<T>(this Task<T> task, Action<T> action)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled || t.Exception != null)
                    return;
                action(t.Result);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}

