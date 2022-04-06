using System;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 6.Cell value is a number/integer in the specified range of value (ex: between 0 and 100, or positive, or strictly negative,…).
    /// </summary>
    public class NumberRangeValidator : CellValidatorBase
    {
        public enum RangeType
        {
            SpecificRange,
            Positive,
            Negative
        }
        private RangeType _rangeType;
        private decimal _minValue;
        private decimal _maxValue;

        public NumberRangeValidator(string rangeType)
        {
            _rangeType = (RangeType)Enum.Parse(typeof(RangeType), rangeType);

            if (_rangeType == RangeType.SpecificRange)
            {
                throw new ArgumentException("SpecificRange should provide minValue and maxValue");
            }
        }

        public NumberRangeValidator(RangeType rangeType)
        {
            if (rangeType == RangeType.SpecificRange)
            {
                throw new ArgumentException("SpecificRange should provide minValue and maxValue");
            }
            _rangeType = rangeType;
        }

        public NumberRangeValidator(decimal minValue, decimal maxValue)
        {
            _rangeType = RangeType.SpecificRange;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public NumberRangeValidator(string rangeType, string minValue, string maxValue)
        {
            _rangeType = (RangeType)Enum.Parse(typeof(RangeType), rangeType);

            if (_rangeType == RangeType.SpecificRange)
            {
                if (!decimal.TryParse(minValue, out _minValue))
                {
                    throw new ArgumentException("invalid minValue");
                }

                if (!decimal.TryParse(maxValue, out _maxValue))
                {
                    throw new ArgumentException("invalid minValue");
                }
            }
        }

        public override string GetName() => $"NumberRange_{AllowEmpty}_{_rangeType}_{_minValue}_{_maxValue}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = decimal.TryParse(value, out decimal parsed);
            if (rv.IsValid)
            {
                if (_rangeType == RangeType.SpecificRange)
                {
                    if (parsed < _minValue || parsed > _maxValue)
                    {
                        rv.IsValid = false;
                        rv.Message = ValidationMessage.NUMBER_OUT_OF_RANGE;
                    }
                }
                else if (_rangeType == RangeType.Positive)
                {
                    if (parsed <= 0)
                    {
                        rv.IsValid = false;
                        rv.Message = ValidationMessage.NUMBER_NOT_POSITIVE;
                    }
                }
                else if (_rangeType == RangeType.Negative)
                {
                    if (parsed >= 0)
                    {
                        rv.IsValid = false;
                        rv.Message = ValidationMessage.NUMBER_NOT_NEGATIVE;
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
