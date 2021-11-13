using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppShared.Behaviors
{
    /// <summary>
    /// This class extends the behavior of the Entry control to invoke a command when an event occurs.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class EntryLineValidationBehaviour : BehaviorBase<Entry>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the IsValidProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(EntryLineValidationBehaviour), true, BindingMode.TwoWay);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether it is valid or not.
        /// </summary>
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);

            set => SetValue(IsValidProperty, value);
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method for store the place holder color value
        /// </summary>
        /// <param name="bindable">Bindable object</param>
        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject.Focused += AssociatedObject_Focused;
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            AssociatedObject.Focused -= AssociatedObject_Focused;
        }

        private void AssociatedObject_Focused(object sender, FocusEventArgs e)
        {
            IsValid = true;
        }

        #endregion
    }
}
