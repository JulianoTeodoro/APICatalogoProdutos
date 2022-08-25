using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class PopulaProdutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, dataCadastro, CategoriaId) " +
                "VALUES ('Coca-Cola', 'Refrigerante', 5.45, 'cocacola.jpg', 50, now(), 1)");
            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, dataCadastro, CategoriaId) " +
               "VALUES ('Fanta', 'Refrigerante', 4.50, 'fanta.jpg', 30, now(), 1)");
            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, dataCadastro, CategoriaId) " +
               "VALUES ('Coxinha', 'Salgados', 3.00, 'coxinha.jpg', 45, now(), 2)");
            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, dataCadastro, CategoriaId) " +
               "VALUES ('Sorvete', 'Sobremesa', 10.00, 'sorvete.jpg', 10, now(), 3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Produtos");
        }
    }
}
