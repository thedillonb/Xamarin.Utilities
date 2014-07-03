using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Xamarin.Utilities.DialogElements
{
    public interface IElementSizing 
    {
        float GetHeight (UITableView tableView, NSIndexPath indexPath);
    }
}

