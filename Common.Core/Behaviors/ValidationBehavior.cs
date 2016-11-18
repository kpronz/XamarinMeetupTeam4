using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Common.Core
{
    public class ValidationBehavior : Behavior<View>
    {
        private INotifyPropertyChanged notifier;
        private Action<View> action;
        private View view;

        public ValidationBehavior(INotifyPropertyChanged obj, Action<View> propertyChanged)
        {
            notifier = obj;
            action = propertyChanged;
        }
        protected override void OnAttachedTo(View bindable)
        {
            notifier.PropertyChanged += NotificationFired;
            view = bindable;
        }

        protected override void OnDetachingFrom(View bindable)
        {
            notifier.PropertyChanged -= NotificationFired;
        }

        private void NotificationFired(object sender, PropertyChangedEventArgs args)
        {
            action?.Invoke(view);
        }
    }
}

