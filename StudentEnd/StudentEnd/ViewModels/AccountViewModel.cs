using AppShared.Validators;
using AppShared.Validators.Rules;
using StudentEnd.ViewModels;
using Xamarin.Forms.Internals;

namespace StudentEnd.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AccountViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance for the <see cref="AccountViewModel" /> class.
        /// </summary>
        public AccountViewModel()
        {
            InitializeProperties();
            AddValidationRules();
        }


        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the email ID from user in the login page.
        /// </summary>

        public ValidatableObject<string> UserName { get; set; }

        public ValidatableObject<string> Name { get; set; }

        public ValidatableObject<string> Password { get; set; }

        public ValidatableObject<string> Phone { get; set; }
        public ValidatableObject<string> Email { get; set; }


        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            UserName = new ValidatableObject<string>();
            Name = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            Phone = new ValidatableObject<string>();
            Email = new ValidatableObject<string>();
        }

        /// <summary>
        /// This method contains the validation rules
        /// </summary>
        private void AddValidationRules()
        {
            UserName.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "用户名不能为空"});
            Name.ValidationRules.Add(new IsNotNullOrEmptyRule<string>() {ValidationMessage = "姓名不能为空"});
            Password.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "密码不能为空"});
            Email.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "邮箱不能为空"});
            // Email.Validations.Add(new IsValidEmailRule<string> {ValidationMessage = "Invalid Email"});
            Phone.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "电话号码不能为空"});
        }
    }
}