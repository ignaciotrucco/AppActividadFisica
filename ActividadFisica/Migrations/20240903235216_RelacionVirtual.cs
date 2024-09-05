using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadFisica.Migrations
{
    /// <inheritdoc />
    public partial class RelacionVirtual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EjercicioFisico_LugarID",
                table: "EjercicioFisico",
                column: "LugarID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjercicioFisico_Lugares_LugarID",
                table: "EjercicioFisico",
                column: "LugarID",
                principalTable: "Lugares",
                principalColumn: "LugarID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjercicioFisico_Lugares_LugarID",
                table: "EjercicioFisico");

            migrationBuilder.DropIndex(
                name: "IX_EjercicioFisico_LugarID",
                table: "EjercicioFisico");
        }
    }
}
