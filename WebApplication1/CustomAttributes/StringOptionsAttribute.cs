﻿using System.ComponentModel.DataAnnotations;

namespace CustomAttributes
{
    public class StringOptionsAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; } = null!;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
            return new ValidationResult(msg);
        }

    }
}
