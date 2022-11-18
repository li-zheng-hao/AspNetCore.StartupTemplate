using System.ComponentModel.DataAnnotations;

namespace AspNetCore.StartUpTemplate.Utility.Validator;

public class StringMaxBytesAttribute:ValidationAttribute
{
    private readonly int _maxSize;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxSize">单位KB</param>
    public StringMaxBytesAttribute(int maxSize)
    {
        _maxSize = maxSize;
    }
    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        if (value.GetType() != typeof(string))
            throw new ArgumentException("只能用于字符串字节数计算");
        // 默认UTF-8
        var count = System.Text.Encoding.Default.GetByteCount(value.ToString()!);
        if(count>_maxSize*1024)
            return new ValidationResult($"当前字段{validationContext.MemberName}字节数为{count}超过了{_maxSize}最大限制");
        return ValidationResult.Success;
    }

   
}