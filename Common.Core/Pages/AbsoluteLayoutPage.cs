using System;
using Xamarin.Forms;

namespace Common.Core
{
    public class AbsoluteLayoutPage<T> : BoundPage<T>
     where T : ObservableViewModel, new()
    {
        private AbsoluteLayout layout;
        private View content;
        public new View Content
        {
            get { return this.content; }
            set
            {
                if (this.content != null)
                    this.layout.Children.Remove(this.content);

                this.content = value;
                AbsoluteLayout.SetLayoutBounds(content, new Rectangle(1, 1, 1, 1));
                AbsoluteLayout.SetLayoutFlags(content, AbsoluteLayoutFlags.All);
                this.layout.Children.Add(this.content);
            }
        }

        public AbsoluteLayout AbsoluteLayer
        {
            get { return layout; }
            set { layout = value; }
        }


        public AbsoluteLayoutPage()
        {
            base.Content = this.layout = new AbsoluteLayout() { };
        }

        protected override void OnAppearing()
        {
            this.SizeChanged += PageSizeChanged;
            base.OnAppearing();
            PageSizeChanged(this, null);
        }
        protected override void OnDisappearing()
        {
            this.SizeChanged -= PageSizeChanged;
            base.OnDisappearing();
        }

        public virtual void PageSizeChanged(object sender, EventArgs args) { }

        public void Dispose()
        {
            this.SizeChanged -= PageSizeChanged;
        }

    }
}

