using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class IsDateValidatorTests
    {
        [Fact]
        public void IsDateValidator_GetName_Should_Valid()
        {
            IsDateValidator validator = CreateDefaultValidator();

            Assert.Equal("IsDate_False_yyyy-MM-dd", validator.GetName());
        }

        [Fact]
        public void IsDateValidator_Allow_Empty_Should_Valid()
        {
            IsDateValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsDateValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsDateValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsDateValidator_Should_Valid()
        {
            IsDateValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("2022-03-30");

            Assert.Equal(new DateTime(2022, 3, 30), result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsDateValidator_With_Incorrect_Format_Should_Invalid()
        {
            IsDateValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("30-03-2022");

            Assert.Equal(ValidationMessage.NOT_CORRECT_DATE_FORMAT, result.Message);
            Assert.False(result.IsValid);     
        }

        private IsDateValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new("yyyy-MM-dd") { AllowEmpty = allowEmpty };
        }
    }
}
