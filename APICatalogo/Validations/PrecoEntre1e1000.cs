using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APICatalogo.Validations
{
    public class PrecoEntre1e1000 : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var valorPreco = double.Parse(value.ToString(), CultureInfo.InvariantCulture);

            if (valorPreco < 1.00 && valorPreco > 1000.00)
            {
                return new ValidationResult("O valor deve estar entre 1 e 1000");
            }
            return ValidationResult.Success;
        }
    }
}
