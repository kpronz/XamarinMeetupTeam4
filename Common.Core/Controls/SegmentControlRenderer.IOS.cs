#if __IOS__
using System.ComponentModel;
using Common.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SegmentControl), typeof(SegmentControlRenderer))]
namespace Common.Core
{
    public class SegmentControlRenderer : ViewRenderer
    {
        private SegmentControl _formControl
        {
            get { return Element as SegmentControl; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            this.InitializeFrom(_formControl);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            this.UpdateFrom(_formControl, e.PropertyName);
        }
    }

    public static class UIViewExtensions
    {
        public static void InitializeFrom(this UIView nativeControl, SegmentControl formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            nativeControl.Layer.MasksToBounds = true;
            nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius;
            nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
        }

        public static void UpdateFrom(this UIView nativeControl, SegmentControl formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == SegmentControl.CornerRadiusProperty.PropertyName)
            {
                nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius;
            }

            if (propertyChanged == SegmentControl.BorderColorProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }

            if (propertyChanged == SegmentControl.BorderThicknessProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }
        }

        public static void UpdateBorder(this UIView nativeControl, Color color, int thickness)
        {
            nativeControl.Layer.BorderColor = color.ToCGColor();
            nativeControl.Layer.BorderWidth = thickness;
            nativeControl.BackgroundColor = color.ToUIColor();
        }
    }
}
#endif

