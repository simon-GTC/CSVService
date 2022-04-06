using System;
using Xunit;
using CsvService.Core;
using System.Collections.Generic;

namespace CsvService.Tests
{
    public class InListValidatorTests
    {
        [Fact]
        public void InListValidator_GetName_Should_Valid()
        {
            InListValidator validator = CreateDefaultValidator();

            Assert.Equal("InList_False_JPY,USD,EUR", validator.GetName());
        }

        [Fact]
        public void InListValidator_Allow_Empty_Should_Valid()
        {
            InListValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InListValidator_Not_Allow_Empty_Should_Invalid()
        {
            InListValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InListValidator_Should_Valid()
        {
            InListValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("USD");

            Assert.Equal("USD", result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void InListValidator_Init_With_String_Should_Valid()
        {
            InListValidator validator = new("JPY,USD,EUR");

            ValidationResult result = validator.Validate("USD");

            Assert.Equal("USD", result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void InListValidator_Should_Invalid()
        {
            InListValidator validator = CreateDefaultValidator();
            
            ValidationResult result = validator.Validate("UsD");

            Assert.Equal(ValidationMessage.NOT_IN_LIST, result.Message);
            Assert.False(result.IsValid);
        }

        private InListValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new(new List<string> { "JPY", "USD", "EUR" }) { AllowEmpty = allowEmpty };
        }
    }
}
