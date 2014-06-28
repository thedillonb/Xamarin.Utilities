using MonoTouch.UIKit;

namespace Xamarin.Utilities.Images
{
    public static class Images
    {
        public static UIImage BackChevron { get { return UIImageHelper.FromFileAuto("Xamarin.Utilities.bundle/back"); } }
        public static UIImage ForwardChevron { get { return UIImageHelper.FromFileAuto("Xamarin.Utilities.bundle/forward"); } }
    }
}

