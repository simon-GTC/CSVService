using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using CsvService.Core;

namespace CsvService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidationController : ControllerBase
    {
        public ValidationController()
        {
        }

        /// <summary>
        /// Validate a csv with attached configuration
        /// </summary>
        /// <param name="filePath" example="c:\data\csv\test.csv">a csv file path to validate</param>
        /// <param name="config">
        /// JSON config
        /// <para>delimiter: Csv field delimiter</para>
        /// <para>validateFrom (optional, default 1): Start validation from n line</para>
        /// <para>columnConfigs: Config for individual column. If there are n columns in the csv, there should be n column configs</para>
        /// <para>validatorConfigs: Config for individual value in a column. A column can contain multiple values. If there are n values in the column, there should be n validator config</para>
        /// <para>allowEmpty (optional, default false): A value cannot be empty by default</para>
        /// <para>validator: Name of validator. Available validators are listed below:</para>
        /// <para>IsIntegerValidator: value is an integer</para>
        /// <para>IsNumberValidator: value is a number, parameter(s): max decimal place</para>
        /// <para>IsDateValidator: value is a date, parameter(s): format</para>
        /// <para>NumberRangeValidator: value is within a specific range, parameter(s): range type, min value, max value</para>
        /// <para>IsUpperCaseValidator: value is a string in upper case</para>
        /// <para>IsLowerCaseValidator: value is a string in lower case</para>
        /// <para>IsBooleanValidator: value is a Boolean</para>
        /// <para>InListValidator: value is in a specific list, parameter(s): a specified list</para>
        /// <para>RegexValidator: value match a specific pattern, parameter(s): regex pattern</para>
        /// <para>IsKeyValueListValidator: value is a list of key values, parameter(s): list separator</para>
        /// <para>parameters: Parameter(s) to create the validator</para>
        /// </param>
        /// <response code="200">ok</response>
        /// <response code="400">request error or csv errors</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        [Route("Validate")]
        public ActionResult Validate(string filePath, Config config)
        {
            try
            {
                CsvValidator validator = new CsvValidator(config).Build();

                List<string> errors = validator.Validate(filePath);

                if (errors.Count == 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(errors);
                }

            }
            catch (FileNotFoundException)
            {
                return BadRequest("file cannot be found");
            }
            catch (CsvHelper.BadDataException)
            {
                return BadRequest("file is not a CSV");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
