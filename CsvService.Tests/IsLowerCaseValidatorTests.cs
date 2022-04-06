using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class IsLowerCaseValidatorTests
    {
        [Fact]
        public void IsLowerCaseValidator_GetName_Should_Valid()
        {
            IsLowerCaseValidator validator = CreateDefaultValidator();

            Assert.Equal("IsLowerCase_False", validator.GetName());
        }

        [Fact]
        public void IsLowerCaseValidator_Allow_Empty_Should_Valid()
        {
            IsLowerCaseValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsLowerCaseValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsLowerCaseValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsLowerCaseValidator_Should_Valid()
        {
            IsLowerCaseValidator validator = CreateDefaultValidator();

            const string ALL_LOWERCASE = "lower";

            ValidationResult result = validator.Validate(ALL_LOWERCASE);

            Assert.Equal(ALL_LOWERCASE, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsUpperCaseValidator_Should_Invalid()
        {
            IsLowerCaseValidator validator = CreateDefaultValidator();

            const string ALL_UPPERCASE = "UPPER";

            ValidationResult result = validator.Validate(ALL_UPPERCASE);

            Assert.Equal(ValidationMessage.NOT_LOWER_CASE, result.Message);
            Assert.False(result.IsValid);
        }

        private IsLowerCaseValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new() { AllowEmpty = allowEmpty };
        }
    }
}
