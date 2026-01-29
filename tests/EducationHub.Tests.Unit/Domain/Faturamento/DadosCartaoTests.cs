using EducationHub.Faturamento.Domain.Entidades;
using EducationHub.Core.DomainObjects;
using FluentAssertions;

namespace EducationHub.Tests.Unit.Domain.Faturamento;

public class DadosCartaoTests
{
    [Fact]
    public void DadosCartao_DeveCriarDadosCartao_ComDadosValidos()
    {
        // Arrange
        var nomeTitular = "João Silva";
        var numero = "4532015112830366";
        var validade = "12/28";
        var cvv = "123";

        // Act
        var dadosCartao = new DadosCartao(nomeTitular, numero, validade, cvv);

        // Assert
        dadosCartao.Should().NotBeNull();
        dadosCartao.NomeTitular.Should().Be(nomeTitular);
        dadosCartao.Numero.Should().Be(numero);
        dadosCartao.Validade.Should().Be(validade);
        dadosCartao.Cvv.Should().Be(cvv);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Jo")]
    public void DadosCartao_DeveLancarExcecao_QuandoNomeTitularInvalido(string nomeTitularInvalido)
    {
        // Arrange
        var numero = "4532015112830366";
        var validade = "12/28";
        var cvv = "123";

        // Act & Assert
        Action act = () => new DadosCartao(nomeTitularInvalido, numero, validade, cvv);
        act.Should().Throw<DomainException>().WithMessage("*nome do titular*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123")]
    [InlineData("12345678901234567890")] // Mais de 19 dígitos
    public void DadosCartao_DeveLancarExcecao_QuandoNumeroCartaoInvalido(string numeroInvalido)
    {
        // Arrange
        var nomeTitular = "João Silva";
        var validade = "12/28";
        var cvv = "123";

        // Act & Assert
        Action act = () => new DadosCartao(nomeTitular, numeroInvalido, validade, cvv);
        act.Should().Throw<DomainException>().WithMessage("*número do cartão*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("13/28")] // Mês inválido
    [InlineData("00/28")] // Mês zero
    [InlineData("12/2028")] // Formato errado
    [InlineData("12-28")] // Separador errado
    public void DadosCartao_DeveLancarExcecao_QuandoValidadeInvalida(string validadeInvalida)
    {
        // Arrange
        var nomeTitular = "João Silva";
        var numero = "4532015112830366";
        var cvv = "123";

        // Act & Assert
        Action act = () => new DadosCartao(nomeTitular, numero, validadeInvalida, cvv);
        act.Should().Throw<DomainException>().WithMessage("*validade*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12")]
    [InlineData("12345")] // Mais de 4 dígitos
    public void DadosCartao_DeveLancarExcecao_QuandoCvvInvalido(string cvvInvalido)
    {
        // Arrange
        var nomeTitular = "João Silva";
        var numero = "4532015112830366";
        var validade = "12/28";

        // Act & Assert
        Action act = () => new DadosCartao(nomeTitular, numero, validade, cvvInvalido);
        act.Should().Throw<DomainException>().WithMessage("*CVV*");
    }

    [Fact]
    public void DadosCartao_DeveAceitarCvvDe3Digitos()
    {
        // Arrange
        var nomeTitular = "João Silva";
        var numero = "4532015112830366";
        var validade = "12/28";
        var cvv = "123";

        // Act
        var dadosCartao = new DadosCartao(nomeTitular, numero, validade, cvv);

        // Assert
        dadosCartao.Cvv.Should().Be(cvv);
    }

    [Fact]
    public void DadosCartao_DeveAceitarCvvDe4Digitos()
    {
        // Arrange
        var nomeTitular = "João Silva";
        var numero = "371449635398431"; // American Express
        var validade = "12/28";
        var cvv = "1234";

        // Act
        var dadosCartao = new DadosCartao(nomeTitular, numero, validade, cvv);

        // Assert
        dadosCartao.Cvv.Should().Be(cvv);
    }
}
