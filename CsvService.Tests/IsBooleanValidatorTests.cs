using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class IsBooleanValidatorTests
    {
        [Fact]
        public void IsBooleanValidator_GetName_Should_Valid()
        {
            IsBooleanValidator validator = CreateDefaultValidator();

            Assert.Equal("IsBoolean_False", validator.GetName());
        }

        [Fact]
        public void IsBooleanValidator_Allow_Empty_Should_Valid()
        {
            IsBooleanValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsBooleanValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsBooleanValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }
        
        [Fact]
        public void IsBooleanValidator_Should_Valid()
        {
            IsBooleanValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("true");

            Assert.True((bool)result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsBooleanValidator_Should_Invalid()
        {
            IsBooleanValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("some string");

            Assert.Equal(ValidationMessage.NOT_A_BOOLEAN, result.Message);
            Assert.False(result.IsValid);     
        }

        private IsBooleanValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new() { AllowEmpty = allowEmpty };
        }
    }
}
