using Questlog.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

public class ValidArchetypeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult("Archetype is required.");
        }

        if (Enum.IsDefined(typeof(Archetype), value))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The value '{value}' is not a valid archetype.");
    }
}
