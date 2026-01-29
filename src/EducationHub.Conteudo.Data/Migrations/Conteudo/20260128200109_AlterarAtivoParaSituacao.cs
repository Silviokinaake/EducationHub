using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationHub.Conteudo.Data.Migrations.Conteudo
{
    /// <inheritdoc />
    public partial class AlterarAtivoParaSituacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Cursos",
                newName: "Situacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Situacao",
                table: "Cursos",
                newName: "Ativo");
        }
    }
}
