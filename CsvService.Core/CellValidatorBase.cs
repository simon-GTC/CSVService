using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace CsvService.Core
{
    public abstract class CellValidatorBase : ICellValidator
    {
        protected ConcurrentDictionary<string, ValidationResult> _cache = new();
        /// <summary>
        /// Enable caching by default to have best performance
        /// POC: if there are lots of repeated value, enabling it should improve the performance
        /// </summary>
        public bool EnableCaching { get; set; } = true;
        /// <summary>
        /// Is the value allow empty
        /// </summary>
        public bool AllowEmpty { get; set; }
        /// <summary>
        /// Name of the validator, a combination of validator type and parameters
        /// Shoule be unique for different validator type and parameters
        /// </summary>
        public abstract string GetName();
        /// <summary>
        /// Validate the value
        /// </summary>

        protected abstract ValidationResult InternalValidate(string value);
        public virtual ValidationResult Validate(string value)
        {
            if (EnableCaching && _cache.ContainsKey(value))
            {
                return _cache[value];
            }

            if (CheckEmpty(value, out ValidationResult rv))
            {
                return rv;
            }

            rv = InternalValidate(value);

            if (EnableCaching)
            {
                _cache[value] = rv;
            }

            return rv;            
        }

        /// <summary>
        /// Check if the value allow empty and if the value is empty
        /// </summary>
        protected bool CheckEmpty(string value, out ValidationResult validationResult)
        {
            validationResult = new();

            if (string.IsNullOrEmpty(value))
            {
                if (AllowEmpty)
                {
                    validationResult.IsValid = true;
                }
                else
                {
                    validationResult.IsValid = false;
                    validationResult.Message = ValidationMessage.CANNOT_BE_EMPTY;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
