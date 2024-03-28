using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APICatalog.DTOs
{
    public class ProductDTOUpdateRequest : IValidatableObject
    {
        public DateTime RegistrationDate { get; set; }

        [Range(1, 9999, ErrorMessage = "O estoque deve estar entre 1 e 9999 unidades!")]
        public float Stock { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RegistrationDate.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("A data deve ser maior que a data atual", new[] { nameof(RegistrationDate) });
            }
        }
    }
}
