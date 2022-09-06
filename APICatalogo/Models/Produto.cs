using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome de produto é necessário!")]
        [StringLength(80, MinimumLength = 4)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "A descrição é necessário!")]
        [StringLength(150, MinimumLength = 5)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é necessário!")]
        [Column(TypeName = "decimal(10,2)")]
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


    }
}
