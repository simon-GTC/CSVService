using System;
using Xunit;
using CsvService.Core;

namespace CsvService.Tests
{
    public class NumberRangeValidatorTests
    {
        [Fact]
        public void NumberRangeValidator_GetName_Should_Valid()
        {
            NumberRangeValidator validator = CreateDefaultValidator();

            Assert.Equal("NumberRange_False_SpecificRange_0_100", validator.GetName());
        }

        [Fact]
        public void NumberRangeValidator_Allow_Empty_Should_Valid()
        {
            NumberRangeValidator validator = CreateDefaultValidator(true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Not_Allow_Empty_Should_Invalid()
        {
            NumberRangeValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Specific_Range_Should_Valid()
        {
            NumberRangeValidator validator = CreateDefaultValidator();

            const string IN_RANGE_NUMBER = "0";

            ValidationResult result = validator.Validate(IN_RANGE_NUMBER);

            Assert.Equal(0m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Init_With_String_Specific_Range_Should_Valid()
        {
            NumberRangeValidator validator = new("SpecificRange", "0", "100");

            const string IN_RANGE_NUMBER = "0";

            ValidationResult result = validator.Validate(IN_RANGE_NUMBER);

            Assert.Equal(0m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Positive_Should_Valid()
        {
            NumberRangeValidator validator = new(NumberRangeValidator.RangeType.Positive);

            const string POSITIVE_NUMBER = "1";

            ValidationResult result = validator.Validate(POSITIVE_NUMBER);

            Assert.Equal(1m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Negative_Should_Valid()
        {
            NumberRangeValidator validator = new(NumberRangeValidator.RangeType.Negative);

            const string NEGATIVE_NUMBER = "-1";

            ValidationResult result = validator.Validate(NEGATIVE_NUMBER);

            Assert.Equal(-1m, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Out_Of_Range_Should_Valid()
        {
            NumberRangeValidator validator = CreateDefaultValidator();

            const string OUT_OF_RANGE_NUMBER = "110";

            ValidationResult result = validator.Validate(OUT_OF_RANGE_NUMBER);

            Assert.Equal(ValidationMessage.NUMBER_OUT_OF_RANGE, result.Message);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Not_Positive_Should_Valid()
        {
            NumberRangeValidator validator = new(NumberRangeValidator.RangeType.Positive);

            const string NEGATIVE_NUMBER = "-1";

            ValidationResult result = validator.Validate(NEGATIVE_NUMBER);

            Assert.Equal(ValidationMessage.NUMBER_NOT_POSITIVE, result.Message);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void NumberRangeValidator_Not_Negative_Should_Valid()
        {
            NumberRangeValidator validator = new(NumberRangeValidator.RangeType.Negative);

            const string POSITIVE_NUMBER = "1";

            ValidationResult result = validator.Validate(POSITIVE_NUMBER);

            Assert.Equal(ValidationMessage.NUMBER_NOT_NEGATIVE, result.Message);
            Assert.False(result.IsValid);
        }

        private NumberRangeValidator CreateDefaultValidator(bool allowEmpty = false)
        {
            return new(0, 100) { AllowEmpty = allowEmpty };
        }
    }
}
