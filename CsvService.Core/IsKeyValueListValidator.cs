using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvService.Core
{
    /// <summary>
    /// Extended goal: Validate that the cell is a list of key values with list separator and key/value separator specified by the user.
    /// </summary>
    public class IsKeyValueListValidator : CellValidatorBase
    {
        private string _separator;

        public override string GetName() => $"IsKeyValueList_{AllowEmpty}";

        public IsKeyValueListValidator(string separator)
        {
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentException("seperator cannot be empty");
            }

            _separator = separator;
        }

        protected override ValidationResult InternalValidate(string value)
        {
            ValidationResult rv = new();

            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            try
            {
                string[] list = value.Split(_separator);

                rv.IsValid = true;

                foreach (string pair in list)
                {
                    string[] keyValue = pair.Split('=');

                    if (keyValue.Length != 2)
                    {
                        rv.IsValid = false;
                        rv.Message = ValidationMessage.NOT_VALID_LIST_OF_KEY_VALUES;
                        break;
                    }

                    if (keyValues.ContainsKey(keyValue[0]))
                    {
                        rv.IsValid = false;
                        rv.Message = ValidationMessage.DUPLICATED_KEY;
                        break;
                    }
                    else
                    {
                        keyValues.Add(keyValue[0], keyValue[1]);
                    }
                }
            }
            catch 
            { 
                rv.IsValid = false;
                rv.Message = ValidationMessage.NOT_VALID_LIST_OF_KEY_VALUES;
            }

            if (rv.IsValid)
            {
                rv.ValidValue = keyValues;
            }

            return rv;
        }
    }
}
