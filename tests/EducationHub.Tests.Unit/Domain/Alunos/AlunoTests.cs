using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Core.DomainObjects;
using FluentAssertions;

namespace EducationHub.Tests.Unit.Domain.Alunos;

public class AlunoTests
{
    [Fact]
    public void Aluno_DeveCriarAluno_ComDadosValidos()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var nome = "João Silva";
        var email = "joao.silva@email.com";
        var dataNascimento = DateTime.UtcNow.AddYears(-20);

        // Act
        var aluno = new Aluno(Guid.NewGuid(), usuarioId, nome, email, dataNascimento);

        // Assert
        aluno.Should().NotBeNull();
        aluno.UsuarioId.Should().Be(usuarioId);
        aluno.Nome.Should().Be(nome);
        aluno.Email.Should().Be(email);
        aluno.DataNascimento.Should().Be(dataNascimento);
        aluno.Matriculas.Should().BeEmpty();
        aluno.Certificados.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Jo")]
    public void Aluno_DeveLancarExcecao_QuandoNomeInvalido(string nomeInvalido)
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var email = "joao.silva@email.com";
        var dataNascimento = DateTime.UtcNow.AddYears(-20);

        // Act & Assert
        Action act = () => new Aluno(Guid.NewGuid(), usuarioId, nomeInvalido, email, dataNascimento);
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("emailinvalido")]
    [InlineData("email@")]
    [InlineData("@email.com")]
    public void Aluno_DeveLancarExcecao_QuandoEmailInvalido(string emailInvalido)
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var nome = "João Silva";
        var dataNascimento = DateTime.UtcNow.AddYears(-20);

        // Act & Assert
        Action act = () => new Aluno(Guid.NewGuid(), usuarioId, nome, emailInvalido, dataNascimento);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Aluno_DeveLancarExcecao_QuandoUsuarioIdVazio()
    {
        // Arrange
        var usuarioId = Guid.Empty;
        var nome = "João Silva";
        var email = "joao.silva@email.com";
        var dataNascimento = DateTime.UtcNow.AddYears(-20);

        // Act & Assert
        Action act = () => new Aluno(Guid.NewGuid(), usuarioId, nome, email, dataNascimento);
        act.Should().Throw<DomainException>().WithMessage("*UsuarioId*");
    }

    [Fact]
    public void Aluno_DeveLancarExcecao_QuandoIdadeMenorQueMinimo()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var nome = "João Silva";
        var email = "joao.silva@email.com";
        var dataNascimento = DateTime.UtcNow.AddYears(-8); // 8 anos

        // Act & Assert
        Action act = () => new Aluno(Guid.NewGuid(), usuarioId, nome, email, dataNascimento);
        act.Should().Throw<DomainException>().WithMessage("*10 anos*");
    }

    [Fact]
    public void Aluno_DeveAlterarNome_ComSucesso()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var novoNome = "Maria Santos";

        // Act
        aluno.SetNome(novoNome);

        // Assert
        aluno.Nome.Should().Be(novoNome);
    }

    [Fact]
    public void Aluno_DeveAlterarEmail_ComSucesso()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var novoEmail = "maria.santos@email.com";

        // Act
        aluno.SetEmail(novoEmail);

        // Assert
        aluno.Email.Should().Be(novoEmail);
    }

    private Aluno CriarAlunoValido()
    {
        return new Aluno(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "João Silva",
            "joao.silva@email.com",
            DateTime.UtcNow.AddYears(-20)
        );
    }
}
