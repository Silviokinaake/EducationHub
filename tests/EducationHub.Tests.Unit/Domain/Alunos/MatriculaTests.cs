using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Enums;
using EducationHub.Core.DomainObjects;
using FluentAssertions;

namespace EducationHub.Tests.Unit.Domain.Alunos;

public class MatriculaTests
{
    [Fact]
    public void Matricula_DeveCriarMatricula_ComDadosValidos()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var valor = 1500m;
        var dataMatricula = DateTime.UtcNow;

        // Act
        var matricula = new Matricula(cursoId, alunoId, valor, dataMatricula);

        // Assert
        matricula.Should().NotBeNull();
        matricula.CursoId.Should().Be(cursoId);
        matricula.AlunoId.Should().Be(alunoId);
        matricula.Valor.Should().Be(valor);
        matricula.DataMatricula.Should().Be(dataMatricula);
        matricula.Status.Should().Be(StatusMatriculaEnum.Pendente);
    }

    [Fact]
    public void Matricula_DeveLancarExcecao_QuandoCursoIdVazio()
    {
        // Arrange
        var cursoId = Guid.Empty;
        var alunoId = Guid.NewGuid();
        var valor = 1500m;
        var dataMatricula = DateTime.UtcNow;

        // Act & Assert
        Action act = () => new Matricula(cursoId, alunoId, valor, dataMatricula);
        act.Should().Throw<DomainException>().WithMessage("*CursoId*");
    }

    [Fact]
    public void Matricula_DeveLancarExcecao_QuandoAlunoIdVazio()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var alunoId = Guid.Empty;
        var valor = 1500m;
        var dataMatricula = DateTime.UtcNow;

        // Act & Assert
        Action act = () => new Matricula(cursoId, alunoId, valor, dataMatricula);
        act.Should().Throw<DomainException>().WithMessage("*AlunoId*");
    }

    [Fact]
    public void Matricula_DeveLancarExcecao_QuandoValorNegativo()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var valor = -100m;
        var dataMatricula = DateTime.UtcNow;

        // Act & Assert
        Action act = () => new Matricula(cursoId, alunoId, valor, dataMatricula);
        act.Should().Throw<DomainException>().WithMessage("*inválido*");
    }

    [Fact]
    public void Matricula_DeveAtivar_ComSucesso()
    {
        // Arrange
        var matricula = CriarMatriculaValida();

        // Act
        matricula.Ativar();

        // Assert
        matricula.Status.Should().Be(StatusMatriculaEnum.Ativa);
    }

    [Fact]
    public void Matricula_DeveConcluir_QuandoAtiva()
    {
        // Arrange
        var matricula = CriarMatriculaValida();
        matricula.Ativar();

        // Act
        matricula.Concluir();

        // Assert
        matricula.Status.Should().Be(StatusMatriculaEnum.Concluida);
    }

    [Fact]
    public void Matricula_DeveLancarExcecao_AoConcluirQuandoNaoAtiva()
    {
        // Arrange
        var matricula = CriarMatriculaValida();

        // Act & Assert
        Action act = () => matricula.Concluir();
        act.Should().Throw<DomainException>().WithMessage("*ativas*");
    }

    [Fact]
    public void Matricula_DeveCancelar_ComSucesso()
    {
        // Arrange
        var matricula = CriarMatriculaValida();

        // Act
        matricula.Cancelar();

        // Assert
        matricula.Status.Should().Be(StatusMatriculaEnum.Cancelada);
    }

    [Fact]
    public void Matricula_DeveLancarExcecao_AoCancelarMatriculaConcluida()
    {
        // Arrange
        var matricula = CriarMatriculaValida();
        matricula.Ativar();
        matricula.Concluir();

        // Act & Assert
        Action act = () => matricula.Cancelar();
        act.Should().Throw<DomainException>().WithMessage("*concluída*");
    }

    [Fact]
    public void Matricula_DeveRegistrarProgresso_ComSucesso()
    {
        // Arrange
        var matricula = CriarMatriculaValida();
        var aulaId = Guid.NewGuid();
        var percentual = 50.0;

        // Act
        matricula.RegistrarProgresso(aulaId, percentual);

        // Assert
        matricula.Historico.Should().HaveCount(1);
        matricula.Historico.First().CursoId.Should().Be(aulaId);
        matricula.Historico.First().ProgressoPercentual.Should().Be(percentual);
    }

    [Fact]
    public void Matricula_DeveAtualizarProgressoExistente()
    {
        // Arrange
        var matricula = CriarMatriculaValida();
        var aulaId = Guid.NewGuid();
        matricula.RegistrarProgresso(aulaId, 50.0);

        // Act
        matricula.RegistrarProgresso(aulaId, 100.0);

        // Assert
        matricula.Historico.Should().HaveCount(1);
        matricula.Historico.First().ProgressoPercentual.Should().Be(100.0);
    }

    [Fact]
    public void Matricula_PodeFinalizar_DeveRetornarFalse_QuandoNaoAtiva()
    {
        // Arrange
        var matricula = CriarMatriculaValida();

        // Act
        var podeFinalizar = matricula.PodeFinalizar();

        // Assert
        podeFinalizar.Should().BeFalse();
    }

    [Fact]
    public void Matricula_PodeFinalizar_DeveRetornarTrue_QuandoTodasAulasCompletas()
    {
        // Arrange
        var matricula = CriarMatriculaValida();
        matricula.Ativar();
        matricula.RegistrarProgresso(Guid.NewGuid(), 100.0);
        matricula.RegistrarProgresso(Guid.NewGuid(), 100.0);

        // Act
        var podeFinalizar = matricula.PodeFinalizar();

        // Assert
        podeFinalizar.Should().BeTrue();
    }

    private Matricula CriarMatriculaValida()
    {
        return new Matricula(
            Guid.NewGuid(),
            Guid.NewGuid(),
            1500m,
            DateTime.UtcNow
        );
    }
}
