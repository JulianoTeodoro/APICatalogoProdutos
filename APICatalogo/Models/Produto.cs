using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    public class Produto// : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome de produto é necessário!")]
        [StringLength(80, MinimumLength = 4)]
        [PrimeiraLetraMaiuscula]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "A descrição é necessário!")]
        [StringLength(150, MinimumLength = 5)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é necessário!")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        [PrecoEntre1e1000]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O estoque é necessário!")]
        [Column(TypeName = "decimal(10,2)")]
        public float Estoque { get; set; }

        [Required(ErrorMessage = "A imagem é obrigatória!")]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }


       /* [Range(18, 65)]
        public int idade { get; set; }*/

        public DateTime dataCadastro { get; set; }

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Categoria? categoria { get; set; }

       /* public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if(primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new ValidationResult("A primeira letra deve ser maiuscula", new[]
                    {
                        nameof(this.Nome),
                    });
                }

                /*if (Preco < 1 && Preco > 1000)
                {
                    yield return new ValidationResult("O valor deve estar entre 1 e 1000", new[]
                    {
                        nameof(this.Preco)
                    });
                }*/
            }
        }
