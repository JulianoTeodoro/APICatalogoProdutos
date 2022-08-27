﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(150)]
        public string? Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public float Estoque { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }
        
        public DateTime dataCadastro { get; set; }

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Categoria? categoria { get; set; }



    }
}
