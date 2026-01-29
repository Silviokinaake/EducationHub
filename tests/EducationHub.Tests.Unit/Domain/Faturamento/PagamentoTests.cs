using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Faturamento.Domain.Enums;
using EducationHub.Core.DomainObjects;
using FluentAssertions;

namespace EducationHub.Tests.Unit.Domain.Faturamento;

public class PagamentoTests
{
    [Fact]
    public void Pagamento_DeveCriarPagamento_ComDadosValidos()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var preMatriculaId = Guid.NewGuid();
        var valor = 199.99m;
        var dadosCartao = new DadosCartao("João Silva", "4532015112830366", "12/28", "123");

        // Act
        var pagamento = new Pagamento(alunoId, preMatriculaId, valor, dadosCartao);

        // Assert
        pagamento.Should().NotBeNull();
        pagamento.AlunoId.Should().Be(alunoId);
        pagamento.PreMatriculaId.Should().Be(preMatriculaId);
        pagamento.Valor.Should().Be(valor);
        pagamento.Status.Should().Be(StatusPagamentoEnum.Pendente);
        pagamento.DataPagamento.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Pagamento_DeveLancarExcecao_QuandoAlunoIdVazio()
    {
        // Arrange
        var alunoId = Guid.Empty;
        var preMatriculaId = Guid.NewGuid();
        var valor = 199.99m;
        var dadosCartao = new DadosCartao("João Silva", "4532015112830366", "12/28", "123");

        // Act & Assert
        Action act = () => new Pagamento(alunoId, preMatriculaId, valor, dadosCartao);
        act.Should().Throw<DomainException>().WithMessage("*AlunoId*");
    }

    [Fact]
    public void Pagamento_DeveLancarExcecao_QuandoPreMatriculaIdVazio()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var preMatriculaId = Guid.Empty;
        var valor = 199.99m;
        var dadosCartao = new DadosCartao("João Silva", "4532015112830366", "12/28", "123");

        // Act & Assert
        Action act = () => new Pagamento(alunoId, preMatriculaId, valor, dadosCartao);
        act.Should().Throw<DomainException>().WithMessage("*PreMatriculaId*");
    }

    [Fact]
    public void Pagamento_DeveLancarExcecao_QuandoValorNegativo()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var preMatriculaId = Guid.NewGuid();
        var valor = -10m;
        var dadosCartao = new DadosCartao("João Silva", "4532015112830366", "12/28", "123");

        // Act & Assert
        Action act = () => new Pagamento(alunoId, preMatriculaId, valor, dadosCartao);
        act.Should().Throw<DomainException>().WithMessage("*negativo*");
    }

    [Fact]
    public void Pagamento_DeveLancarExcecao_QuandoDadosCartaoNulo()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var preMatriculaId = Guid.NewGuid();
        var valor = 199.99m;

        // Act & Assert
        Action act = () => new Pagamento(alunoId, preMatriculaId, valor, null!);
        act.Should().Throw<DomainException>().WithMessage("*dados do cartão*");
    }

    [Fact]
    public void Pagamento_DeveConfirmar_ComSucesso()
    {
        // Arrange
        var pagamento = CriarPagamentoValido();

        // Act
        pagamento.Confirmar();

        // Assert
        pagamento.Status.Should().Be(StatusPagamentoEnum.Confirmado);
        pagamento.DataPagamento.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Pagamento_DeveLancarExcecao_AoConfirmarPagamentoJaConfirmado()
    {
        // Arrange
        var pagamento = CriarPagamentoValido();
        pagamento.Confirmar();

        // Act & Assert
        Action act = () => pagamento.Confirmar();
        act.Should().Throw<DomainException>().WithMessage("*já foi confirmado*");
    }

    [Fact]
    public void Pagamento_DeveRejeitar_ComSucesso()
    {
        // Arrange
        var pagamento = CriarPagamentoValido();
        var motivo = "Saldo insuficiente";

        // Act
        pagamento.Rejeitar(motivo);

        // Assert
        pagamento.Status.Should().Be(StatusPagamentoEnum.Rejeitado);
    }

    [Fact]
    public void Pagamento_DeveLancarExcecao_AoRejeitarPagamentoJaRejeitado()
    {
        // Arrange
        var pagamento = CriarPagamentoValido();
        pagamento.Rejeitar("Saldo insuficiente");

        // Act & Assert
        Action act = () => pagamento.Rejeitar("Outro motivo");
        act.Should().Throw<DomainException>().WithMessage("*já foi rejeitado*");
    }

    [Fact]
    public void Pagamento_DeveAplicarTokenCartao_ComSucesso()
    {
        // Arrange
        var pagamento = CriarPagamentoValido();
        var token = "tok_abc123456";
        var numeroMascarado = "****1234";

        // Act
        pagamento.AplicarTokenCartao(token, numeroMascarado);

        // Assert
        pagamento.TokenCartao.Should().Be(token);
        pagamento.NumeroCartaoMascarado.Should().Be(numeroMascarado);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Pagamento_DeveLancarExcecao_QuandoTokenInvalido(string tokenInvalido)
    {
        // Arrange
        var pagamento = CriarPagamentoValido();

        // Act & Assert
        Action act = () => pagamento.AplicarTokenCartao(tokenInvalido, "****1234");
        act.Should().Throw<DomainException>().WithMessage("*Token*");
    }

    private Pagamento CriarPagamentoValido()
    {
        return new Pagamento(
            Guid.NewGuid(),
            Guid.NewGuid(),
            199.99m,
            new DadosCartao("João Silva", "4532015112830366", "12/28", "123")
        );
    }
}
