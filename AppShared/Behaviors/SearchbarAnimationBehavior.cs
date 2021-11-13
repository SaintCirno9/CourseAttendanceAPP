using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.XForms.Border;
using Xamarin.Forms;

namespace AppShared.Behaviors
{
    public enum AnimationType
    {
        /// <summary>
        /// expand animation.
        /// </summary>
        Expand,

        /// <summary>
        /// shrink animation.
        /// </summary>
        Shrink,
    }

    public class SearchBarAnimationBehavior : Behavior<Button>
    {
        #region fileds

        /// <summary>
        /// Gets or sets the AnimationTypeProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty AnimationTypeProperty =
            BindableProperty.Create(nameof(AnimationType), typeof(AnimationType), typeof(SearchBarAnimationBehavior),
                AnimationType.Expand);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Animation type.
        /// </summary>
        public AnimationType AnimationType
        {
            get => (AnimationType) GetValue(AnimationTypeProperty);
            set => SetValue(AnimationTypeProperty, value);
        }

        /// <summary>
        /// Gets the Button.
        /// </summary>
        public Button Button { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when adding the Button to view.
        /// </summary>
        /// <param name="button">The button</param>
        protected override void OnAttachedTo(Button button)
        {
            if (button != null)
            {
                base.OnAttachedTo(button);
                Button = button;
                button.BindingContextChanged += OnBindingContextChanged;
                button.Clicked += Button_Clicked;
            }
        }

        /// <summary>
        /// Invoked when exit from the view.
        /// </summary>
        /// <param name="button">The button</param>
        protected override void OnDetachingFrom(Button button)
        {
            if (button != null)
            {
                base.OnDetachingFrom(button);
                button.BindingContextChanged -= OnBindingContextChanged;
                button.Clicked -= Button_Clicked;
                Button = null;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = Button.BindingContext;
        }

        /// <summary>
        /// Invoked when binding context is changed.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        /// <summary>
        /// Invoked when button is clicked.
        /// </summary>
        /// <param name="sender">The borderlessEntry</param>
        /// <param name="e">The Text Changed Event args</param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            switch (AnimationType)
            {
                case AnimationType.Expand:
                {
                    double opacity;
                    if (button.Parent is not StackLayout searchLayout)
                    {
                        return;
                    }


                    var searchLayoutChildren = searchLayout.Children;
                    foreach (var child in searchLayoutChildren)
                    {
                        if (child is Button)
                        {
                            child.IsVisible = false;
                        }
                    }

                    var titleLayout = searchLayoutChildren.First(view => view is StackLayout);
                    var searchEntryLayout =
                        searchLayoutChildren.First(view => view is StackLayout && view != titleLayout);

                    titleLayout.IsVisible = false;
                    searchEntryLayout.IsVisible = true;

                    var expandAnimation = new Animation(
                        property =>
                        {
                            searchEntryLayout.WidthRequest = property;
                            opacity = property / searchLayout.Width;
                            searchEntryLayout.Opacity = opacity;
                        },
                        0,
                        searchLayout.Width,
                        Easing.Linear);
                    expandAnimation.Commit(searchEntryLayout, "Expand", 16, 250, Easing.Linear,
                        (p, q) => SearchExpandAnimationCompleted(searchLayoutChildren));
                    break;
                }
                case AnimationType.Shrink:
                {
                    double opacity;
                    if (button.Parent is not StackLayout {Parent: StackLayout searchLayout} searchEntryLayout)
                    {
                        return;
                    }

                    var searchLayoutChildren = searchLayout.Children;

                    foreach (var child in searchLayoutChildren)
                    {
                        if (child is Button)
                        {
                            child.IsVisible = true;
                        }
                    }

                    // Animating Width of the search box, from full width to 0 before it removed from view.
                    var shrinkAnimation = new Animation(
                        property =>
                        {
                            searchEntryLayout.WidthRequest = property;
                            opacity = property / searchLayout.Width;
                            searchEntryLayout.Opacity = opacity;
                        },
                        searchEntryLayout.Width,
                        0,
                        Easing.Linear);
                    shrinkAnimation.Commit(searchEntryLayout, "Shrink", 16, 250, Easing.Linear,
                        (p, q) => SearchBoxAnimationCompleted(searchEntryLayout));

                    var children = searchEntryLayout.Children;
                    if ((children?[1] as SfBorder)?.Content is Entry searchEntry)
                    {
                        searchEntry.Text = string.Empty;
                    }

                    break;
                }
            }
        }

        private void SearchExpandAnimationCompleted(IList<View> children)
        {
            if (children.OfType<StackLayout>().ToList()[1]?.Children[1] is SfBorder searchEntryBorder)
            {
                var searchEntry = searchEntryBorder.Content as Entry;
                searchEntry?.Focus();
            }
        }

        private void SearchBoxAnimationCompleted(StackLayout searchEntryLayout)
        {
            var searchLayoutChildren = (searchEntryLayout.Parent as StackLayout)?.Children;
            searchEntryLayout.IsVisible = false;

            if (searchLayoutChildren is not null)
            {
                searchLayoutChildren.First(view => view is StackLayout).IsVisible = true;
            }
        }

        #endregion
    }
}