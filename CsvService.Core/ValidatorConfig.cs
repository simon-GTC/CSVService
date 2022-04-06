using System;

namespace CsvService.Core
{
    public class ValidatorConfig
    {
        /// <summary>
        /// Name of the validator, a combination of validator type and parameters
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string Name { get; set; }
        /// <summary>
        /// Flag for allowing the value to be empty
        /// </summary>
        public bool? AllowEmpty { get; set; }
        /// <summary>
        /// Name of validator
        /// IsIntegerValidator - value is an integer
        /// IsNumberValidator - value is a number
        /// IsDateValidator - value is a date
        /// NumberRangeValidator - value is within a specific range
        /// IsUpperCaseValidator - value is a string in upper case
        /// IsLowerCaseValidator - value is a string in lower case
        /// IsBooleanValidator - value is a boolean
        /// InListValidator - value is in a specific list
        /// RegexValidator - value match a specific pattern
        /// IsKeyValueListValidator - value is a list of key values
        /// </summary>
        public string Validator { get; set; }
        /// <summary>
        /// parameters to create the validator
        /// </summary
        public string[] Parameters { get; set; }
    }
}
