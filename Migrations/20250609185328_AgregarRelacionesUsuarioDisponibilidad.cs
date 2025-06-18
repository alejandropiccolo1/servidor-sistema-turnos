using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservas_backend.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRelacionesUsuarioDisponibilidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Disponibilidades_PacienteId",
                table: "Disponibilidades",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Disponibilidades_ProfesionalId",
                table: "Disponibilidades",
                column: "ProfesionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disponibilidades_Usuarios_PacienteId",
                table: "Disponibilidades",
                column: "PacienteId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Disponibilidades_Usuarios_ProfesionalId",
                table: "Disponibilidades",
                column: "ProfesionalId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disponibilidades_Usuarios_PacienteId",
                table: "Disponibilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Disponibilidades_Usuarios_ProfesionalId",
                table: "Disponibilidades");

            migrationBuilder.DropIndex(
                name: "IX_Disponibilidades_PacienteId",
                table: "Disponibilidades");

            migrationBuilder.DropIndex(
                name: "IX_Disponibilidades_ProfesionalId",
                table: "Disponibilidades");
        }
    }
}
