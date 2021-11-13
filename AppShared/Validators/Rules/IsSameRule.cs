namespace AppShared.Validators.Rules
{
    public class IsSameRule<T>: IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value is ValidatablePair<string> pair)
            {
                return pair.Item1.Value == pair.Item2.Value;
            }

            return false;
        }
    }
}