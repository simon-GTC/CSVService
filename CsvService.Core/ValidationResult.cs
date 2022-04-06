using System;

namespace CsvService.Core
{
    public class ValidationResult
    {
        /// <summary>
        /// Parsed value if it is valid
        /// </summary>
        public object ValidValue { get; set; }
        /// <summary>
        /// The value is valid or not
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        public override string ToString() => $"Value: {ValidValue}, IsValid: {IsValid}, Message: {Message}";
    }
}
