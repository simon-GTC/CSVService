using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 3.Cell value is an integer (a number without a decimal value).
    /// </summary>
    public class IsIntegerValidator : CellValidatorBase
    {
        public override string GetName() => $"IsInteger_{AllowEmpty}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = int.TryParse(value, out int parsed);
            if (rv.IsValid)
            {
                rv.ValidValue = parsed;
                rv.IsValid = true;
            }
            else
            {
                rv.Message = ValidationMessage.NOT_AN_INTEGER;
            }

            return rv;
        }
    }
}
