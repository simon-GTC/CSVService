using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 8.Cell value is a string in lower case.
    /// </summary>
    public class IsLowerCaseValidator : CellValidatorBase
    {
        public override string GetName() => $"IsLowerCase_{AllowEmpty}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = IsAllLowerCase(value);

            if (rv.IsValid)
            {
                rv.ValidValue = value;
            }
            else
            {
                rv.Message = ValidationMessage.NOT_LOWER_CASE;
            }

            return rv;
        }
        
        private bool IsAllLowerCase(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsLower(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
