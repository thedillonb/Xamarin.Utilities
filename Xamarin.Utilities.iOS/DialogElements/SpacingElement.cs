using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.Utilities.DialogElements
{
    public class SpacingElement : Element, IElementSizing
    {
        private readonly float _height;

        public SpacingElement(float height) 
        {
            _height = height;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) ?? new UITableViewCell(UITableViewCellStyle.Default, CellKey);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.SeparatorInset = UIEdgeInsets.Zero;
            cell.BackgroundColor = UIColor.Clear;
            cell.ContentView.BackgroundColor = UIColor.Clear;
            return cell;
        }

        public float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return _height;
        }
    }
}