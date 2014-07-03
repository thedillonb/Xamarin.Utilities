using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace Xamarin.Utilities.DialogElements
{
    public class InputElement : EntryElement
    {
        public InputElement(string caption, string placeholder, string value)
            : base(caption, placeholder, value)
        {
            TitleFont = StyledStringElement.DefaultTitleFont.WithSize(StyledStringElement.DefaultTitleFont.PointSize);
            EntryFont = UIFont.SystemFontOfSize(14);
            TitleColor = StyledStringElement.DefaultTitleColor;
        }
    }
}

