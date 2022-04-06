using System;

namespace CsvService.Core
{
    public interface ICellValidator
    {
        bool AllowEmpty { get; set; }
        string GetName();
        ValidationResult Validate(string value);
    }
}
