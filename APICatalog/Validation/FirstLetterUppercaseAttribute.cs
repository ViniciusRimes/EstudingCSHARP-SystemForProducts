using System.ComponentModel.DataAnnotations;

namespace APICatalog.Validation
{
    public class FirstLetterUppercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, 
        ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString())){
                return ValidationResult.Success;
            }
            var firstLetter = value.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("A primeira letra do nome do produto deve ser maíuscula!");
            }
            return ValidationResult.Success;
        }
    }
}
