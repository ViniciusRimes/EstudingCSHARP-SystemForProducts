using APICatalog.Validation;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.Models;
public class Product : IValidatableObject
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(30, ErrorMessage = "O nome deve ter entre 5 e 30 caracteres", MinimumLength = 5)]
    [FirstLetterUppercase]
    public string? Name { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "O descrição deve ter no máximo {1} caracteres")]
    public string? Description { get; set; }
 
    [Required]
    [Column(TypeName ="decimal(12,2)")]
    [Range(1, 999999, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    public decimal Price { get; set; }

    public float Stock { get; set; }
    
    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImageUrl { get; set; }

    public DateTime RegistrationDate { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Name.ToString()))
        {
            var firstLetter = Name.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do nome do produto deve ser maíuscula!", new[]
                {
                    nameof(Name)
                });
            }
        }
        if(Stock <= 0) {
            yield return new ValidationResult("O estoque do produto deve ser maior que zero", new[]
                {
                    nameof(Stock)
                });
        }
    }
}
