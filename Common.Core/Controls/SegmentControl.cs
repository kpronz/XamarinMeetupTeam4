using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Common.Core
{
    public class SegmentButton : Button { }
    public class SegmentControlView : ContentView
    {
        public string DisplayTitle { get; set; }
    }
    public class SegmentControl : Grid
    {
        public List<SegmentControlView> SegmentViews { get; set; } = new List<SegmentControlView>();

        public double SegmentControlHeight { get; set; }
        private Button[] segmentButtons;

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize",
                                    typeof(double),
                                    typeof(SegmentControl),
                                    0.0);
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create("FontFamily",
                                    typeof(string),
                                    typeof(SegmentControl),
                                    null);
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(double), typeof(SegmentControl), 0.0);

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create("BorderThickness", typeof(int), typeof(SegmentControl), 0);

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor", typeof(Color), typeof(SegmentControl), Color.Black);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty SelectedBackgroundProperty =
            BindableProperty.Create("SelectedBackground",
                                    typeof(Color),
                                    typeof(SegmentControl),
                                    Color.Black);
        public Color SelectedBackground
        {
            get { return (Color)this.GetValue(SelectedBackgroundProperty); }
            set { this.SetValue(SelectedBackgroundProperty, value); }
        }

        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create("SelectedTextColor",
                                    typeof(Color),
                                    typeof(SegmentControl),
                                    Color.White);
        public Color SelectedTextColor
        {
            get { return (Color)this.GetValue(SelectedTextColorProperty); }
            set { this.SetValue(SelectedTextColorProperty, value); }
        }

        public static readonly BindableProperty UnselectedBackgroundProperty =
            BindableProperty.Create("UnselectedBackground",
                                    typeof(Color),
                                    typeof(SegmentControl),
                                    Color.Gray);
        public Color UnselectedBackground
        {
            get { return (Color)this.GetValue(UnselectedBackgroundProperty); }
            set { this.SetValue(UnselectedBackgroundProperty, value); }
        }

        public static readonly BindableProperty UnselectedTextColorProperty =
            BindableProperty.Create("UnselectedTextColor",
                                    typeof(Color),
                                    typeof(SegmentControl),
                                    Color.Black);
        public Color UnselectedTextColor
        {
            get { return (Color)this.GetValue(UnselectedTextColorProperty); }
            set { this.SetValue(UnselectedTextColorProperty, value); }
        }


        ~SegmentControl()
        {
            foreach (var btn in segmentButtons)
            {
                btn.Clicked -= ButtonsClickedEvent;
            }

        }

        public void Render()
        {
            if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
                ColumnSpacing = -6.5;
            else
                ColumnSpacing = BorderThickness;

            RowDefinitions = new RowDefinitionCollection(){
                new RowDefinition() { Height = new GridLength(SegmentControlHeight, GridUnitType.Absolute) }
            };
            segmentButtons = new Button[SegmentViews.Count];
            for (int x = 0; x < SegmentViews.Count; x++)
            {

                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                var view = SegmentViews[x];
                view.VerticalOptions = LayoutOptions.FillAndExpand;
                view.IsVisible = x == 0 ? true : false;
                var btn = new SegmentButton()
                {
                    Text = view.DisplayTitle,
                    BorderRadius = 0,

                };
                if (FontSize > 0)
                    btn.FontSize = this.FontSize;
                if (!string.IsNullOrEmpty(FontFamily))
                    btn.FontFamily = this.FontFamily;

                if (x == 0)
                    SetButton(btn, true);
                else
                    SetButton(btn, false);

                btn.Clicked += ButtonsClickedEvent;
                segmentButtons[x] = btn;
                this.Children.Add(btn, x, 0);
            }
        }

        private async void ButtonsClickedEvent(object sender, EventArgs arg)
        {
            var btn = (Button)sender;
            var idx = segmentButtons.ToList().IndexOf(btn);
            var view = SegmentViews[idx];

            for (int z = 0; z < segmentButtons.Length; z++)
            {
                var segBtn = segmentButtons[z];
                var segView = SegmentViews[z];

                if (idx == z)
                {
                    SetButton(segBtn, true);
                }
                else {
                    SetButton(segBtn, false);
                    if (segView.IsVisible)
                    {
                        await segView.FadeTo(0, 125);
                        segView.IsVisible = false;
                    }
                }
            }

            view.IsVisible = true;
            await view.FadeTo(1, 125);
        }
        private void SetButton(Button btn, bool isSelected)
        {
            if (isSelected)
            {
                btn.BackgroundColor = SelectedBackground;
                btn.TextColor = SelectedTextColor;
            }
            else {
                btn.BackgroundColor = UnselectedBackground;
                btn.TextColor = UnselectedTextColor;
            }
        }
    }
}

