using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsicoAgendaApi.Migrations
{
    /// <inheritdoc />
    public partial class FixDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Pagamentos",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorSessao",
                table: "Pacientes",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_ConsultaId",
                table: "Pagamentos",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Consultas_ConsultaId",
                table: "Pagamentos",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Consultas_ConsultaId",
                table: "Pagamentos");

            migrationBuilder.DropIndex(
                name: "IX_Pagamentos_ConsultaId",
                table: "Pagamentos");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Pagamentos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorSessao",
                table: "Pacientes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }
    }
}
