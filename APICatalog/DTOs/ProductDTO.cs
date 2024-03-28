using APICatalog.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APICatalog.Models;
using System.Text.Json.Serialization;

namespace APICatalog.DTOs
{
    public class ProductDTO
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
        [Range(1, 999999, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Price { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
