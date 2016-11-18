using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Common.Core
{
    public class RankingControl : StackLayout, IDisposable
    {
        private StarBehavior behavior5;

        public static readonly BindableProperty SelectedRankProperty =
            BindableProperty.Create("SelectedRank",
                                    typeof(int),
                                    typeof(RankingControl),
                                    0);

        public int SelectedRank
        {
            get { return (int)this.GetValue(SelectedRankProperty); }
            set
            {

                this.SetValue(SelectedRankProperty, value);

                if (behavior5 != null)
                    behavior5.Rating = value;
            }
        }

        public RankingControl()
        {
            this.HeightRequest = 44;
            var behavior1 = new StarBehavior() { GroupName = "WMAStar" };
            var gd1 = new Grid();
            var unSelectedImg1 = new Image() { Source = "star_outline.png" };
            var selectedImg1 = new Image() { Source = "star_selected.png" };
            selectedImg1.SetBinding(Image.IsVisibleProperty, new Binding(source: behavior1, path: "IsStarred"));
            gd1.Children.Add(unSelectedImg1, 0, 0);
            gd1.Children.Add(selectedImg1, 0, 0);
            gd1.Behaviors.Add(behavior1);

            var behavior2 = new StarBehavior() { GroupName = "WMAStar" };
            var gd2 = new Grid();
            var unSelectedImg2 = new Image() { Source = "star_outline.png" };
            var selectedImg2 = new Image() { Source = "star_selected.png" };
            selectedImg2.SetBinding(Image.IsVisibleProperty, new Binding(source: behavior2, path: "IsStarred"));
            gd2.Children.Add(unSelectedImg2, 0, 0);
            gd2.Children.Add(selectedImg2, 0, 0);
            gd2.Behaviors.Add(behavior2);

            var behavior3 = new StarBehavior() { GroupName = "WMAStar" };
            var gd3 = new Grid();
            var unSelectedImg3 = new Image() { Source = "star_outline.png" };
            var selectedImg3 = new Image() { Source = "star_selected.png" };
            selectedImg3.SetBinding(Image.IsVisibleProperty, new Binding(source: behavior3, path: "IsStarred"));
            gd3.Children.Add(unSelectedImg3, 0, 0);
            gd3.Children.Add(selectedImg3, 0, 0);
            gd3.Behaviors.Add(behavior3);

            var behavior4 = new StarBehavior() { GroupName = "WMAStar" };
            var gd4 = new Grid();
            var unSelectedImg4 = new Image() { Source = "star_outline.png" };
            var selectedImg4 = new Image() { Source = "star_selected.png" };
            selectedImg4.SetBinding(Image.IsVisibleProperty, new Binding(source: behavior4, path: "IsStarred"));
            gd4.Children.Add(unSelectedImg4, 0, 0);
            gd4.Children.Add(selectedImg4, 0, 0);
            gd4.Behaviors.Add(behavior4);

            behavior5 = new StarBehavior() { GroupName = "WMAStar" };
            var gd5 = new Grid();
            var unSelectedImg5 = new Image() { Source = "star_outline.png" };
            var selectedImg5 = new Image() { Source = "star_selected.png" };
            selectedImg5.SetBinding(Image.IsVisibleProperty, new Binding(source: behavior5, path: "IsStarred"));
            gd5.Children.Add(unSelectedImg5, 0, 0);
            gd5.Children.Add(selectedImg5, 0, 0);
            gd5.Behaviors.Add(behavior5);

            behavior5.PropertyChanged += RatingchangedEvent;

            this.Orientation = StackOrientation.Horizontal;
            this.Children.Add(gd1);
            this.Children.Add(gd2);
            this.Children.Add(gd3);
            this.Children.Add(gd4);
            this.Children.Add(gd5);
        }

        private void RatingchangedEvent(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Rating")
            {
                SelectedRank = behavior5.Rating;
            }
        }
        ~RankingControl()
        {
            if (behavior5 != null)
                behavior5.PropertyChanged -= RatingchangedEvent;
        }
        public void Dispose()
        {
            if (behavior5 != null)
                behavior5.PropertyChanged -= RatingchangedEvent;
        }
    }

    public class StarBehavior : Behavior<View>
    {
        TapGestureRecognizer tapRecognizer;
        static List<StarBehavior> defaultBehaviors = new List<StarBehavior>();
        static Dictionary<string, List<StarBehavior>> starGroups = new Dictionary<string, List<StarBehavior>>();

        public static readonly BindableProperty GroupNameProperty =
            BindableProperty.Create("GroupName",
                                    typeof(string),
                                    typeof(StarBehavior),
                                    null,
                                    propertyChanged: OnGroupNameChanged);

        public string GroupName
        {
            set { SetValue(GroupNameProperty, value); }
            get { return (string)GetValue(GroupNameProperty); }
        }

        public static readonly BindableProperty RatingProperty =
           BindableProperty.Create("Rating",
                                   typeof(int),
                                   typeof(StarBehavior), default(int));

        public int Rating
        {
            set { SetValue(RatingProperty, value); }
            get { return (int)GetValue(RatingProperty); }
        }

        static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StarBehavior behavior = (StarBehavior)bindable;
            string oldGroupName = (string)oldValue;
            string newGroupName = (string)newValue;

            // Remove existing behavior from Group
            if (String.IsNullOrEmpty(oldGroupName))
            {
                defaultBehaviors.Remove(behavior);
            }
            else
            {
                List<StarBehavior> behaviors = starGroups[oldGroupName];
                behaviors.Remove(behavior);

                if (behaviors.Count == 0)
                {
                    starGroups.Remove(oldGroupName);
                }
            }

            // Add New Behavior to the group
            if (String.IsNullOrEmpty(newGroupName))
            {
                defaultBehaviors.Add(behavior);
            }
            else
            {
                List<StarBehavior> behaviors = null;

                if (starGroups.ContainsKey(newGroupName))
                {
                    behaviors = starGroups[newGroupName];
                }
                else
                {
                    behaviors = new List<StarBehavior>();
                    starGroups.Add(newGroupName, behaviors);
                }

                behaviors.Add(behavior);
            }

        }


        public static readonly BindableProperty IsStarredProperty =
            BindableProperty.Create("IsStarred",
                                    typeof(bool),
                                    typeof(StarBehavior),
                                    false,
                                    propertyChanged: OnIsStarredChanged);

        public bool IsStarred
        {
            set { SetValue(IsStarredProperty, value); }
            get { return (bool)GetValue(IsStarredProperty); }
        }

        static void OnIsStarredChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StarBehavior behavior = (StarBehavior)bindable;

            if ((bool)newValue)
            {
                string groupName = behavior.GroupName;
                List<StarBehavior> behaviors = null;

                if (string.IsNullOrEmpty(groupName))
                {
                    behaviors = defaultBehaviors;
                }
                else
                {
                    behaviors = starGroups[groupName];
                }

                bool itemReached = false;
                int count = 1, position = 0;
                // all positions to left IsStarred = true and all position to the right is false
                foreach (var item in behaviors)
                {
                    if (item != behavior && !itemReached)
                    {
                        item.IsStarred = true;
                    }
                    if (item == behavior)
                    {
                        itemReached = true;
                        item.IsStarred = true;
                        position = count;
                    }
                    if (item != behavior && itemReached)
                        item.IsStarred = false;

                    item.Rating = position;
                    count++;
                }

            }


        }

        public StarBehavior()
        {
            defaultBehaviors.Add(this);
        }

        protected override void OnAttachedTo(View view)
        {
            tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnTapRecognizerTapped;
            view.GestureRecognizers.Add(tapRecognizer);
        }

        protected override void OnDetachingFrom(View view)
        {
            view.GestureRecognizers.Remove(tapRecognizer);
            tapRecognizer.Tapped -= OnTapRecognizerTapped;
        }

        void OnTapRecognizerTapped(object sender, EventArgs args)
        {
            //HACK: PropertyChange does not fire, if the value is not changed :-(
            IsStarred = false;
            IsStarred = true;
        }
    }
}

