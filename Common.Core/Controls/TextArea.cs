using System;
using Xamarin.Forms;

namespace Common.Core
{
    public class TextArea : Label
    {
        public static readonly BindableProperty LinksEnabledProperty =
            BindableProperty.Create("LinksEnabled",
                                    typeof(bool),
                                    typeof(TextArea),
                                    false);
        public bool LinksEnabled
        {
            get { return (bool)this.GetValue(LinksEnabledProperty); }
            set { this.SetValue(LinksEnabledProperty, value); }
        }
    }
}

