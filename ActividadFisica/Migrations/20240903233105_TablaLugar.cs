using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadFisica.Migrations
{
    /// <inheritdoc />
    public partial class TablaLugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LugarID",
                table: "EjercicioFisico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lugares",
                columns: table => new
                {
                    LugarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugares", x => x.LugarID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropColumn(
                name: "LugarID",
                table: "EjercicioFisico");
        }
    }
}
