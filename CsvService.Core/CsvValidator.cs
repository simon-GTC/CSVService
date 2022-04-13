using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace CsvService.Core
{
    /// <summary>
    /// Csv validator allowing custom config
    /// </summary>
    public class CsvValidator
    {
        private Dictionary<string, ICellValidator> _validators = new();
        private Config _config;
        private BlockingCollection<RowObject> _rows = new BlockingCollection<RowObject>();
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private const int TASK_COUNT = 4;

        public List<string> Errors = new List<string>();

        public CsvValidator(Config config)
        {
            _config = config;

            if (config.ValidateFrom.HasValue && config.ValidateFrom <= 0)
            {
                throw new Exception("invalid config");
            }
        }

        /// <summary>
        /// Populate the validators with config
        /// </summary>
        public CsvValidator Build()
        {
            Type type = typeof(ICellValidator);
            Dictionary<string, Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .ToDictionary(x => x.Name, x => x);

            try
            {
                for (int i = 0; i < _config.ColumnConfigs.Length; i++)
                {
                    for (int j = 0; j < _config.ColumnConfigs[i].ValidatorConfigs.Length; j++)
                    {
                        ValidatorConfig validatorConfig = _config.ColumnConfigs[i].ValidatorConfigs[j];
                        string validatorName = validatorConfig.Validator;
                        string[] parameters = validatorConfig.Parameters;

                        if (types.ContainsKey(validatorName))
                        {
                            ICellValidator validator = (ICellValidator)Activator.CreateInstance(types[validatorName], parameters);

                            validator.AllowEmpty = validatorConfig.AllowEmpty.GetValueOrDefault(false);

                            validatorConfig.Name = validator.GetName();

                            Register(validator);
                        }
                    }
                }

                return this;
            }
            catch
            {
                throw new Exception("invalid config");
            }
        }
        public void Register(ICellValidator validator)
        {
            _validators[validator.GetName()] = validator;
        }

        /// <summary>
        /// Validate a csv file
        /// </summary>
        /// <returns>
        /// If the csv is valid, return empty list
        /// If the csv is invalid, return a list of error message
        /// </returns>
        public List<string> Validate(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            Errors.Clear();

            int currentRow = 0;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = _config.Delimiter,
                HasHeaderRecord = false
            };

            Task[] tasks = new Task[TASK_COUNT];
            for (int i = 0; i < TASK_COUNT; i++)
            {
                tasks[i] = new Task(ProcessQueue);
                tasks[i].Start();
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                while (csv.Read())
                {
                    // Extended goal: Start validation from n line
                    if (_config.ValidateFrom.HasValue
                            && currentRow < _config.ValidateFrom.GetValueOrDefault(1) - 1)
                    {
                        currentRow++;
                        continue;
                    }

                    if (_cts.IsCancellationRequested)
                    {
                        break;
                    }

                    var row = csv.GetRecord<dynamic>();

                    // _rows may mark completed before adding
                    try
                    {
                        if (!_rows.IsAddingCompleted)
                        {
                            _rows.TryAdd(new RowObject { RowNumber = currentRow, Row = row });
                        }
                    }
                    catch { }

                    currentRow++;
                }

                _rows.CompleteAdding();
            }

            Task.WaitAll(tasks);

            return Errors;
        }

        /// <summary>
        /// Validate a single row in the csv
        /// </summary>
        /// <returns>
        /// Is the row valid
        /// </returns>
        public List<string> ValidateRow(int rowNumber, dynamic row)
        {
            List<string> errors = new();

            int currentColumn = 0;

            foreach (var column in row)
            {
                ValidatorConfig[] validatorConfigs = _config.ColumnConfigs[currentColumn].ValidatorConfigs;

                if (validatorConfigs != null)
                {
                    string[] cellValues = column.Value.ToString().Split(",");

                    for (int i = 0; i < cellValues.Length; i++)
                    {
                        if (validatorConfigs.Length > i)
                        {
                            // Extended goal: A cell can be a list of values, each would need to be validated
                            ValidationResult rs = _validators[validatorConfigs[i].Name].Validate(cellValues[i]);

                            if (!rs.IsValid)
                            {
                                if (cellValues.Length > 1)
                                {
                                    errors.Add(GetErrorMessage(rowNumber, currentColumn + 1, $"Value {i + 1} - {rs.Message}"));
                                }
                                else
                                {
                                    errors.Add(GetErrorMessage(rowNumber, currentColumn + 1, rs.Message));
                                }
                            }
                        }
                    }
                }

                currentColumn++;
            }

            return errors;
        }

        public void ProcessQueue()
        {
            while (!_cts.IsCancellationRequested && !_rows.IsCompleted)
            {
                if (_rows.TryTake(out RowObject row, 10))
                {
                    foreach (string error in ValidateRow(row.RowNumber, row.Row))
                    {
                        if (Errors.Count >= 50)
                        {
                            _rows.CompleteAdding();
                            _cts.Cancel();
                        }
                        else
                        {
                            Errors.Add(error);
                        }
                    }
                }
            }
        }

        private string GetErrorMessage(int row, int column, string message) => $"row {row}, column {column}: {message}";
    }

    public class RowObject
    {
        public int RowNumber { get; set; }
        public dynamic Row { get; set; }
    }
}
