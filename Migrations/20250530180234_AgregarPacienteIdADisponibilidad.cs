using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservas_backend.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPacienteIdADisponibilidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Disponibilidades",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Disponibilidades");
        }
    }
}
