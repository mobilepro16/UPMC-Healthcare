using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using UPMC_App.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Frame), typeof(MaterialFrameRenderer))]
namespace UPMC_App.iOS
{
    public class MaterialFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            //shadow to match better material design

            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 0.80f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}