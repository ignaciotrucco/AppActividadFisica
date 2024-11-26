using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadFisica.Migrations
{
    /// <inheritdoc />
    public partial class NuevoCampoMET : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NroMET",
                table: "Tipo_Ejercicios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NroMET",
                table: "Tipo_Ejercicios");
        }
    }
}
