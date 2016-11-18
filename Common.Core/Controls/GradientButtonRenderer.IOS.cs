#if __IOS__
using System;
using System.ComponentModel;
using System.Linq;
using Common.Core;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientButton), typeof(GradientButtonRenderer))]
namespace Common.Core
{
    public class GradientButtonRenderer : ButtonRenderer
    {
        GradientButton caller;
        CAGradientLayer gradient;
        CALayer caLayer;

        public override void LayoutSubviews()
        {
            foreach (var layer in Control?.Layer.Sublayers.Where(layer => layer is CAGradientLayer))
                layer.Frame = GetBounds();//caller.Bounds.ToRectangleF();
            base.LayoutSubviews();
        }

        private CGRect GetBounds()
        {
            return new CGRect(0, 0, caller.Bounds.Width, caller.Bounds.Height);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                caller = e.NewElement as GradientButton;

                gradient = new CAGradientLayer();
                gradient.Frame = Control.Bounds;
                gradient.CornerRadius = Control.Layer.CornerRadius = caller.CornerRadius;

                gradient.Colors = new CGColor[]
                {
                    caller.StartColor.ToCGColor(),
                    caller.EndColor.ToCGColor(),
                };

                if (caller.ShadowColor != null)
                {
                    Control.Layer.ShadowRadius = (nfloat)caller.ShadowRadius;
                    Control.Layer.ShadowColor = caller.ShadowColor.ToCGColor();
                    Control.Layer.ShadowOffset = new CGSize(0.0f, caller.ShadowOffset);
                    Control.Layer.ShadowOpacity = caller.ShadowOpacity;
                    Control.Layer.MasksToBounds = false;

                }
                Control.SetTitleColor(caller.DisabledTextColor.ToUIColor(), UIControlState.Disabled);

                //caLayer = Control?.Layer.Sublayers.LastOrDefault();
                //Control?.Layer.InsertSublayerBelow(gradient, caLayer);

                Control?.Layer.InsertSublayer(gradient, 0);

            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GradientButton.IsEnabledProperty.PropertyName)
            {
                Control.Layer.ShadowColor = caller.ShadowColor.ToCGColor();
                gradient.Colors = new CGColor[]
                 {
                    caller.StartColor.ToCGColor(),
                    caller.EndColor.ToCGColor(),
                 };
                Control.SetNeedsDisplay();

            }
            if (e.PropertyName == "Width" || e.PropertyName == "Height")
            {
                gradient.Frame = caller.Bounds.ToRectangleF();
            }
            base.OnElementPropertyChanged(sender, e);
        }

    }
}
#endif

