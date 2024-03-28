using APICatalog.Models;
using APICatalog.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.DTOs
{
    public class ProductDTOUpdateResponse
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public float Stock { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int CategoryId { get; set; }
    }
}
