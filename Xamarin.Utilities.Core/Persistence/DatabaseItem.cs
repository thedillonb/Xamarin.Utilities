namespace Xamarin.Utilities.Core.Persistence
{
    public interface IDatabaseItem<out T>
    {
        T Id { get; }
    }
}