using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Core.DomainObjects;
using FluentAssertions;

namespace EducationHub.Tests.Unit.Domain.Conteudo;

public class AulaTests
{
    [Fact]
    public void Aula_DeveCriarAula_ComDadosValidos()
    {
        // Arrange
        var titulo = "Introdução ao C#";
        var conteudoAula = "Nesta aula vamos aprender os conceitos básicos da linguagem C#";
        var materialDeApoio = "Slides em PDF disponíveis";
        var duracao = TimeSpan.FromMinutes(45);
        var cursoId = Guid.NewGuid();

        // Act
        var aula = new Aula(titulo, conteudoAula, materialDeApoio, duracao, cursoId);

        // Assert
        aula.Should().NotBeNull();
        aula.Titulo.Should().Be(titulo);
        aula.ConteudoAula.Should().Be(conteudoAula);
        aula.MaterialDeApoio.Should().Be(materialDeApoio);
        aula.Duracao.Should().Be(duracao);
        aula.CursoId.Should().Be(cursoId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Au")]
    public void Aula_DeveLancarExcecao_QuandoTituloInvalido(string tituloInvalido)
    {
        // Arrange
        var conteudoAula = "Nesta aula vamos aprender os conceitos básicos da linguagem C#";
        var materialDeApoio = "Slides em PDF disponíveis";
        var duracao = TimeSpan.FromMinutes(45);
        var cursoId = Guid.NewGuid();

        // Act & Assert
        Action act = () => new Aula(tituloInvalido, conteudoAula, materialDeApoio, duracao, cursoId);
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Cont")]
    public void Aula_DeveLancarExcecao_QuandoConteudoInvalido(string conteudoInvalido)
    {
        // Arrange
        var titulo = "Introdução ao C#";
        var materialDeApoio = "Slides em PDF disponíveis";
        var duracao = TimeSpan.FromMinutes(45);
        var cursoId = Guid.NewGuid();

        // Act & Assert
        Action act = () => new Aula(titulo, conteudoInvalido, materialDeApoio, duracao, cursoId);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Aula_DeveLancarExcecao_QuandoDuracaoMenorQueMinimo()
    {
        // Arrange
        var titulo = "Introdução ao C#";
        var conteudoAula = "Nesta aula vamos aprender os conceitos básicos da linguagem C#";
        var materialDeApoio = "Slides em PDF disponíveis";
        var duracao = TimeSpan.FromSeconds(30); // Menos de 1 minuto
        var cursoId = Guid.NewGuid();

        // Act & Assert
        Action act = () => new Aula(titulo, conteudoAula, materialDeApoio, duracao, cursoId);
        act.Should().Throw<DomainException>().WithMessage("*1 minuto*");
    }

    [Fact]
    public void Aula_DeveLancarExcecao_QuandoCursoIdVazio()
    {
        // Arrange
        var titulo = "Introdução ao C#";
        var conteudoAula = "Nesta aula vamos aprender os conceitos básicos da linguagem C#";
        var materialDeApoio = "Slides em PDF disponíveis";
        var duracao = TimeSpan.FromMinutes(45);
        var cursoId = Guid.Empty;

        // Act & Assert
        Action act = () => new Aula(titulo, conteudoAula, materialDeApoio, duracao, cursoId);
        act.Should().Throw<DomainException>().WithMessage("*CursoId*");
    }

    [Fact]
    public void Aula_DeveAtualizarConteudo_ComSucesso()
    {
        // Arrange
        var aula = CriarAulaValida();
        var novoConteudo = "Conteúdo atualizado com mais detalhes sobre C#";

        // Act
        aula.AtualizarConteudo(novoConteudo);

        // Assert
        aula.ConteudoAula.Should().Be(novoConteudo);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Aula_DeveLancarExcecao_AoAtualizarConteudoInvalido(string conteudoInvalido)
    {
        // Arrange
        var aula = CriarAulaValida();

        // Act & Assert
        Action act = () => aula.AtualizarConteudo(conteudoInvalido);
        act.Should().Throw<DomainException>();
    }

    private Aula CriarAulaValida()
    {
        return new Aula(
            "Introdução ao C#",
            "Nesta aula vamos aprender os conceitos básicos da linguagem C#",
            "Slides em PDF disponíveis",
            TimeSpan.FromMinutes(45),
            Guid.NewGuid()
        );
    }
}
