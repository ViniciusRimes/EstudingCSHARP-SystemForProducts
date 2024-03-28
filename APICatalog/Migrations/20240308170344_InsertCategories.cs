using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalog.Migrations
{
    /// <inheritdoc />
    public partial class InsertCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into categories(Name, ImageURL) Values('Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("Insert into categories(Name, ImageURL) Values('Alimentos', 'alimentos.jpg')");
            migrationBuilder.Sql("Insert into categories(Name, ImageURL) Values('Carnes', 'carnes.jpg')");
            migrationBuilder.Sql("Insert into categories(Name, ImageURL) Values('Eletrônicos', 'eletronicos.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categories");
        }
    }
}
