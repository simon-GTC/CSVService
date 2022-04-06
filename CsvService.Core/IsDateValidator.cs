using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 5.Cell value is a date that follows the specified format given by the user.
    /// </summary>
    public class IsDateValidator : CellValidatorBase
    {
        private string _format = null;

        public IsDateValidator(string format)
        {
            _format = format;
        }

        public override string GetName() => $"IsDate_{AllowEmpty}_{_format}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = DateTime.TryParseExact(value, _format, null,
                            System.Globalization.DateTimeStyles.None, out DateTime parsed);

            if (rv.IsValid)
            {
                rv.ValidValue = parsed;
                rv.IsValid = true;
            }
            else
            {
                rv.Message = ValidationMessage.NOT_CORRECT_DATE_FORMAT;
            }

            return rv;
        }
    }
}
