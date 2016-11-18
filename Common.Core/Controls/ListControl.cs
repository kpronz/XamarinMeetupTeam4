using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections;

namespace Common.Core
{
    public class ListControl : ListView, IDisposable
    {
        public bool MaintainSelection { get; set; }

        public static readonly BindableProperty ScrollIndexProperty =
                BindableProperty.Create(propertyName: "ScrollIndex",
                                        returnType: typeof(int),
                                        declaringType: typeof(ListControl),
                                        defaultValue: -1,
                                        propertyChanged: ScrollIndexPropertyChanged);

        public static void ScrollIndexPropertyChanged(BindableObject bindable, object oldValue, object newvalue)
        {
            ((ListControl)bindable).ScrollToIndex();
        }
        public int ScrollIndex
        {
            get { return (int)this.GetValue(ScrollIndexProperty); }
            set { this.SetValue(ScrollIndexProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty =
            BindableProperty.Create("ItemClickCommand",
                                    typeof(ICommand),
                                    typeof(ListControl),
                                    null);
        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { this.SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create("LoadMoreCommand",
                                    typeof(ICommand),
                                    typeof(ListControl),
                                    null);
        public ICommand LoadMoreCommand
        {
            get { return (ICommand)this.GetValue(LoadMoreCommandProperty); }
            set { this.SetValue(LoadMoreCommandProperty, value); }
        }

        public ListControl() : base(ListViewCachingStrategy.RetainElement)
        {
            this.ItemTapped += this.OnItemTapped;
            this.ItemAppearing += this.OnItemAppearing;
        }

        ~ListControl()
        {
            this.ItemTapped -= this.OnItemTapped;
            this.ItemAppearing -= this.OnItemAppearing;
        }

        public void Dispose()
        {
            this.ItemTapped -= this.OnItemTapped;
            this.ItemAppearing -= this.OnItemAppearing;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var execute = ItemClickCommand?.CanExecute(e);
                if (execute.HasValue && execute.Value)
                    ItemClickCommand?.Execute(e.Item);

                if (!MaintainSelection)
                    this.SelectedItem = null;
            }
        }
        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemsSource != null && e.Item != null)
            {
                var items = ItemsSource as IList;

                if (e.Item == items[items.Count - 1])
                {
                    var execute = LoadMoreCommand?.CanExecute(e);
                    if (execute.HasValue && execute.Value)
                        LoadMoreCommand?.Execute(e.Item);
                }
            }
        }
        private void ScrollToIndex()
        {
            if (ItemsSource != null)
            {
                var list = (IList)ItemsSource;
                if (list.Count > ScrollIndex && ScrollIndex < (list.Count + 1))
                {
                    var obj = list[ScrollIndex];
                    ScrollTo(obj, ScrollToPosition.Start, true);
                }
            }

        }
    }
}

