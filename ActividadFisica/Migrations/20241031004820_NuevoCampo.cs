using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadFisica.Migrations
{
    /// <inheritdoc />
    public partial class NuevoCampo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioID",
                table: "Lugares",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Lugares");
        }
    }
}
