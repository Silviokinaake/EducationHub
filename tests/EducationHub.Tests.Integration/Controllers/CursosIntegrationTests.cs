using EducationHub.Tests.Integration.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EducationHub.Tests.Integration.Controllers;

public class CursosIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CursosIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ObterTodosCursos_DeveRetornarOk()
    {
        // Act
        var response = await _client.GetAsync("/api/cursos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ObterCursoPorId_ComIdInvalido_DeveRetornarNotFound()
    {
        // Arrange
        var cursoId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/cursos/{cursoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
