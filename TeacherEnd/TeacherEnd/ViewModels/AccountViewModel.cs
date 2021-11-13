using AppShared.Validators;
using AppShared.Validators.Rules;
using TeacherEnd.ViewModels;
using Xamarin.Forms.Internals;

namespace TeacherEnd.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AccountViewModel : BaseViewModel
    {
        public ValidatableObject<string> UserName { get; set; } = new();

        public ValidatableObject<string> Name { get; set; } = new();

        public ValidatableObject<string> Password { get; set; } = new();

        public ValidatableObject<string> Phone { get; set; } = new();
        public ValidatableObject<string> Email { get; set; } = new();
        
        public AccountViewModel()
        {
            UserName.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "UserName Required"});
            Name.ValidationRules.Add(new IsNotNullOrEmptyRule<string>() {ValidationMessage = "Name Required"});
            Password.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "Password Required"});
            Email.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "Email Required"});
            // Email.Validations.Add(new IsValidEmailRule<string> {ValidationMessage = "Invalid Email"});
            Phone.ValidationRules.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "Phone Required"});
        }
    }
}