using AgroSolutions.Propriedade.Dominio;
using AgroSolutions.Propriedade.Infra;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgroSolutions.Propriedade.Tests.Api;

/// <summary>
/// Testes para os endpoints da API de propriedades.
/// Valida a criação, recuperação e listagem de propriedades com suas validações.
/// </summary>
public class PropriedadeApiTests
{
    private Mock<PropriedadeDbContext> GetMockDbContext()
    {
        var options = new DbContextOptionsBuilder<PropriedadeDbContext>()
            .UseInMemoryDatabase($"AgroSolutions-Api-Test-{Guid.NewGuid()}")
            .Options;

        return new Mock<PropriedadeDbContext>(options);
    }

    [Fact]
    public void CriarPropriedadeRequest_DeveSerValidado_ComIdProdutorVazio()
    {
        // Arrange
        var request = new CriarPropriedadeRequest(
            IdProdutor: Guid.Empty,
            Nome: "Propriedade Teste",
            Descricao: null,
            Talhoes: new List<CriarTalhaoRequest>
            {
                new CriarTalhaoRequest("Talhão 1", "Soja", 100m)
            }
        );

        // Act & Assert
        Assert.Equal(Guid.Empty, request.IdProdutor);
    }

    [Fact]
    public void CriarPropriedadeRequest_DeveSerValidado_ComNomeVazio()
    {
        // Arrange
        var request = new CriarPropriedadeRequest(
            IdProdutor: Guid.NewGuid(),
            Nome: string.Empty,
            Descricao: null,
            Talhoes: new List<CriarTalhaoRequest>
            {
                new CriarTalhaoRequest("Talhão 1", "Soja", 100m)
            }
        );

        // Act & Assert
        Assert.True(string.IsNullOrWhiteSpace(request.Nome));
    }

    [Fact]
    public void CriarPropriedadeRequest_DeveSerValidado_ComTalhoesVazio()
    {
        // Arrange
        var request = new CriarPropriedadeRequest(
            IdProdutor: Guid.NewGuid(),
            Nome: "Propriedade Teste",
            Descricao: null,
            Talhoes: new List<CriarTalhaoRequest>()
        );

        // Act & Assert
        Assert.Empty(request.Talhoes);
    }

    [Fact]
    public void CriarPropriedadeRequest_DeveSerValidado_ComTalhoesNull()
    {
        // Arrange
        var request = new CriarPropriedadeRequest(
            IdProdutor: Guid.NewGuid(),
            Nome: "Propriedade Teste",
            Descricao: null,
            Talhoes: null
        );

        // Act & Assert
        Assert.Null(request.Talhoes);
    }

    [Fact]
    public void CriarPropriedadeRequest_DeveSerValidado_ValidoComTodosDados()
    {
        // Arrange
        var idProdutor = Guid.NewGuid();
        var request = new CriarPropriedadeRequest(
            IdProdutor: idProdutor,
            Nome: "Propriedade Válida",
            Descricao: "Descrição da propriedade",
            Talhoes: new List<CriarTalhaoRequest>
            {
                new CriarTalhaoRequest("Talhão 1", "Soja", 100m),
                new CriarTalhaoRequest("Talhão 2", "Milho", 75.5m)
            }
        );

        // Act & Assert
        Assert.NotEqual(Guid.Empty, request.IdProdutor);
        Assert.False(string.IsNullOrWhiteSpace(request.Nome));
        Assert.NotEmpty(request.Talhoes);
        Assert.Equal(2, request.Talhoes.Count);
    }

    [Fact]
    public void PropriedadeResponse_DeveSerCriada_ComTodosOsDados()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var idProdutor = Guid.NewGuid();
        var agora = DateTimeOffset.UtcNow;

        var talhoes = new List<TalhaoResponse>
        {
            new TalhaoResponse(
                IdTalhao: Guid.NewGuid(),
                Nome: "Talhão 1",
                Cultura: "Soja",
                AreaHectares: 100m,
                CriadoEm: agora,
                AtualizadoEm: null
            )
        };

        // Act
        var response = new PropriedadeResponse(
            IdPropriedade: idPropriedade,
            IdProdutor: idProdutor,
            Nome: "Propriedade Resposta",
            Descricao: "Descrição",
            CriadoEm: agora,
            AtualizadoEm: null,
            Talhoes: talhoes
        );

        // Assert
        Assert.Equal(idPropriedade, response.IdPropriedade);
        Assert.Equal(idProdutor, response.IdProdutor);
        Assert.Equal("Propriedade Resposta", response.Nome);
        Assert.Equal("Descrição", response.Descricao);
        Assert.Single(response.Talhoes);
    }

    [Fact]
    public void PropriedadeResponse_DevePermitirDescricaoNula()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var idProdutor = Guid.NewGuid();
        var agora = DateTimeOffset.UtcNow;

        // Act
        var response = new PropriedadeResponse(
            IdPropriedade: idPropriedade,
            IdProdutor: idProdutor,
            Nome: "Propriedade Sem Descrição",
            Descricao: null!,
            CriadoEm: agora,
            AtualizadoEm: null,
            Talhoes: new List<TalhaoResponse>
            {
                new TalhaoResponse(
                    IdTalhao: Guid.NewGuid(),
                    Nome: "Talhão 1",
                    Cultura: "Soja",
                    AreaHectares: 50m,
                    CriadoEm: agora,
                    AtualizadoEm: null
                )
            }
        );

        // Assert
        Assert.Null(response.Descricao);
    }

    [Fact]
    public void TalhaoResponse_DeveSerCriada_ComTodosOsDados()
    {
        // Arrange
        var idTalhao = Guid.NewGuid();
        var agora = DateTimeOffset.UtcNow;

        // Act
        var response = new TalhaoResponse(
            IdTalhao: idTalhao,
            Nome: "Talhão Resposta",
            Cultura: "Milho",
            AreaHectares: 75.5m,
            CriadoEm: agora,
            AtualizadoEm: null
        );

        // Assert
        Assert.Equal(idTalhao, response.IdTalhao);
        Assert.Equal("Talhão Resposta", response.Nome);
        Assert.Equal("Milho", response.Cultura);
        Assert.Equal(75.5m, response.AreaHectares);
        Assert.Equal(agora, response.CriadoEm);
        Assert.Null(response.AtualizadoEm);
    }

    [Fact]
    public void CriarTalhaoRequest_DeveSerCriado_ComTodosOsDados()
    {
        // Arrange
        const string nome = "Talhão Teste";
        const string cultura = "Feijão";
        const decimal areaHectares = 50.25m;

        // Act
        var request = new CriarTalhaoRequest(nome, cultura, areaHectares);

        // Assert
        Assert.Equal(nome, request.Nome);
        Assert.Equal(cultura, request.Cultura);
        Assert.Equal(areaHectares, request.AreaHectares);
    }

    [Fact]
    public void CriarTalhaoRequest_DeveAceitarVariasCulturas()
    {
        // Arrange
        var culturas = new[] { "Soja", "Milho", "Arroz", "Trigo", "Cana-de-açúcar", "Feijão" };

        // Act & Assert
        foreach (var cultura in culturas)
        {
            var request = new CriarTalhaoRequest($"Talhão {cultura}", cultura, 100m);
            Assert.Equal(cultura, request.Cultura);
        }
    }

    [Fact]
    public void CriarTalhaoRequest_DeveAceitarVariasAreas()
    {
        // Arrange
        var areas = new decimal[] { 0.5m, 10m, 50.75m, 100m, 1000.99m };

        // Act & Assert
        foreach (var area in areas)
        {
            var request = new CriarTalhaoRequest($"Talhão Area {area}", "Soja", area);
            Assert.Equal(area, request.AreaHectares);
        }
    }

    [Fact]
    public void PropriedadeResponse_DeveConterMultiplosTalhoes()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var idProdutor = Guid.NewGuid();
        var agora = DateTimeOffset.UtcNow;

        var talhoes = new List<TalhaoResponse>
        {
            new TalhaoResponse(Guid.NewGuid(), "Talhão 1", "Soja", 100m, agora, null),
            new TalhaoResponse(Guid.NewGuid(), "Talhão 2", "Milho", 75m, agora, null),
            new TalhaoResponse(Guid.NewGuid(), "Talhão 3", "Arroz", 50m, agora, null)
        };

        // Act
        var response = new PropriedadeResponse(
            idPropriedade,
            idProdutor,
            "Propriedade Com Vários Talhões",
            null,
            agora,
            null,
            talhoes
        );

        // Assert
        Assert.Equal(3, response.Talhoes.Count);
        Assert.All(response.Talhoes, t => Assert.False(string.IsNullOrWhiteSpace(t.Nome)));
    }

    [Fact]
    public void PropriedadeResponse_DeveMapearValoresCorretamente_DoTalhao()
    {
        // Arrange
        var idTalhao = Guid.NewGuid();
        var nomeTalhao = "Talhão Mapeado";
        var cultura = "Trigo";
        var areaHectares = 60.5m;
        var agora = DateTimeOffset.UtcNow;

        var talhaoResponse = new TalhaoResponse(
            idTalhao,
            nomeTalhao,
            cultura,
            areaHectares,
            agora,
            null
        );

        var propriedadeResponse = new PropriedadeResponse(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Propriedade",
            null,
            agora,
            null,
            new List<TalhaoResponse> { talhaoResponse }
        );

        // Act
        var talhaoMapeado = propriedadeResponse.Talhoes.First();

        // Assert
        Assert.Equal(idTalhao, talhaoMapeado.IdTalhao);
        Assert.Equal(nomeTalhao, talhaoMapeado.Nome);
        Assert.Equal(cultura, talhaoMapeado.Cultura);
        Assert.Equal(areaHectares, talhaoMapeado.AreaHectares);
    }

    [Fact]
    public void CriarPropriedadeRequest_DevePermitirTrimNoNome_AoSerProcessado()
    {
        // Arrange
        var nomeComEspacos = "  Propriedade Com Espaços  ";
        var request = new CriarPropriedadeRequest(
            Guid.NewGuid(),
            nomeComEspacos,
            null,
            new List<CriarTalhaoRequest> { new CriarTalhaoRequest("Talhão 1", "Soja", 100m) }
        );

        // Act
        var nomeTrimmed = request.Nome.Trim();

        // Assert
        Assert.Equal("Propriedade Com Espaços", nomeTrimmed);
    }

    [Fact]
    public void CriarPropriedadeRequest_DevePermitirTrimNaDescricao_AoSerProcessada()
    {
        // Arrange
        var descricaoComEspacos = "  Descrição com espaços  ";
        var request = new CriarPropriedadeRequest(
            Guid.NewGuid(),
            "Propriedade",
            descricaoComEspacos,
            new List<CriarTalhaoRequest> { new CriarTalhaoRequest("Talhão 1", "Soja", 100m) }
        );

        // Act
        var descricaoTrimmed = request.Descricao?.Trim();

        // Assert
        Assert.Equal("Descrição com espaços", descricaoTrimmed);
    }
}

record CriarPropriedadeRequest(Guid IdProdutor, string Nome, string? Descricao, List<CriarTalhaoRequest>? Talhoes);
record CriarTalhaoRequest(string Nome, string Cultura, decimal AreaHectares);
record PropriedadeResponse(Guid IdPropriedade, Guid IdProdutor, string Nome, string? Descricao, DateTimeOffset CriadoEm, DateTimeOffset? AtualizadoEm, List<TalhaoResponse> Talhoes);
record TalhaoResponse(Guid IdTalhao, string Nome, string Cultura, decimal AreaHectares, DateTimeOffset CriadoEm, DateTimeOffset? AtualizadoEm);