using Questlog.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

public class ValidArchetypeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Archetype is required.");
        }

        // Print the actual value and its type for debugging
        Console.WriteLine($"Received value: {value} (Type: {value.GetType()})");

        // Since value is of type Archetype, we can assume it is valid if not null
        // (or if you want to explicitly check it is a defined enum, you can do so)
        if (value is Archetype)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The value '{value}' is not a valid archetype.");
    }
}
