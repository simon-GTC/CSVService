using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class IsNumberValidatorTests
    {
        [Fact]
        public void IsNumberValidator_GetName_Should_Valid()
        {
            IsNumberValidator validator = CreateDefaultValidator();

            Assert.Equal("IsNumber_False_3", validator.GetName());
        }

        [Fact]
        public void IsNumberValidator_Allow_Empty_Should_Valid()
        {
            IsNumberValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsNumberValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsNumberValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsNumberValidator_Without_Max_Decimal_Places_Should_Valid()
        {
            IsNumberValidator validator = new();

            const string NUMBER_WITH_LONG_DECIMAL_PLACES = "100.123456789";

            ValidationResult result = validator.Validate(NUMBER_WITH_LONG_DECIMAL_PLACES);

            Assert.Equal(100.123456789m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsNumberValidator_With_Max_Decimal_Places_Should_Valid()
        {            
            IsNumberValidator validator = CreateDefaultValidator();

            const string NUMBER_WITH_MAX_DECIMAL_PLACES = "100.123";

            ValidationResult result = validator.Validate(NUMBER_WITH_MAX_DECIMAL_PLACES);

            Assert.Equal(100.123m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);     
        }

        [Fact]
        public void IsNumberValidator_Init_With_String_Should_Valid()
        {
            IsNumberValidator validator = new("3");
            
            const string NUMBER_WITH_MAX_DECIMAL_PLACES = "100.123";

            ValidationResult result = validator.Validate(NUMBER_WITH_MAX_DECIMAL_PLACES);

            Assert.Equal(100.123m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);     
        }

        [Fact]
        public void IsNumberValidator_With_Non_Number_Should_Invalid()
        {
            IsNumberValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("x");

            Assert.Equal(ValidationMessage.NOT_A_NUMBER, result.Message);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsNumberValidator_With_Exceeded_Decimal_Places_Should_Invalid()
        {
            IsNumberValidator validator = CreateDefaultValidator();

            const string NUMBER_WITH_FOUR_DECIMAL_PLACES = "100.1234";

            ValidationResult result = validator.Validate(NUMBER_WITH_FOUR_DECIMAL_PLACES);

            Assert.Equal(ValidationMessage.TOO_MANY_DECIMAL_PLACES, result.Message);
            Assert.False(result.IsValid);
        }

        private IsNumberValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new(3) { AllowEmpty = allowEmpty };
        }
    }
}
