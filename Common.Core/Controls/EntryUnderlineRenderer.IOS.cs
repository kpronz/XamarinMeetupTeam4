#if __IOS__
using System;
using Common.Core;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryUnderline), typeof(EntryUnderlineRenderer))]
namespace Common.Core
{
    public class EntryUnderlineRenderer : EntryRenderer
    {
        private CALayer bottomBorder;
        private CGColor controlColor;
        private EntryUnderline formControl;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;

                formControl = (Element as EntryUnderline);
                controlColor = formControl.EntryColor.ToCGColor();

                var ctrl = (UITextField)Control;
                ctrl.TextColor = formControl.EntryColor.ToUIColor();
                ctrl.TintColor = formControl.EntryColor.ToUIColor();

                var fontSize = ctrl.Font.PointSize;
                var s1 = fontSize + 2;

                if (formControl.Icon != null)
                {
                    if (formControl.Icon.IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1)
                        formControl.Icon = formControl.Icon + ".png";

                    var imgView = new UIImageView(new CGRect(0, 0, (fontSize), (fontSize)));
                    imgView.Image = ChangeImageColor(new UIImage(formControl.Icon), formControl.EntryColor.ToUIColor());
                    Resize(imgView, fontSize);

                    var paddingView = new UIView(new CGRect(0, 0, (fontSize + 4), (fontSize + 4)));
                    paddingView.AddSubview(imgView);
                    ctrl.LeftViewMode = UITextFieldViewMode.Always;
                    ctrl.LeftView = paddingView;
                }
                //if (formControl.IsPassword)
                //{
                //    var imgView = new UIImageView(new CGRect(0, 0, (fontSize), (fontSize)));
                //    imgView.Image = ChangeImageColor(new UIImage("view.png"), formControl.EntryColor.ToUIColor());
                //    Resize(imgView, fontSize);

                //    var paddingView = new UIView(new CGRect(0, 0, (fontSize + 4), (fontSize + 4)));

                //    var btn = new UIButton(paddingView.Frame);
                //    btn.AddSubview(imgView);
                //    btn.TouchUpInside += (ee, aa) =>
                //    {
                //        formControl.IsPassword = !formControl.IsPassword;
                //    };
                //    paddingView.AddSubview(btn);
                //    ctrl.RightViewMode = UITextFieldViewMode.Always;
                //    ctrl.RightView = paddingView;
                //    ctrl.RightView.UserInteractionEnabled = true;
                //}
            }
        }


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width")
            {
                var width = ((Entry)sender).Width;
                var height = ((Entry)sender).Height;
                bottomBorder?.RemoveFromSuperLayer();
                if (width > 0 && height > 0)
                    CreateUnderline((nfloat)height, (nfloat)width);
            }
            if (e.PropertyName == "Height")
            {
                var width = ((Entry)sender).Width;
                var height = ((Entry)sender).Height;
                bottomBorder?.RemoveFromSuperLayer();
                if (width > 0 && height > 0)
                    CreateUnderline((nfloat)height, (nfloat)width);
            }
            if (e.PropertyName == "HasError")
            {
                //if (bottomBorder!=null)
                //{
                //	if (formControl.HasError)
                //		bottomBorder.BackgroundColor = UIColor.Red.CGColor;
                //	else
                //		bottomBorder.BackgroundColor = controlColor;
                //}
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void CreateUnderline(nfloat height, nfloat width)
        {
            bottomBorder = new CALayer();
            bottomBorder.Frame = new CoreGraphics.CGRect(0, height - 1, width, 1);
            bottomBorder.BackgroundColor = controlColor;
            Control.Layer.AddSublayer(bottomBorder);

        }

        private UIImage ChangeImageColor(UIImage image, UIColor color)
        {
            var rect = new CGRect(0, 0, image.Size.Width, image.Size.Height);
            UIGraphics.BeginImageContext(rect.Size);
            var ctx = UIGraphics.GetCurrentContext();
            ctx.ClipToMask(rect, image.CGImage);
            ctx.SetFillColor(color.CGColor);
            ctx.FillRect(rect);
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return UIImage.FromImage(img.CGImage, 1.0f, UIImageOrientation.DownMirrored);
        }

        private void Resize(UIImageView imgView, nfloat size)
        {
            var newSize = new CGSize(size, size);
            UIGraphics.BeginImageContextWithOptions(newSize, false, UIScreen.MainScreen.Scale);
            imgView.Image.Draw(new CGRect(0, 0, newSize.Width, newSize.Height));
            imgView.Image = UIGraphics.GetImageFromCurrentImageContext();
            imgView.ContentMode = UIViewContentMode.ScaleAspectFit;
        }
    }
}

#endif