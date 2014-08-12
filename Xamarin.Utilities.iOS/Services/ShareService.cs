using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Services
{
	public class ShareService : IShareService
    {
		public void ShareUrl(string url)
		{
			var item = new NSUrl(new Uri(url).AbsoluteUri);
			var activityItems = new NSObject[] { item };
			UIActivity[] applicationActivities = null;
			var activityController = new UIActivityViewController (activityItems, applicationActivities);
            UIApplication.SharedApplication.Delegate.Window.RootViewController.PresentViewController(activityController, true, null);
		}
    }
}

