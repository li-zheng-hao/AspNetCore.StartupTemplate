using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.StartUpTemplate.Utility.Validator;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        var files = value as IList<IFormFile>;
        foreach(var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            if (file != null)
            {
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(file.FileName));
                }
            }
        }
        
        return ValidationResult.Success;
    }

    public string GetErrorMessage(string name)
    {
        return $"{name} extension is not allowed!";
    }
}