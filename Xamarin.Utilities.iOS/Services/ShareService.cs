using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.Utilities.Services
{
	public class ShareService : IShareService
    {
		public void ShareUrl(Uri uri)
		{
			var item = new NSUrl(uri.AbsoluteUri);
			var activityItems = new NSObject[] { item };
			UIActivity[] applicationActivities = null;
			var activityController = new UIActivityViewController (activityItems, applicationActivities);
            UIApplication.SharedApplication.Delegate.Window.RootViewController.PresentViewController(activityController, true, null);
		}
    }
}

