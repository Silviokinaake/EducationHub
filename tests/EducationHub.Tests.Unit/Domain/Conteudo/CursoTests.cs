using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Conteudo.Domain.Enums;
using EducationHub.Core.DomainObjects;
using FluentAssertions;

namespace EducationHub.Tests.Unit.Domain.Conteudo;

public class CursoTests
{
    [Fact]
    public void Curso_DeveCriarCurso_ComDadosValidos()
    {
        // Arrange
        var titulo = "Curso de C#";
        var descricao = "Aprenda C# do básico ao avançado com este curso completo.";
        var cargaHoraria = TimeSpan.FromHours(40);
        var instrutor = "Professor João";
        var nivel = "Intermediário";
        var conteudoProgramatico = new ConteudoProgramatico(
            "Aprender C# do básico ao avançado",
            "Módulo 1: Introdução\nMódulo 2: Avançado",
            "Aulas práticas e teóricas",
            "Livro de C#, Documentação Microsoft"
        );

        // Act
        var curso = new Curso(titulo, descricao, cargaHoraria, instrutor, nivel, 100.00m, conteudoProgramatico);

        // Assert
        curso.Should().NotBeNull();
        curso.Titulo.Should().Be(titulo);
        curso.Descricao.Should().Be(descricao);
        curso.CargaHoraria.Should().Be(cargaHoraria);
        curso.Instrutor.Should().Be(instrutor);
        curso.Nivel.Should().Be(nivel);
        curso.ConteudoProgramatico.Should().Be(conteudoProgramatico);
        curso.Situacao.Should().Be(SituacaoCurso.Ativo);
        curso.Aulas.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Cu")]
    public void Curso_DeveLancarExcecao_QuandoTituloInvalido(string tituloInvalido)
    {
        // Arrange
        var descricao = "Aprenda C# do básico ao avançado com este curso completo.";
        var cargaHoraria = TimeSpan.FromHours(40);
        var instrutor = "Professor João";
        var nivel = "Intermediário";
        var conteudoProgramatico = new ConteudoProgramatico(
            "Aprender C# do básico ao avançado",
            "Módulo 1: Introdução\nMódulo 2: Avançado",
            "Aulas práticas e teóricas",
            "Livro de C#, Documentação Microsoft"
        );

        // Act & Assert
        Action act = () => new Curso(tituloInvalido, descricao, cargaHoraria, instrutor, nivel, 150.00m, conteudoProgramatico);
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Desc")]
    public void Curso_DeveLancarExcecao_QuandoDescricaoInvalida(string descricaoInvalida)
    {
        // Arrange
        var titulo = "Curso de C#";
        var cargaHoraria = TimeSpan.FromHours(40);
        var instrutor = "Professor João";
        var nivel = "Intermediário";
        var conteudoProgramatico = new ConteudoProgramatico(
            "Aprender C# do básico ao avançado",
            "Módulo 1: Introdução\nMódulo 2: Avançado",
            "Aulas práticas e teóricas",
            "Livro de C#, Documentação Microsoft"
        );

        // Act & Assert
        Action act = () => new Curso(titulo, descricaoInvalida, cargaHoraria, instrutor, nivel, 200.00m, conteudoProgramatico);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Curso_DeveLancarExcecao_QuandoCargaHorariaMenorQueMinimo()
    {
        // Arrange
        var titulo = "Curso de C#";
        var descricao = "Aprenda C# do básico ao avançado com este curso completo.";
        var cargaHoraria = TimeSpan.FromMinutes(20); // Menor que 30 minutos
        var instrutor = "Professor João";
        var nivel = "Intermediário";
        var conteudoProgramatico = new ConteudoProgramatico(
            "Aprender C# do básico ao avançado",
            "Módulo 1: Introdução\nMódulo 2: Avançado",
            "Aulas práticas e teóricas",
            "Livro de C#, Documentação Microsoft"
        );

        // Act & Assert
        Action act = () => new Curso(titulo, descricao, cargaHoraria, instrutor, nivel, 250.00m, conteudoProgramatico);
        act.Should().Throw<DomainException>().WithMessage("*30 minutos*");
    }

    [Fact]
    public void Curso_DeveLancarExcecao_QuandoConteudoProgramaticoNulo()
    {
        // Arrange
        var titulo = "Curso de C#";
        var descricao = "Aprenda C# do básico ao avançado com este curso completo.";
        var cargaHoraria = TimeSpan.FromHours(40);
        var instrutor = "Professor João";
        var nivel = "Intermediário";

        // Act & Assert
        Action act = () => new Curso(titulo, descricao, cargaHoraria, instrutor, nivel, 300.00m, null);
        act.Should().Throw<DomainException>().WithMessage("*conteúdo programático*");
    }

    [Fact]
    public void Curso_DeveAdicionarAula_ComSucesso()
    {
        // Arrange
        var curso = CriarCursoValido();
        var aula = new Aula("Aula 1: Introdução", "Conteúdo da aula de introdução", "Material de apoio", TimeSpan.FromHours(2), curso.Id);

        // Act
        curso.AdicionarAula(aula);

        // Assert
        curso.Aulas.Should().HaveCount(1);
        curso.Aulas.Should().Contain(aula);
    }

    [Fact]
    public void Curso_DeveLancarExcecao_AoAdicionarAulaNula()
    {
        // Arrange
        var curso = CriarCursoValido();

        // Act & Assert
        Action act = () => curso.AdicionarAula(null!);
        act.Should().Throw<DomainException>().WithMessage("*aula não pode ser nula*");
    }

    [Fact]
    public void Curso_DeveAtualizarDescricao_ComSucesso()
    {
        // Arrange
        var curso = CriarCursoValido();
        var novaDescricao = "Nova descrição detalhada do curso de C# avançado.";

        // Act
        curso.AtualizarDescricao(novaDescricao);

        // Assert
        curso.Descricao.Should().Be(novaDescricao);
    }

    [Fact]
    public void Curso_DeveDesativar_ComSucesso()
    {
        // Arrange
        var curso = CriarCursoValido();

        // Act
        curso.Desativar();

        // Assert
        curso.Situacao.Should().Be(SituacaoCurso.Inativo);
    }

    [Fact]
    public void Curso_DeveAtivar_ComSucesso()
    {
        // Arrange
        var curso = CriarCursoValido();
        curso.Desativar();

        // Act
        curso.Ativar();

        // Assert
        curso.Situacao.Should().Be(SituacaoCurso.Ativo);
    }

    private Curso CriarCursoValido()
    {
        return new Curso(
            "Curso de C#",
            "Aprenda C# do básico ao avançado com este curso completo.",
            TimeSpan.FromHours(40),
            "Professor João",
            "Intermediário",
            350.00m,
            new ConteudoProgramatico(
                "Aprender C# do básico ao avançado",
                "Módulo 1: Introdução\nMódulo 2: Avançado",
                "Aulas práticas e teóricas",
                "Livro de C#, Documentação Microsoft"
            )
        );
    }
}
