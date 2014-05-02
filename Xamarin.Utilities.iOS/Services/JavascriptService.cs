using MonoTouch.JavaScriptCore;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Services
{
    public class JavascriptService : IJavascriptService
    {
        public IJavascriptInstance CreateInstance()
        {
            return new JavascriptInstance(new JSVirtualMachine());
        }
    }

    public class JavascriptInstance : IJavascriptInstance
    {
        private readonly JSContext _ctx;

        public JavascriptInstance(JSVirtualMachine jsVirtualMachine)
        {
            _ctx = new JSContext(jsVirtualMachine);
        }

        public string Execute(string script)
        {
            return _ctx.EvaluateScript(script).ToString();
        }
    }


}