using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 9.Cell value is a Boolean.
    /// </summary>
    public class IsBooleanValidator : CellValidatorBase
    {
        public override string GetName() => $"IsBoolean_{AllowEmpty}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = bool.TryParse(value, out bool parsed);
            if (rv.IsValid)
            {
                rv.ValidValue = parsed;
                rv.IsValid = true;
            }
            else
            {
                rv.Message = ValidationMessage.NOT_A_BOOLEAN;
            }

            return rv;
        }
    }
}
