using System;
using Xunit;
using CsvService.Core;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

namespace CsvService.Tests
{
    public class CsvValidatorTests
    {
        const string TEST_DATA_PATH = @"..\..\..\TestData\";

        [Fact]
        public void CsvValidator_Validate_Valid_CSV_Should_Valid()
        {
            var validator = new CsvValidator(CreateDefaultConfig()).Build();
            
            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Valid_CSV.csv"));

            Assert.Empty(errors);
        }

        [Fact]
        public void CsvValidator_Validate_Big_Valid_CSV_Should_Valid()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var validator = new CsvValidator(CreateDefaultConfig()).Build();
            
            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Big_Valid_CSV.csv"));

            Assert.Empty(errors);
            sw.Stop();
            // 5123
            // 2 no 4128
            var total = sw.Elapsed.TotalMilliseconds;
        }

        [Fact]
        public void CsvValidator_Validate_Big_Error_CSV_Should_Invalid()
        {
            var validator = new CsvValidator(CreateDefaultConfig()).Build();
            
            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Big_Error_CSV.csv"));

            Assert.NotEmpty(errors);
            Assert.Equal(50, errors.Count);
        }

        [Fact]
        public void CsvValidator_Validate_Stroke_Delimiter_Valid_CSV_Should_Valid()
        {
            var validator = new CsvValidator(CreateDefaultConfig(delimiter: "|")).Build();

            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Stroke_Delimiter_Valid_CSV.csv"));

            Assert.Empty(errors);
        }

        [Fact]
        public void CsvValidator_Validate_Error_CSV_Should_Have_Errors()
        {
            var validator = new CsvValidator(CreateDefaultConfig()).Build();

            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Error_CSV.csv"));

            Assert.Equal(13, errors.Count);
            Assert.Equal("row 1, column 1: the cell should not be empty", errors[0]);
            Assert.Equal("row 1, column 2: the cell is not an integer", errors[1]);
            Assert.Equal("row 1, column 3: the cell value has too many decimal places", errors[2]);
            Assert.Equal("row 1, column 4: the cell is not in correct date format", errors[3]);
            Assert.Equal("row 1, column 5: the cell value is out of range", errors[4]);
            Assert.Equal("row 1, column 6: the cell value is not positive", errors[5]);
            Assert.Equal("row 1, column 7: the cell value is not negative", errors[6]);
            Assert.Equal("row 1, column 8: the cell is not in upper case", errors[7]);
            Assert.Equal("row 1, column 9: the cell is not in lower case", errors[8]);
            Assert.Equal("row 1, column 10: the cell is not a boolean", errors[9]);
            Assert.Equal("row 1, column 11: the cell is not in provided list", errors[10]);
            Assert.Equal("row 1, column 12: the cell does not match the pattern", errors[11]);
            Assert.Equal("row 1, column 13: the cell is not a valid list of key values", errors[12]);
        }

        [Fact]
        public void CsvValidator_Validate_Multi_Rows_Error_CSV_Should_Have_50_Errors()
        {
            var validator = new CsvValidator(CreateDefaultConfig()).Build();

            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Multi_Rows_Error_CSV.csv"));

            Assert.Equal(50, errors.Count);
        }

        [Fact]
        public void CsvValidator_Validate_Multi_Rows_Error_CSV_Should_Have_From_10th_Row_Should_Have_22_Errors()
        {
            var validator = new CsvValidator(CreateDefaultConfig(validateFrom: 10)).Build();

            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Multi_Rows_Error_CSV.csv"));

            Assert.Equal(26, errors.Count);
        }

        [Fact]
        public void CsvValidator_Validate_Cell_With_List_Of_Value_Should_Valid()
        {
            var validator = new CsvValidator(CreateCellWithListOfValueConfig()).Build();

            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Cell_with_List_of_Value_Valid_CSV.csv"));

            Assert.Empty(errors);
        }

        [Fact]
        public void CsvValidator_Validate_Cell_With_List_Of_Value_Should_Have_Errors()
        {
            var validator = new CsvValidator(CreateCellWithListOfValueConfig()).Build();

            List<string> errors = validator.Validate(Path.Combine(TEST_DATA_PATH, "Cell_with_List_of_Value_Error_CSV.csv"));

            Assert.NotEmpty(errors);
        }

        [Fact]
        public void CsvValidator_ValidateRow_Should_Have_Errors()
        {
            string csvData = "1,2,b,a";
            dynamic record = GetCsvRecord(csvData);

            Config config = new Config() { Delimiter = ",", ColumnConfigs = new ColumnConfig[4] };
            for (int i = 0; i < 4; i++)
            {
                config.ColumnConfigs[i].ValidatorConfigs = new ValidatorConfig[1]
                {
                    new ValidatorConfig
                    {
                        Validator = "IsIntegerValidator"
                    }
                };
            }

            CsvValidator validator = new CsvValidator(config).Build();
            List<string> errors = validator.ValidateRow(1, record);

            Assert.Equal(2, errors.Count);
            Assert.Equal("row 1, column 3: the cell is not an integer", errors[0]);
            Assert.Equal("row 1, column 4: the cell is not an integer", errors[1]);
        }

        [Fact]
        public void CsvValidator_ValidateRow_Should_Valid()
        {
            string csvData = "1,2,3,4";
            dynamic record = GetCsvRecord(csvData);

            Config config = new Config() { Delimiter = ",", ColumnConfigs = new ColumnConfig[4] };
            for (int i = 0; i < 4; i++)
            {
                config.ColumnConfigs[i].ValidatorConfigs = new ValidatorConfig[1]
                {
                    new ValidatorConfig
                    {
                        Validator = "IsIntegerValidator"
                    }
                };
            }

            CsvValidator validator = new CsvValidator(config).Build();
            List<string> errors = validator.ValidateRow(1, record);

            Assert.Empty(errors);
        }

        private dynamic GetCsvRecord(string csvData)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = false
            };

            TextReader reader = new System.IO.StringReader(csvData);
            var csvReader = new CsvReader(reader, config);
            {
                csvReader.Read();
                return csvReader.GetRecord<dynamic>();
            }
        }

        private Config CreateDefaultConfig(string delimiter = ",", int validateFrom = 1)
        {
            Config config = new Config() { Delimiter = delimiter, ValidateFrom = validateFrom, ColumnConfigs = new ColumnConfig[13] };

            config.ColumnConfigs[0].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsIntegerValidator" } };

            config.ColumnConfigs[1].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsIntegerValidator", AllowEmpty = true } };

            config.ColumnConfigs[2].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsNumberValidator", Parameters = new string[1] { "3" } } };

            config.ColumnConfigs[3].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsDateValidator", Parameters = new string[1] { "yyyy-MM-dd" } } };

            config.ColumnConfigs[4].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "NumberRangeValidator", Parameters = new string[3] { "SpecificRange", "0", "100" } } };

            config.ColumnConfigs[5].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "NumberRangeValidator", Parameters = new string[1] { "Positive" } } };

            config.ColumnConfigs[6].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "NumberRangeValidator", Parameters = new string[1] { "Negative" } } };

            config.ColumnConfigs[7].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsUpperCaseValidator" } };

            config.ColumnConfigs[8].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsLowerCaseValidator" } };

            config.ColumnConfigs[9].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsBooleanValidator" } };

            config.ColumnConfigs[10].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "InListValidator", Parameters = new string[1] { "JPY,USD,EUR"} } };

            config.ColumnConfigs[11].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "RegexValidator", Parameters = new string[1] { "^[A-Z][a-z]+$"} } };

            config.ColumnConfigs[12].ValidatorConfigs = new ValidatorConfig[1]
            { new ValidatorConfig { Validator = "IsKeyValueListValidator", Parameters = new string[1] { "|"} } };

            return config;
        }

        private Config CreateCellWithListOfValueConfig(string delimiter = ",", int validateFrom = 1)
        {
            Config config = new Config() { Delimiter = delimiter, ValidateFrom = validateFrom, ColumnConfigs = new ColumnConfig[2] };

            config.ColumnConfigs[0].ValidatorConfigs = new ValidatorConfig[3]
            {
                new ValidatorConfig { Validator = "IsIntegerValidator" },
                new ValidatorConfig { Validator = "NumberRangeValidator", Parameters = new string[3] { "SpecificRange", "0", "100" } },
                new ValidatorConfig { Validator = "IsNumberValidator", Parameters = new string[1] {"3"} }
            };

            config.ColumnConfigs[1].ValidatorConfigs = new ValidatorConfig[2]
            {
                new ValidatorConfig { Validator = "InListValidator", Parameters = new string[1] { "JPY,USD,EUR"} },
                new ValidatorConfig { Validator = "InListValidator", Parameters = new string[1] { "JPY,USD,EUR"} }
            };

            return config;
        }
    }
}
