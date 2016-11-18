using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Common.Core
{
    public class RequiredValidationBehavior : Behavior<Entry>
    {

        public static readonly BindableProperty ErrorMessageProperty =
            BindableProperty.Create("ErrorMessage",
                                    typeof(string),
                                    typeof(NumericValidationBehavior),
                                    null);
        public string ErrorMessage
        {
            get { return (string)this.GetValue(ErrorMessageProperty); }
            set { this.SetValue(ErrorMessageProperty, value); }
        }

        public static readonly BindableProperty HasErrorProperty =
            BindableProperty.Create("HasError",
                                    typeof(bool),
                                    typeof(RequiredValidationBehavior),
                                    false);

        public bool HasError
        {
            get { return (bool)base.GetValue(HasErrorProperty); }
            private set { base.SetValue(HasErrorProperty, value); }
        }

        private Action<bool, Entry> action;


        public RequiredValidationBehavior(Action<bool, Entry> action)
        {
            this.action = action;
        }
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var isValid = !string.IsNullOrEmpty(args.NewTextValue);
            HasError = !isValid;
            action?.Invoke(isValid, (Entry)sender);
        }
    }
}

