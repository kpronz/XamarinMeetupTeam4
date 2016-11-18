using System;
using Xamarin.Forms;

namespace Common.Core
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        private Action<bool> action;

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
                                    typeof(NumericValidationBehavior),
                                    false);

        public bool HasError
        {
            get { return (bool)base.GetValue(HasErrorProperty); }
            private set { base.SetValue(HasErrorProperty, value); }
        }


        public NumericValidationBehavior(Action<bool> action)
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
            double result;
            var isValid = double.TryParse(args.NewTextValue, out result);
            HasError = !isValid;
            action?.Invoke(isValid);

        }
    }
}

