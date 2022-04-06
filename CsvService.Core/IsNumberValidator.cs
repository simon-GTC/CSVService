using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 4.Cell value is a number. Can specify the maximum decimal place value (ex: if 3 is the max, then 2.1234 is invalid and 5.236 is valid).
    /// </summary>
    public class IsNumberValidator : CellValidatorBase
    {
        private int? _maxDecimalPlace = null;

        public IsNumberValidator()
        {

        }

        public IsNumberValidator(int maxDecimalPlace)
        {
            _maxDecimalPlace = maxDecimalPlace;
        }

        public IsNumberValidator(string maxDecimalPlace)
        {
            _maxDecimalPlace = Convert.ToInt32(maxDecimalPlace);
        }
        public override string GetName() => $"IsNumber_{AllowEmpty}_{_maxDecimalPlace}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = decimal.TryParse(value, out decimal parsed);

            if (rv.IsValid)
            {
                if (_maxDecimalPlace is not null)
                {
                    int dotIndex = value.IndexOf('.');

                    if (dotIndex > 0)
                    {
                        int decimalPlaceLength = value.Length - dotIndex - 1;
                        
                        if (decimalPlaceLength > _maxDecimalPlace.Value)
                        {
                            rv.IsValid = false;
                            rv.Message = ValidationMessage.TOO_MANY_DECIMAL_PLACES;
                        }
                    }
                }
            }
            else
            {
                rv.Message = ValidationMessage.NOT_A_NUMBER;
            }

            if (rv.IsValid)
            {
                rv.ValidValue = parsed;
            }

            return rv;
        }
    }
}
