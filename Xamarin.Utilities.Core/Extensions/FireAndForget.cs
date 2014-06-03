namespace System.Threading.Tasks
{
    public static class FireAndForgetTask
    {
        public static Task FireAndForget(this Task task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var aggException = t.Exception.Flatten();
                    foreach (var exception in aggException.InnerExceptions)
                        Diagnostics.Debug.WriteLine("Fire and Forget failed: " + exception.Message + " - " + exception.StackTrace);
                }
                else if (t.IsCanceled)
                {
                    Diagnostics.Debug.WriteLine("Fire and forget canceled.");
                }
            });
        }

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

