using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    public sealed class Categoria
    {

        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome de categoria é necessária!")]
        [StringLength(80, MinimumLength = 4)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "A imagem é obrigatória!")]
        [StringLength(300, MinimumLength = 5)]
        public string? ImagemUrl { get; set; }
        public ICollection<Produto>? produtos { get; set; }

        public Categoria()
        {
            produtos = new List<Produto>();
        }


    }
}
