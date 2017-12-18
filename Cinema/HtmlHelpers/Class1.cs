using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cinema.HtmlHelpers
{
    public class ValidatonDate : CompareAttribute
    {
        private string _otherPropertyName;

        public ValidatonDate(string otherProperty) : base(otherProperty)
        {
            _otherPropertyName = otherProperty;
        }

        public ValidatonDate(string otherProperty, string errorMessage) : base(otherProperty)
        {
            ErrorMessage = errorMessage;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.GetType() != typeof(DateTime)) throw new ArgumentException("Атрибут применим только для DateTime? типа.");

            if ((DateTime)value < DateTime.Now) return new ValidationResult(ErrorMessage);
            else return ValidationResult.Success;
           
        }
    }
}