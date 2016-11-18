#if __ANDROID__
using System;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Common.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientButton), typeof(GradientButtonRenderer))]
namespace Common.Core
{
    public class GradientButtonRenderer : ButtonRenderer
    {
        GradientButton caller;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                caller = e.NewElement as GradientButton;

                var gradient = new GradientDrawable(GradientDrawable.Orientation.TopBottom, new[] {
                    caller.StartColor.ToAndroid().ToArgb(),
                    caller.EndColor.ToAndroid().ToArgb()
                });
                gradient.SetCornerRadius(caller.CornerRadius);
                SetButtonDisableState();
                Control.SetBackground(gradient);
            }
        }

        private void SetButtonDisableState()
        {
            int[][] states = new int[][] {
                new int[] { Android.Resource.Attribute.StateEnabled }, // enabled
                new int[] {-Android.Resource.Attribute.StateEnabled }, // disabled
                new int[] {-Android.Resource.Attribute.StateChecked }, // unchecked
                new int[] { Android.Resource.Attribute.StatePressed }  // pressed
            };
            int[] colors = new int[] {
                caller.TextColor.ToAndroid(),
                caller.DisabledTextColor.ToAndroid(),
                caller.TextColor.ToAndroid(),
                caller.TextColor.ToAndroid()
            };
            var buttonStates = new ColorStateList(states, colors);
            Control.SetTextColor(buttonStates);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GradientButton.IsEnabledProperty.PropertyName)
            {
                var gradient = new GradientDrawable(GradientDrawable.Orientation.TopBottom, new[] {
                    caller.StartColor.ToAndroid().ToArgb(),
                    caller.EndColor.ToAndroid().ToArgb()
                });
                gradient.SetCornerRadius(caller.CornerRadius);
                Control.SetBackground(gradient);
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}
#endif

