using System;

namespace CsvService.Core
{
    public struct ValidationMessage
    {   
        public const string CANNOT_BE_EMPTY = "the cell should not be empty";
        public const string NOT_AN_INTEGER = "the cell is not an integer";
        public const string NOT_A_NUMBER = "the cell is not a number";
        public const string TOO_MANY_DECIMAL_PLACES = "the cell value has too many decimal places";
        public const string NOT_CORRECT_DATE_FORMAT = "the cell is not in correct date format";
        public const string NUMBER_OUT_OF_RANGE = "the cell value is out of range";
        public const string NUMBER_NOT_POSITIVE = "the cell value is not positive";
        public const string NUMBER_NOT_NEGATIVE = "the cell value is not negative";
        public const string NOT_UPPER_CASE = "the cell is not in upper case";
        public const string NOT_LOWER_CASE = "the cell is not in lower case";
        public const string NOT_A_BOOLEAN = "the cell is not a boolean";
        public const string NOT_IN_LIST = "the cell is not in provided list";
        public const string PATTERN_NOT_MATCH = "the cell does not match the pattern";
        public const string NOT_VALID_LIST_OF_KEY_VALUES = "the cell is not a valid list of key values";
        public const string DUPLICATED_KEY = "the cell contains duplicated key";
    }
}
