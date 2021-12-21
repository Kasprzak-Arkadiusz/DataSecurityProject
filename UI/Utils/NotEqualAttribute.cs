using System;
using System.ComponentModel.DataAnnotations;

namespace UI.Utils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "The value of {0} cannot be the same as the value of the {1}.";
        private string OtherProperty { get; }
        private string OtherPropertyName { get; }

        public NotEqualAttribute(string otherProperty, string otherPropertyName)
            : base(DefaultErrorMessage)
        {
            OtherProperty = otherProperty;
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);

            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

            if (value.Equals(otherPropertyValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherPropertyName);
        }
    }
}