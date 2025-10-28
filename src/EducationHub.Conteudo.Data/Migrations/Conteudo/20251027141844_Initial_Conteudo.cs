using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationHub.Conteudo.Data.Migrations.Conteudo
{
    /// <inheritdoc />
    public partial class Initial_Conteudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CargaHoraria = table.Column<TimeSpan>(type: "time", nullable: false),
                    Instrutor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Nivel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ConteudoProgramatico_Objetivo = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ConteudoProgramatico_Conteudo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    ConteudoProgramatico_Metodologia = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    ConteudoProgramatico_Bibliografia = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CursoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ConteudoAula = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false),
                    MaterialDeApoio = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Duracao = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aulas_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_CursoId",
                table: "Aulas",
                column: "CursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "Cursos");
        }
    }
}
