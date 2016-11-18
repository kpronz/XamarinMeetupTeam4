#if __ANDROID__
using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Common.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using DroidView = Android.Views.View;

[assembly: ExportRenderer(typeof(SegmentButton), typeof(SegmentButtonRenderer))]
[assembly: ExportRenderer(typeof(SegmentControl), typeof(SegmentControlRenderer))]
namespace Common.Core
{
    public class SegmentButtonRenderer : ButtonRenderer
    {

        private Button _formControl
        {
            get { return Element as SegmentButton; }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            var x = _formControl.BorderRadius;
            _formControl.BorderColor = Xamarin.Forms.Color.Black;
            _formControl.BorderWidth = 1;
            _formControl.BorderRadius = 1;
        }

    }
    public class SegmentControlRenderer : ViewRenderer<SegmentControl, DroidView>
    {
        private float _cornerRadius;
        private RectF _bounds;
        private Path _path;

        private SegmentControl _formControl
        {
            get { return Element as SegmentControl; }
        }

        public SegmentControlRenderer()
        {
            SetWillNotDraw(false);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SegmentControl> e)
        {
            base.OnElementChanged(e);

            var element = (SegmentControl)Element;

            _cornerRadius = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)element.CornerRadius,
                Context.Resources.DisplayMetrics);
        }



        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (w != oldw && h != oldh)
            {
                _bounds = new RectF(0, 0, w, h);
            }

            _path = new Path();
            _path.Reset();
            _path.AddRoundRect(_bounds, _cornerRadius, _cornerRadius, Path.Direction.Cw);
            _path.Close();
        }

        public override void Draw(Canvas canvas)
        {
            canvas.Save();
            canvas.ClipPath(_path);
            base.Draw(canvas);
            canvas.Restore();
        }
    }


}
#endif

