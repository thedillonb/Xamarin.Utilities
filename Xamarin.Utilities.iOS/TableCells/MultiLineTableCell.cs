using MonoTouch.UIKit;
using System.Drawing;

namespace Xamarin.Utilities.TableCells
{
	public class MultiLineTableCell : UITableViewCell
	{
		public MultiLineTableCell (UITableViewCellStyle style, string reuseIdentifier)
			: base (style, reuseIdentifier)
		{
			BackgroundColor = UIColor.Clear;
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			
			TextLabel.Font = UIFont.SystemFontOfSize(20);
			TextLabel.LineBreakMode = UILineBreakMode.TailTruncation;
			TextLabel.Lines = 1;
			TextLabel.SizeToFit();
			
			TextLabel.Frame = new RectangleF(10, 5, Frame.Width, 21);
		}
	}
}