using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsicoAgendaApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusPagamento",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "NomeCompleto",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "ValorSessao",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Consultas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Pagamentos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPagamento",
                table: "Pagamentos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPagamento",
                table: "Pagamentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Pagamentos",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Pagamentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Pacientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Pacientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Pacientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Pacientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Pacientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Pacientes",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "Pacientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmada",
                table: "Consultas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Consultas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Consultas",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Realizada",
                table: "Consultas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Relatorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    DataRelatorio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relatorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relatorios_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relatorios_PacienteId",
                table: "Relatorios",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relatorios");

            migrationBuilder.DropColumn(
                name: "MetodoPagamento",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Confirmada",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "Realizada",
                table: "Consultas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Pagamentos",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPagamento",
                table: "Pagamentos",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "StatusPagamento",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "NomeCompleto",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorSessao",
                table: "Pacientes",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Consultas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
