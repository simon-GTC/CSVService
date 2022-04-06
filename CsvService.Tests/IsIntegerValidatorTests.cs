using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class IsIntegerValidatorTests
    {
        [Fact]
        public void IsIntegerValidator_GetName_Should_Valid()
        {
            IsIntegerValidator validator = CreateDefaultValidator();

            Assert.Equal("IsInteger_False", validator.GetName());
        }

        [Fact]
        public void IsIntegerValidator_Allow_Empty_Should_Valid()
        {
            IsIntegerValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsIntegerValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsIntegerValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsIntegerValidator_Should_Valid()
        {
            IsIntegerValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("100");

            Assert.Equal(100, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsEmptyValidator_With_Non_Number_Should_Invalid()
        {
            IsIntegerValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("x");

            Assert.Equal(ValidationMessage.NOT_AN_INTEGER, result.Message);
            Assert.False(result.IsValid);
     
        }

        [Fact]
        public void IsEmptyValidator_With_Non_Decimal_Should_Invalid()
        {
            IsIntegerValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("100.123");

            Assert.Equal(ValidationMessage.NOT_AN_INTEGER, result.Message);
            Assert.False(result.IsValid);
        }

        private IsIntegerValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new() { AllowEmpty = allowEmpty };
        }
    }
}
