using System;
using System.Collections.Generic;

namespace CsvService.Core
{
    /// <summary>
    /// Validation 10.Cell value is contained in a specified list (ex: valid values are “JPY, USD, EUR”. If value is CAD or Jpy or UsD, then cell is invalid).
    /// </summary>
    public class InListValidator : CellValidatorBase
    {
        private IList<string> _list;

        public InListValidator(IList<string> list)
        {
            _list = list;
        }

        public InListValidator(string listString)
        {
            _list = listString.Split(',');
        }

        public override string GetName() => $"InList_{AllowEmpty}_{string.Join(',', _list)}";

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            rv.IsValid = _list.Contains(value);

            if (rv.IsValid)
            {
                rv.ValidValue = value;
            }
            else
            {
                rv.Message = ValidationMessage.NOT_IN_LIST;
            }

            return rv;
        }
    }
}
