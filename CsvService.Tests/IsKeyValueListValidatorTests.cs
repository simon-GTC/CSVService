using System;
using Xunit;
using CsvService.Core;
using System.Collections.Generic;

namespace CsvService.Tests
{
    public class IsKeyValueListValidatorTests
    {
        [Fact]
        public void IsKeyValueListValidator_GetName_Should_Valid()
        {
            IsKeyValueListValidator validator = CreateDefaultValidator();

            Assert.Equal("IsKeyValueList_False", validator.GetName());
        }

        [Fact]
        public void IsKeyValueListValidator_Allow_Empty_Should_Valid()
        {
            IsKeyValueListValidator validator = CreateDefaultValidator(allowEmpty: true);

            ValidationResult result = validator.Validate("");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsKeyValueListValidator_Not_Allow_Empty_Should_Invalid()
        {
            IsKeyValueListValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate("");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsKeyValueListValidator_Should_Valid()
        {          
            IsKeyValueListValidator validator = CreateDefaultValidator();
            
            const string VALID_KEY_VALUES = "key1=value1|key2=value2";
            
            ValidationResult result = validator.Validate(VALID_KEY_VALUES);
            
            Dictionary<string, string> exptectedValue = new Dictionary<string, string>();
            exptectedValue.Add("key1","value1");
            exptectedValue.Add("key2","value2");
            
            Assert.Equal(exptectedValue, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsKeyValueListValidator_With_Specified_Separator_Should_Valid()
        {          
            IsKeyValueListValidator validator = CreateDefaultValidator(separator: ",");
            
            const string VALID_KEY_VALUES = "key1=value1,key2=value2";
            
            ValidationResult result = validator.Validate(VALID_KEY_VALUES);
            
            Dictionary<string, string> exptectedValue = new Dictionary<string, string>();
            exptectedValue.Add("key1","value1");
            exptectedValue.Add("key2","value2");
            
            Assert.Equal(exptectedValue, result.ValidValue);
            Assert.Null(result.Message);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("key1=value1,key2=value2")]
        [InlineData("key1=value1|key1")]
        [InlineData("key1=value1|key1=value2=value3")]
        public void IsKeyValueListValidator_Should_Invalid(string value)
        {          
            IsKeyValueListValidator validator = CreateDefaultValidator();

            ValidationResult result = validator.Validate(value);
            
            Assert.Equal(ValidationMessage.NOT_VALID_LIST_OF_KEY_VALUES, result.Message);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsKeyValueListValidator_With_Duplicated_Key_Should_Invalid()
        {          
            IsKeyValueListValidator validator = CreateDefaultValidator();

            const string DUPLICATED_KEY = "key1=value1|key1=value2";
            
            ValidationResult result = validator.Validate(DUPLICATED_KEY);
            
            Assert.Equal(ValidationMessage.DUPLICATED_KEY, result.Message);
            Assert.False(result.IsValid);
        }

        private IsKeyValueListValidator CreateDefaultValidator(string separator = "|", bool allowEmpty = false)
        {
            return new(separator) { AllowEmpty = allowEmpty };
        }
    }
}
