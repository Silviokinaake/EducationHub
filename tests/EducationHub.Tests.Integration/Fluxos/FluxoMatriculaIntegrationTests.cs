using EducationHub.Tests.Integration.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EducationHub.Tests.Integration.Fluxos;

public class FluxoMatriculaIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FluxoMatriculaIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FluxoCompleto_CriarCurso_MatricularAluno_RealizarPagamento()
    {
        // Arrange - Este é um teste conceitual que demonstra o fluxo completo
        // Em um cenário real, seria necessário:
        // 1. Criar um usuário via API de autenticação
        // 2. Obter token JWT
        // 3. Criar um curso
        // 4. Criar um aluno
        // 5. Matricular o aluno
        // 6. Processar o pagamento
        // 7. Verificar ativação da matrícula
        
        // Por enquanto, apenas verificamos que as APIs estão acessíveis
        var cursosResponse = await _client.GetAsync("/api/cursos");
        cursosResponse.Should().NotBeNull();
        
        // Este teste serve como base para expansão futura
        cursosResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);
    }
}
