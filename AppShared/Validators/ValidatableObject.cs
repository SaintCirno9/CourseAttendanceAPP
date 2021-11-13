using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Internals;

namespace AppShared.Validators
{
    /// <summary>
    /// This class contains the method to validate the fields
    /// </summary>
    /// <typeparam name="T">Validatable object parameter</typeparam>
    [Preserve(AllMembers = true)]
    public class ValidatableObject<T> : IValidatable<T>
    {
        #region Fields

        /// <summary>
        /// Gets or Sets IsValid
        /// </summary>
        private bool isValid = true;

        /// <summary>
        /// Gets or Sets errors
        /// </summary>
        private List<string> errors = new List<string>();

        /// <summary>
        /// Gets or Sets CleanOnChange
        /// </summary>
        private bool cleanOnChange = true;

        /// <summary>
        /// Gets or Sets value
        /// </summary>
        private T value;

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region properties

        /// <summary>
        /// Gets or Sets the Validations
        /// </summary>
        public List<IValidationRule<T>> ValidationRules { get; } = new List<IValidationRule<T>>();

        /// <summary>
        /// Gets or Sets the Errors
        /// </summary>
        public List<string> Errors
        {
            get => errors;

            private set
            {
                errors = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is clean on change.
        /// </summary>
        public bool CleanOnChange
        {
            get => cleanOnChange;

            set
            {
                cleanOnChange = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value
        {
            get => value;
            set
            {
                this.value = value;

                if (CleanOnChange)
                {
                    IsValid = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is valid or not.
        /// </summary>
        public bool IsValid
        {
            get => isValid;

            set
            {
                isValid = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// this method for validate the email
        /// </summary>
        /// <returns>returns bool value</returns>
        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errorMessages = ValidationRules.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errorMessages.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }

        public override string ToString()
        {
            return $"{Value}";
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