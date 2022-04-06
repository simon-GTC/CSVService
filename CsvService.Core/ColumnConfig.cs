using System;

namespace CsvService.Core
{
    /// <summary>
    /// Column validation config
    /// </summary>
    public struct ColumnConfig
    {
        /// <summary>
        /// Array of validator config
        /// A column may contain multiple values separated by , (for example: value1,value2,value3)
        /// Each value has its own validator
        /// If there are 3 values in a column, there should be 3 validators
        /// </summary>
        public ValidatorConfig[] ValidatorConfigs { get; set; }
    }
}
