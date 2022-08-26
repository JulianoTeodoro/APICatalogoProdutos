using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class PopulaCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO categorias(Nome, ImagemUrl) VALUES('Alimentos', 'alimento.jpg')");
            migrationBuilder.Sql("INSERT INTO categorias(Nome, ImagemUrl) VALUES('Bebidas', 'bebida.jpg')");
            migrationBuilder.Sql("INSERT INTO categorias(Nome, ImagemUrl) VALUES('Sobremesas', 'sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM categorias");
        }
    }
}
