using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace Xamarin.Utilities.Extensions
{
    public static class UIImageExtensions
    {
        public static UIImage ConvertToGrayScale(this UIImage This)
        {
            var imageRect = new RectangleF(PointF.Empty, This.Size);
            using (var colorSpace = CGColorSpace.CreateDeviceGray())
            using (var context = new CGBitmapContext(IntPtr.Zero, (int)imageRect.Width, (int)imageRect.Height, 8, 0, colorSpace, CGImageAlphaInfo.None))
            {
                context.DrawImage(imageRect, This.CGImage);
                using (var imageRef = context.ToImage())
                    return new UIImage(imageRef);
            }
        }
    }
}