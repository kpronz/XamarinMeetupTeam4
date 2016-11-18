#if __IOS__
using System;
using Common.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TextArea), typeof(TextAreaRenderer))]
namespace Common.Core
{
    public class TextAreaRenderer : ViewRenderer<TextArea, UITextView>
    {
        private UITextView txtView;
        private TextArea parent;

        protected override void OnElementChanged(ElementChangedEventArgs<TextArea> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
                parent = e.NewElement;

            if (txtView == null)
            {
                txtView = new UITextView();
                txtView.Text = parent.Text;
                txtView.Font = parent.Font.ToUIFont();

                var txtColor = UIColor.Black;
                if ((int)parent.TextColor.R != -1)
                    txtColor = parent.TextColor.ToUIColor();

                txtView.TextColor = txtColor;
                txtView.Editable = false;
                txtView.ScrollEnabled = false;

                if (parent.LinksEnabled)
                    txtView.DataDetectorTypes = UIDataDetectorType.All;

                SetNativeControl(txtView);
            }

        }
    }
}
#endif

