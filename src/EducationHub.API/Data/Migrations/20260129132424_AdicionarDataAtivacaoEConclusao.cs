using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationHub.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDataAtivacaoEConclusao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Cursos",
                newName: "Situacao");

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

            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Cursos",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
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

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Cursos");

            migrationBuilder.RenameColumn(
                name: "Situacao",
                table: "Cursos",
                newName: "Ativo");
        }
    }
}
