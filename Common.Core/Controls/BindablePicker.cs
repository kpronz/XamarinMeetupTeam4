using System;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Core
{
    public class PickerItem
    {
        public int ID { get; set; }
        public string OptionText { get; set; }
    }
    public class BindablePicker : Picker, IDisposable
    {
        public static readonly BindableProperty BindingPathProperty =
            BindableProperty.Create("BindingPath",
                                    typeof(string),
                                    typeof(BindablePicker),
                                    string.Empty);
        public string BindingPath
        {
            get { return (string)this.GetValue(BindingPathProperty); }
            set { this.SetValue(BindingPathProperty, value); }
        }

        public static readonly BindableProperty EntryColorProperty =
            BindableProperty.Create("EntryColor",
                                    typeof(Color),
                                    typeof(BindablePicker),
                                    Color.Black);
        public Color EntryColor
        {
            get { return (Color)this.GetValue(EntryColorProperty); }
            set { this.SetValue(EntryColorProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(propertyName: "ItemsSource",
                           returnType: typeof(IEnumerable<object>),
                           declaringType: typeof(BindablePicker),
                           defaultValue: null,
                           propertyChanged: OnItemsSourcePropertyChanged);

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }


        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(propertyName: "SelectedItem",
                           returnType: typeof(object),
                           declaringType: typeof(BindablePicker),
                           defaultValue: null,
                           defaultBindingMode: BindingMode.TwoWay,
                           propertyChanged: OnSelectedItemPropertyChanged);

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public BindablePicker()
        {
            if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
            {
                this.SelectedIndexChanged += SelectedIndexHasChanged;
            }
            else {
                this.Unfocused += OnUnfocused;
            }
        }
        ~BindablePicker()
        {
            if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
            {
                this.SelectedIndexChanged -= SelectedIndexHasChanged;
            }
            else {
                this.Unfocused -= OnUnfocused;
            }

        }
        public void Dispose()
        {
            if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
            {
                this.SelectedIndexChanged -= SelectedIndexHasChanged;
            }
            else {
                this.Unfocused -= OnUnfocused;
            }
        }
        private void OnUnfocused(object sender, EventArgs args)
        {
            if (this.SelectedIndex == -1)
                SelectedItem = null;
            else
                SelectedItem = ItemsSource.ToArray()[this.SelectedIndex];
        }
        private void SelectedIndexHasChanged(object sender, EventArgs args)
        {
            if (this.SelectedIndex == -1)
                SelectedItem = null;
            else
                SelectedItem = ItemsSource.ToArray()[this.SelectedIndex];
        }


        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object value, object newValue)
        {
            var picker = (BindablePicker)bindable;
            var notifyCollection = newValue as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += (sender, args) =>
                {
                    picker.Items.Clear();
                    foreach (var item in ItemSourceBindableList(bindable, (IEnumerable)notifyCollection))
                    {
                        picker.Items.Add((item ?? "").ToString());
                    }
                };
            }

            if (newValue == null)
                return;

            picker.Items.Clear();

            foreach (var item in ItemSourceBindableList(bindable, (IEnumerable)newValue))
                picker.Items.Add((item ?? "").ToString());
        }

        private static List<string> ItemSourceBindableList(BindableObject bindable, IEnumerable collection)
        {

            var list = new List<string>();
            var picker = (BindablePicker)bindable;
            if (!string.IsNullOrEmpty(picker.BindingPath))
            {
                var iList = collection as ICollection;

                if (collection != null && iList.Count > 0)
                {
                    PropertyInfo prop = null;
                    foreach (var obj in collection)
                    {
                        if (prop == null)
                            prop = obj.GetType().GetProperty(picker.BindingPath);

                        list.Add(prop.GetValue(obj, null).ToString());
                    }
                }
            }

            return list;

        }

        private static void OnSelectedItemPropertyChanged(BindableObject bindable, object value, object newValue)
        {
            var picker = (BindablePicker)bindable;
            if (picker.ItemsSource != null)
                picker.SelectedIndex = picker.ItemsSource.IndexOf(picker.SelectedItem);
        }
    }
}

