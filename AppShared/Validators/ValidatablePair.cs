using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Internals;

namespace AppShared.Validators
{
    /// <summary>
    /// This class contains the method to have validate the Pair fields
    /// </summary>
    /// <typeparam name="T">Validatable pair parameter</typeparam>  
    [Preserve(AllMembers = true)]
    public class ValidatablePair<T> : IValidatable<ValidatablePair<T>>
    {
        #region Fields

        /// <summary>
        /// Gets or Sets isValid
        /// </summary>
        private bool isValid = true;

        #endregion

        #region PropertyChanged

        /// <summary>
        /// The PropertyChanged event declared.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Property

        /// <summary>
        /// Gets or Sets the Validation
        /// </summary>
        public List<IValidationRule<ValidatablePair<T>>> ValidationRules { get; } = new List<IValidationRule<ValidatablePair<T>>>();

        /// <summary>
        /// Gets or sets a value indicating whether it is valid or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return isValid;
            }

            set
            {
                isValid = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or Sets Errors
        /// </summary>
        public List<string> Errors { get; private set; } = new List<string>();

        /// <summary>
        /// Gets or sets Item1.
        /// </summary>
        public ValidatableObject<T> Item1 { get; set; } = new ValidatableObject<T>();

        /// <summary>
        /// Gets or sets Item2.
        /// </summary>
        public ValidatableObject<T> Item2 { get; set; } = new ValidatableObject<T>();

        #endregion

        #region Methods

        /// <summary>
        /// Validate the Items
        /// </summary>
        /// <returns>returns bool value</returns>
        public bool Validate()
        {
            var item1IsValid = Item1.Validate();
            var item2IsValid = Item2.Validate();
            if (item1IsValid && item2IsValid)
            {
                Errors.Clear();
                IEnumerable<string> errors = ValidationRules.Where(v => !v.Check(this))
                    .Select(v => v.ValidationMessage);
                Errors = errors.ToList();
                Item2.Errors.Clear();
                Item2.Errors.AddRange(Errors);
                Item2.IsValid = !Errors.Any();
            }

            IsValid = !Item1.Errors.Any() && !Item2.Errors.Any();
            return IsValid;
        }

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">The PropertyName</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
