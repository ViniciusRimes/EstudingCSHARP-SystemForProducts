using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalog.Migrations
{
    /// <inheritdoc />
    public partial class InsertProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageURL, Stock, RegistrationDate, CategoryId) " +
                "Values('Coca-Cola 2.25L', 'Refrigerante Coca-Cola 2.25L', '9.99', 'bebidas.jpg', 100, now(), 1)");
            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageURL, Stock, RegistrationDate, CategoryId) " +
                "Values('Arroz Tio João 5kg', 'Arroz Tio João 5kg', '20.99', 'alimentos.jpg', 100, now(), 2)");
            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageURL, Stock, RegistrationDate, CategoryId) " +
                "Values('Peito de frango 1kg', 'Peito de frango 1kg', '14.99', 'carnes.jpg', 100, now(), 3)");
            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageURL, Stock, RegistrationDate, CategoryId) " +
                "Values('Peito de frango 1kg', 'Peito de frango 1kg', '14.99', 'carnes.jpg', 100, now(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categories");
        }
    }
}
