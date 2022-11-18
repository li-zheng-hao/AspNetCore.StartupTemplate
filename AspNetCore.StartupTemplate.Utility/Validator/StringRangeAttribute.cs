using System.ComponentModel.DataAnnotations;

namespace AspNetCore.StartUpTemplate.Utility.Validator;

public class StringRangeAttribute : ValidationAttribute
{
    public string[] AllowableValues { get; set; }

    public StringRangeAttribute(params string[] allowValues)
    {
        AllowableValues = allowValues;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (AllowableValues?.Contains(value?.ToString()) == true)
        {
            return ValidationResult.Success;
        }
        var msg = $"请输入允许的值: {string.Join(", ", (AllowableValues ?? new string[] { "没有发现允许的值" }))}.";
        return new ValidationResult(msg);
    }
}