using System;
using MonoTouch.UIKit;
using Xamarin.Utilities.Core.Services;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace Xamarin.Utilities.ViewControllers
{
    public abstract class WebViewController : UIViewController
    {
        protected readonly INetworkActivityService NetworkActivityService = IoC.Resolve<INetworkActivityService>();
        private bool _domLoaded = false;
        private List<string> _toBeExecuted = new List<string>();

        public UIWebView Web { get; private set; }

        public WebViewController()
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem() { Title = "" };
            Web = new UIWebView {ScalesPageToFit = true};
            Web.LoadFinished += OnLoadFinished;
            Web.LoadStarted += OnLoadStarted;
            Web.LoadError += OnLoadError;
            Web.ShouldStartLoad = (w, r, n) => ShouldStartLoad(r, n);
            EdgesForExtendedLayout = UIRectEdge.None;
        }

        protected virtual bool ShouldStartLoad (MonoTouch.Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
        {

            var url = request.Url;
            if(url.Scheme.Equals("app")) {
                var func = url.Host;

                if (func.Equals("ready"))
                {
                    _domLoaded = true;
                    foreach (var e in _toBeExecuted)
                        Web.EvaluateJavascript(e);
                }
                else if(func.Equals("comment")) 
                {
//                    var commentModel = _serializationService.Deserialize<JavascriptCommentModel>(UrlDecode(url.Fragment));
//                    PromptForComment(commentModel);
                }

                return false;
            }

            return true;
        }

        protected virtual void OnLoadError (object sender, UIWebErrorArgs e)
        {
            NetworkActivityService.PopNetworkActive();
        }

        protected virtual void OnLoadStarted (object sender, EventArgs e)
        {
            NetworkActivityService.PushNetworkActive();
        }

        protected virtual void OnLoadFinished(object sender, EventArgs e)
        {
            NetworkActivityService.PopNetworkActive();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(true, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Add(Web);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            Web.Frame = View.Bounds;
        }

        protected static string JavaScriptStringEncode(string data)
        {
            return System.Web.HttpUtility.JavaScriptStringEncode(data);
        }

        protected static string UrlDecode(string data)
        {
            return System.Web.HttpUtility.UrlDecode(data);
        }

        protected string LoadFile(string path)
        {
            if (path == null)
                return string.Empty;

            var uri = Uri.EscapeUriString("file://" + path) + "#" + Environment.TickCount;
            InvokeOnMainThread(() => Web.LoadRequest(new MonoTouch.Foundation.NSUrlRequest(new MonoTouch.Foundation.NSUrl(uri))));
            return uri;
        }

        protected void LoadContent(string content, string contextPath)
        {
            contextPath = contextPath.Replace("/", "//").Replace(" ", "%20");
            Web.LoadHtmlString(content, NSUrl.FromString("file:/" + contextPath + "//"));
        }

        protected void ExecuteJavascript(string data)
        {
            if (_domLoaded)
                InvokeOnMainThread(() => Web.EvaluateJavascript(data));
            else
                _toBeExecuted.Add(data);
        }

        public void GoUrl(NSUrl url)
        {
            Web.LoadRequest(new NSUrlRequest(url));
        }

        public override void ViewWillAppear(bool animated)
        {
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(false, animated);
            base.ViewWillAppear(animated);
            var bounds = View.Bounds;
            Web.Frame = bounds;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            Web.Frame = View.Bounds;
        }
    }
}

