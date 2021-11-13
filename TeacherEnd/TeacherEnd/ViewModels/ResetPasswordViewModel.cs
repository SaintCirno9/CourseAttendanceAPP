using AppShared.Validators;
using AppShared.Validators.Rules;
using TeacherEnd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeacherEnd.ViewModels
{
    /// <summary>
    /// ViewModel for reset password page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ResetPasswordViewModel : BaseViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordViewModel" /> class.
        /// </summary>
        public ResetPasswordViewModel()
        {
            InitializeProperties();
            AddValidationRules();
            SubmitCommand = new Command(SubmitClicked);
            SignUpCommand = new Command(SignUpClicked);
        }


        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the new password from user in the reset password page.
        /// </summary>
        public ValidatablePair<string> Password { get; set; }
        
        #region Methods

        /// <summary>
        /// Initialize whether fieldsvalue are true or false.
        /// </summary>
        /// <returns>true or false </returns>
        public bool AreFieldsValid()
        {
            bool isPassword = Password.Validate();
            return isPassword;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            Password = new ValidatablePair<string>();
        }

        /// <summary>
        /// Validation rule for password
        /// </summary>
        private void AddValidationRules()
        {
            Password.Item1.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "Password Required"});
            Password.Item2.ValidationRules.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = "Re-enter Password"});
        }

        /// <summary>
        /// Invoked when the Submit button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SubmitClicked(object obj)
        {
            if (AreFieldsValid())
            {
                // Do something
            }
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            // Do something
        }

        #endregion
    }
}