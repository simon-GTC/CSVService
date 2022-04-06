using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class IsUpperCaseValidatorTests
    {
        [Fact]
        public void IsUpperCaseValidator_GetName_Should_Valid()
        {
            IsUpperCaseValidator validator = CreateDefaultValidator();

            Assert.Equal("IsUpperCase_False", validator.GetName());
        }

        [Fact]
        public void IsUpperCaseValidator_Allow_Empty_Should_Valid()
        {
            IsUpperCaseValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsUpperCaseValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsUpperCaseValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsUpperCaseValidator_Should_Valid()
        {          
            IsUpperCaseValidator validator = CreateDefaultValidator();
            
            const string ALL_UPPERCASE = "UPPER";
            
            ValidationResult result = validator.Validate(ALL_UPPERCASE);
            
            Assert.Equal(ALL_UPPERCASE, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsUpperCaseValidator_Should_Invalid()
        {          
            IsUpperCaseValidator validator = CreateDefaultValidator();
            
            const string ALL_LOWERCASE = "lower";

            ValidationResult result = validator.Validate(ALL_LOWERCASE);
            
            Assert.Equal(ValidationMessage.NOT_UPPER_CASE, result.Message);
            Assert.False(result.IsValid);
        }

        private IsUpperCaseValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new() { AllowEmpty = allowEmpty };
        }
    }
}
