namespace AppShared.Validators.Rules
{
    public class IsNotZeroRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return value.ToString() is not "0";
        }
    }
}