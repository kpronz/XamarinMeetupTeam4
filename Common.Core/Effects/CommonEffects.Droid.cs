#if __ANDROID__
using System;
using Android.Graphics.Drawables;
using Common.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(ViewShadow), "ViewShadow")]
[assembly: ExportEffect(typeof(HideTableSeparator), "HideTableSeparator")]
namespace Common.Core
{
    public class HideTableSeparator : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
            {
                var listView = Control as global::Android.Widget.ListView;
                //listView.Divider = null;
                listView.Divider = new ColorDrawable(Android.Graphics.Color.Transparent);
                listView.DividerHeight = 0;
            }
        }

        protected override void OnDetached()
        {

        }
    }

    public class ViewShadow : PlatformEffect
    {

        protected override void OnAttached()
        {
            //Container.SetBackgroundResource(Resource.Drawable.viewshadow);
        }

        protected override void OnDetached()
        {

        }
    }
}
#endif

