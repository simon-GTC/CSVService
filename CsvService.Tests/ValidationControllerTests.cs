using System;
using Xunit;
using CsvService.Core;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using CsvService.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CsvService.Tests
{
    public class ValidatorControllerTests
    {
        const string TEST_DATA_PATH = @"..\..\..\TestData\";

        [Fact]
        public void ValidationController_Validate_Valid_CSV_Should_Valid()
        {
            ValidationController controller = new();

            ActionResult<List<string>> results = controller.Validate(Path.Combine(TEST_DATA_PATH, "Valid_CSV.csv"), CreateDefaultConfig());

            Assert.IsType<OkResult>(results.Result);
        }

        [Fact]
        public void ValidationController_Validate_Should_Throw_File_Not_Found()
        {
            ValidationController controller = new();
            ActionResult<List<string>> results = controller.Validate(@"", CreateDefaultConfig());

            Assert.IsType<BadRequestObjectResult>(results.Result);
            Assert.Equal(400, ((BadRequestObjectResult)results.Result).StatusCode);
            Assert.Equal("file cannot be found", ((BadRequestObjectResult)results.Result).Value);
        }

        [Fact]
        public void ValidationController_Validate_Should_Throw_Invalid_Config()
        {
            ValidationController controller = new();
            ActionResult<List<string>> results = controller.Validate(@"", new Config());

            Assert.IsType<BadRequestObjectResult>(results.Result);
            Assert.Equal(400, ((BadRequestObjectResult)results.Result).StatusCode);
            Assert.Equal("invalid config", ((BadRequestObjectResult)results.Result).Value);
        }

        [Fact]
        public void ValidationController_Validate_Should_Throw_Invalid_Csv_File()
        {
            ValidationController controller = new();
            ActionResult<List<string>> results = controller.Validate(Path.Combine(TEST_DATA_PATH, "word.docx"), CreateDefaultConfig());

            Assert.IsType<BadRequestObjectResult>(results.Result);
            Assert.Equal(400, ((BadRequestObjectResult)results.Result).StatusCode);
            Assert.Equal("file is not a CSV", ((BadRequestObjectResult)results.Result).Value);
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
    }
}
