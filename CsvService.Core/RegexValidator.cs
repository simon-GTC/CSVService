using System;
using System.Text.RegularExpressions;

namespace CsvService.Core
{
    /// <summary>
    /// Extended goal: A cell follow the Regex specified sent by the user(ex: to be valid a cell must match ^[A-Z][a-z]+$)
    /// </summary>
    public class RegexValidator : CellValidatorBase
    {
        private string _pattern = null;

        public override string GetName() => $"Regex_{AllowEmpty}";

        public RegexValidator(string pattern)
        {
            _pattern = pattern;
        }

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            Regex regex = new Regex(_pattern);

            rv.IsValid = regex.IsMatch(value);

            if (rv.IsValid)
            {
                rv.ValidValue = value;
            }
            else
            {
                rv.Message = ValidationMessage.PATTERN_NOT_MATCH;
            }

            return rv;
        }
    }
}
