# CSV Validation Service

The goal is to create a .Net Core service that will accept REST calls to validate a CSV file.  Imagine that this service would be used by other services that create CSV files to be sent automatically to clients. We want to check that the end-result files are valid, and follow a set of rules set for each column value. 

## Create Configuration
#### Definition
```json
{
  "delimiter": "string",
  "validateFrom": 0,
  "columnConfigs": [
    {
      "validatorConfigs": [
        {
          "allowEmpty": true,
          "validator": "string",
          "parameters": [
            "string"
          ]
        }
      ]
    }
  ]
}
```

"delimiter": Csv field delimiter (ex: , |)
"validateFrom": (optional, default: 1) Start validation from n line
"columnConfigs": Config for individual column. If there are n columns in the csv, there should be n column configs
"validatorConfigs": Config for individual value in a column. A column can contain multiple values. If there are n values in the column, there should be n validator config
"allowEmpty": (optional, default: false) A value cannot be empty by default
"validator": Name of validator. Available validators are listed below:
+ IsIntegerValidator: value is an integer
+ IsNumberValidator: value is a number
    + parameter: max decimal place allowed (ex: if 3 is the max, then 2.1234 is invalid and 5.236 is valid).
+ IsDateValidator: value is a date, parameter(s): format
    + parameter: date format (ex: if format is yyyy-MM-dd, then 2022-04-01 is valid )
+ NumberRangeValidator: value is within a specific range, 
    + parameter 1: range type. available values:
        + SpecificRange: value is between two values. parameter 2 and 3 must be provided
        + Positive: value is positive. do not need parameter 2 and 3
        + Negative: value is negative. do not need parameter 2 and 3
    + parameter 2: minimum value. must be provided if parameter 1 is SpecificRange
    + parameter 3: maximum value. must be provided if parameter 1 is SpecificRange
+ IsUpperCaseValidator: value is a string in upper case
+ IsLowerCaseValidator: value is a string in lower case
+ IsBooleanValidator: value is a Boolean
+ InListValidator: value is in a specific list
    + parameter: a specified list (ex: valid values are “JPY, USD, EUR”. If value is CAD or Jpy or UsD, then cell is invalid).
+ RegexValidator: value match a specific pattern
    + parameter: regex pattern (ex: if pattern is \^[A-Z][a-z]+$, then Az is valid and aZ is invalid)
+ IsKeyValueListValidator: value is a list of key values
    + parameter: list separator (ex: if | is separator and value is key1=value1|key2=value2, so we validate we have key/value pairs separated by | and they follow the key=value pattern and each value is valid)

### Example
```json
{
   "Delimiter":",",
   "ValidateFrom":2,
   "ColumnConfigs":[
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsIntegerValidator",
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":true,
               "Validator":"IsIntegerValidator",
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsNumberValidator",
               "Parameters":[
                  "3"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsDateValidator",
               "Parameters":[
                  "yyyy-MM-dd"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"NumberRangeValidator",
               "Parameters":[
                  "SpecificRange",
                  "0",
                  "100"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"NumberRangeValidator",
               "Parameters":[
                  "Positive"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"NumberRangeValidator",
               "Parameters":[
                  "Negative"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsUpperCaseValidator",
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsLowerCaseValidator",
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsBooleanValidator",
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"InListValidator",
               "Parameters":[
                  "JPY,USD,EUR"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"RegexValidator",
               "Parameters":[
                  "^[A-Z][a-z]+$"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":false,
               "Validator":"IsKeyValueListValidator",
               "Parameters":[
                  "|"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[
            {
               "AllowEmpty":null,
               "Validator":"IsIntegerValidator",
               "Parameters":null
            },
            {
               "AllowEmpty":null,
               "Validator":"NumberRangeValidator",
               "Parameters":[
                  "SpecificRange",
                  "0",
                  "100"
               ]
            },
            {
               "AllowEmpty":null,
               "Validator":"IsNumberValidator",
               "Parameters":[
                  "3"
               ]
            }
         ]
      }
   ]
}
```

### Example Explanation
```json
{
   "Delimiter":",", -- the delimiter of the csv is comma
   "ValidateFrom":2, -- validator will validate from line 2
   "ColumnConfigs":[ -- there are 14 items in this array, meaning there will be 13 columns in the csv
      {
         "ValidatorConfigs":[ -- column 1, only one value in the cell
            {
               "AllowEmpty":false, -- the cell does not allow empty
               "Validator":"IsIntegerValidator", -- cell value is an integer
               "Parameters":null -- no parameter to construct the validator
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 2
            {
               "AllowEmpty":true, -- the cell allow empty
               "Validator":"IsIntegerValidator", -- cell value is an integer
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 3
            {
               "AllowEmpty":false,
               "Validator":"IsNumberValidator", -- cell value is a number
               "Parameters":[ -- only one parameter is required
                  "3" -- maximum decimal place is 3
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 4
            {
               "AllowEmpty":false,
               "Validator":"IsDateValidator", -- cell value is a date
               "Parameters":[ -- only one parameter is required
                  "yyyy-MM-dd" -- the date formate is yyyy-MM-dd
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 5
            {
               "AllowEmpty":false,
               "Validator":"NumberRangeValidator", cell value is a number/integer in the specified range of value
               "Parameters":[ -- take up to 3 parameters
                  "SpecificRange", -- cell value between parameter 2 (0 in this example) and parameter 3 (100 in this example), required all 3 parameters
                  "0", -- the minimum of the cell value
                  "100" -- maximum of the cell value
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 6
            {
               "AllowEmpty":false,
               "Validator":"NumberRangeValidator", -- cell value is a number/integer in the specified range of value
               "Parameters":[
                  "Positive" -- cell value is positive
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 7
            {
               "AllowEmpty":false,
               "Validator":"NumberRangeValidator", -- cell value is a number/integer in the specified range of value
               "Parameters":[
                  "Negative" -- cell value is negative
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 8
            {
               "AllowEmpty":false,
               "Validator":"IsUpperCaseValidator", -- cell value is a string in upper case
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 9
            {
               "AllowEmpty":false,
               "Validator":"IsLowerCaseValidator", -- cell value is a string in lower case
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 10
            {
               "AllowEmpty":false,
               "Validator":"IsBooleanValidator", -- cell value is a Boolean
               "Parameters":null
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 11
            {
               "AllowEmpty":false,
               "Validator":"InListValidator", -- cell value is contained in a specified list
               "Parameters":[
                  "JPY,USD,EUR" -- valid values are "JPY,USD,EUR"
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 12
            {
               "AllowEmpty":false,
               "Validator":"RegexValidator", -- cell follow the Regex
               "Parameters":[
                  "^[A-Z][a-z]+$" -- cell value must match this pattern
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 13
            {
               "AllowEmpty":false,
               "Validator":"IsKeyValueListValidator", -- the cell is a list of key values with list separator (ex: value is key1=value|key2=value2 means there are 2 key/value pairs)
               "Parameters":[
                  "|" -- list separator
               ]
            }
         ]
      },
      {
         "ValidatorConfigs":[ -- column 14, the cell has 3 values, separated by comma (ex: 1,2,3.123)
            {
               "AllowEmpty":null,
               "Validator":"IsIntegerValidator", -- first value of the cell is an integer
               "Parameters":null
            },
            {
               "AllowEmpty":null,
               "Validator":"NumberRangeValidator", -- second value of the cell is between 0 and 100
               "Parameters":[
                  "SpecificRange",
                  "0",
                  "100"
               ]
            },
            {
               "AllowEmpty":null,
               "Validator":"IsNumberValidator", -- third value of the cell is a number with a maximum of 3 decimal places
               "Parameters":[
                  "3"
               ]
            }
         ]
      }
   ]
}
```