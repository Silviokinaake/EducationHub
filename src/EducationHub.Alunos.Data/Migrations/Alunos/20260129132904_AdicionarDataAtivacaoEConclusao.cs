using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationHub.Alunos.Data.Migrations.Alunos
{
    /// <inheritdoc />
    public partial class AdicionarDataAtivacaoEConclusao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtivacao",
                table: "Matriculas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataConclusao",
                table: "Matriculas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAtivacao",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "DataConclusao",
                table: "Matriculas");
        }
    }
}
