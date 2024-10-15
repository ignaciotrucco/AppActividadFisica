using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadFisica.Migrations
{
    /// <inheritdoc />
    public partial class RelacionVirtualEventoEjercicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EjercicioFisico_EventoDeportivoID",
                table: "EjercicioFisico",
                column: "EventoDeportivoID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjercicioFisico_EventosDeportivos_EventoDeportivoID",
                table: "EjercicioFisico",
                column: "EventoDeportivoID",
                principalTable: "EventosDeportivos",
                principalColumn: "EventoDeportivoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjercicioFisico_EventosDeportivos_EventoDeportivoID",
                table: "EjercicioFisico");

            migrationBuilder.DropIndex(
                name: "IX_EjercicioFisico_EventoDeportivoID",
                table: "EjercicioFisico");
        }
    }
}
