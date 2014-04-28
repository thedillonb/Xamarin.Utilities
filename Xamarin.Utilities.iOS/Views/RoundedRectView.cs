using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.UIKit;

namespace Xamarin.Utilities.Views
{
    public class RoundedRectView : UIView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
        /// </summary>
        public RoundedRectView()
            : base()
        {
            this.fCornerRadius = 25f;
            this.eRoundedCorners = UIRectCorner.AllCorners;
            this.UpdateMask();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
        /// </summary>
        /// <param name='rect'>rectangle of the view</param>
        public RoundedRectView(RectangleF rect)
            : base(rect)
        {
            this.fCornerRadius = 25f;
            this.eRoundedCorners = UIRectCorner.AllCorners;
            this.UpdateMask();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
        /// </summary>
        /// <param name='rect'>Rectangle of the view</param>
        /// <param name='oBackgroundColor'>background color</param>
        public RoundedRectView(RectangleF rect, UIColor oBackgroundColor)
            : base(rect)
        {
            this.fCornerRadius = 25f;
            this.eRoundedCorners = UIRectCorner.AllCorners;
            this.BackgroundColor = oBackgroundColor;
            this.UpdateMask();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
        /// </summary>
        /// <param name='rect'>Rectangle of the view</param>
        /// <param name='oBackgroundColor'>background color</param>
        /// <param name='eCornerFlags'>rounded corners</param>
        public RoundedRectView(RectangleF rect, UIColor oBackgroundColor, UIRectCorner eCornerFlags)
            : base(rect)
        {
            this.fCornerRadius = 25f;
            this.eRoundedCorners = eCornerFlags;
            this.BackgroundColor = oBackgroundColor;
            this.UpdateMask();
        }

        /// <summary>
        /// Updates the layer's mask. On iOS there are no springs/struts (no auto resizing) on CALayers. Therefore the mask has to be adjusted whenever the
        /// UIView's properties are changed.
        /// </summary>
        /// </summary>
        private void UpdateMask()
        {
            // Add a layer that holds the rounded corners.
            UIBezierPath oMaskPath = UIBezierPath.FromRoundedRect(this.Bounds, this.eRoundedCorners, new SizeF(this.fCornerRadius, this.fCornerRadius));

            CAShapeLayer oMaskLayer = new CAShapeLayer();
            oMaskLayer.Frame = this.Bounds;
            oMaskLayer.Path = oMaskPath.CGPath;

            // Set the newly created shape layer as the mask for the image view's layer
            this.Layer.Mask = oMaskLayer;
        }

        /// <summary>
        /// The corner radius. Defaults to 25.0f.
        /// </summary>
        private float fCornerRadius;

        public static UIRectCorner RoundedTopCorners = UIRectCorner.TopLeft | UIRectCorner.TopRight;
        public static UIRectCorner RoundedBottomCorners = UIRectCorner.BottomLeft | UIRectCorner.BottomRight;
        public static UIRectCorner RoundedLeftCorners = UIRectCorner.TopLeft | UIRectCorner.BottomLeft;
        public static UIRectCorner RoundedRightCorners = UIRectCorner.TopRight | UIRectCorner.BottomRight;

        public override UIViewAutoresizing AutoresizingMask
        {
            get
            {
                return base.AutoresizingMask;
            }
            set
            {
                base.AutoresizingMask = value;
                this.UpdateMask();
            }
        }

        public override RectangleF Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
                this.UpdateMask();
            }
        }

        public override RectangleF Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
                this.UpdateMask();
            }
        }

        /// <summary>
        /// The round corners. Default: all corners rounded.
        /// </summary>
        public UIRectCorner eRoundedCorners;
    }
}

