using System;

namespace CsvService.Core
{
    /// <summary>
    /// Configuration of the csv validation service
    /// </summary>
    public struct Config
    {
        /// <summary>
        /// Csv field delimiter
        /// </summary>
        /// <example>,</example>
        public string Delimiter { get; set; }
        /// <summary>        
        /// Extended goal: Start validation from n line
        /// Validate the csv from specific line number
        /// </summary>
        /// <example>10</example>
        public int? ValidateFrom { get; set; }
        /// <summary>
        /// Array of column configs
        /// Each column has its own config
        /// If the are 10 columns in the csv, there should be 10 column config
        /// </summary>
        public ColumnConfig[] ColumnConfigs { get; set; }
    }
}
