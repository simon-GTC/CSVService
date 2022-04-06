using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 7.Cell value is a string in upper case.
    /// </summary>
    public class IsUpperCaseValidator : CellValidatorBase
    {
        public override string GetName() => $"IsUpperCase_{AllowEmpty}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = IsAllUpperCase(value);

            if (rv.IsValid)
            {
                rv.ValidValue = value;
            }
            else
            {
                rv.Message = ValidationMessage.NOT_UPPER_CASE;
            }

            return rv;
        }
        
        private bool IsAllUpperCase(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsUpper(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
