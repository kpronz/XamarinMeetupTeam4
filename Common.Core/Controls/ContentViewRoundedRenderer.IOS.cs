#if __IOS__
using System;
using Common.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentViewRounded), typeof(ContentViewRoundedRenderer))]
namespace Common.Core
{
    public class ContentViewRoundedRenderer : VisualElementRenderer<ContentView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                return;
            }

            Layer.CornerRadius = (nfloat)((ContentViewRounded)Element).CornerRadius;
        }
    }
}
#endif

