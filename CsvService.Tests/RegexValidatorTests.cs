using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class RegexValidatorTests
    {
        [Fact]
        public void RegexValidator_GetName_Should_Valid()
        {
            RegexValidator validator = CreateDefaultValidator();

            Assert.Equal("Regex_False", validator.GetName());
        }

        [Fact]
        public void RegexValidator_Allow_Empty_Should_Valid()
        {
            RegexValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void RegexValidator_Not_Allow_Empty_Should_Invalid()
        {
            RegexValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("Aabcz")]
        [InlineData("Aa")]
        [InlineData("Aabc")]
        public void RegexValidator_Should_Valid(string value)
        {
            RegexValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate(value);

            Assert.Equal(value, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("A1234abc")]
        [InlineData("aA")]
        [InlineData("AaA")]
        public void RegexValidator_Should_Invalid(string value)
        {
            RegexValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate(value);

            Assert.Equal(ValidationMessage.PATTERN_NOT_MATCH, result.Message);
            Assert.False(result.IsValid);
        }

        private RegexValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new(@"^[A-Z][a-z]+$") { AllowEmpty = allowEmpty };
        }
    }
}
