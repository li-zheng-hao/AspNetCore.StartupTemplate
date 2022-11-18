using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.StartUpTemplate.Utility.Validator;


public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        var files = value as IList<IFormFile>;
        foreach(var file in files)
        {
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage(file.FileName));
                }
            }
        }    
        

        return ValidationResult.Success;
    }

    public string GetErrorMessage(string name)
    {
        return $"{name}'s size is out of range.Maximum allowed file size is { _maxFileSize} bytes.";
    }
}