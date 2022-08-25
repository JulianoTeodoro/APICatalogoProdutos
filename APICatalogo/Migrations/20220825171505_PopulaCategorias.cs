using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class PopulaCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Alimentos', 'alimentos.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Sobremesas', 'sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
        }
    }
}
