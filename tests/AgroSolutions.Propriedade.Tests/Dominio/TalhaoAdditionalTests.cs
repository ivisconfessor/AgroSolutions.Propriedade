using AgroSolutions.Propriedade.Dominio;

namespace AgroSolutions.Propriedade.Tests.Dominio;

public class TalhaoAdditionalTests
{
    [Fact]
    public void Talhao_DeveSerCriado_ComTodosOsDados()
    {
        var idTalhao = Guid.NewGuid();
        var idPropriedade = Guid.NewGuid();
        var agora = DateTimeOffset.UtcNow;

        var talhao = new Talhao
        {
            IdTalhao = idTalhao,
            IdPropriedade = idPropriedade,
            Nome = "Talhão 1",
            Cultura = "Soja",
            AreaHectares = 100.5m,
            CriadoEm = agora
        };

        Assert.Equal(idTalhao, talhao.IdTalhao);
        Assert.Equal(idPropriedade, talhao.IdPropriedade);
        Assert.Equal("Talhão 1", talhao.Nome);
        Assert.Equal("Soja", talhao.Cultura);
        Assert.Equal(100.5m, talhao.AreaHectares);
        Assert.Equal(agora, talhao.CriadoEm);
        Assert.Null(talhao.AtualizadoEm);
    }

    [Theory]
    [InlineData("Soja")]
    [InlineData("Milho")]
    [InlineData("Arroz")]
    [InlineData("Trigo")]
    public void Talhao_DeveAceitarVariasCulturasEAreas(string cultura)
    {
        var areas = new decimal[] { 50m, 75.5m, 100m, 25.25m };
        var area = areas[new Random().Next(areas.Length)];

        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = Guid.NewGuid(),
            Nome = $"Talhão {cultura}",
            Cultura = cultura,
            AreaHectares = area,
            CriadoEm = DateTimeOffset.UtcNow
        };

        Assert.Equal(cultura, talhao.Cultura);
        Assert.True(talhao.AreaHectares > 0);
    }

    [Fact]
    public void Talhao_DeveSerAtualizavel()
    {
        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = Guid.NewGuid(),
            Nome = "Talhão",
            Cultura = "Soja",
            AreaHectares = 50m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        talhao.Cultura = "Milho";
        talhao.AreaHectares = 75m;
        talhao.AtualizadoEm = DateTimeOffset.UtcNow;

        Assert.Equal("Milho", talhao.Cultura);
        Assert.Equal(75m, talhao.AreaHectares);
        Assert.NotNull(talhao.AtualizadoEm);
    }

    [Fact]
    public void Talhao_DeveRelacionarComPropriedade()
    {
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade",
            CriadoEm = DateTimeOffset.UtcNow
        };

        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = propriedade.IdPropriedade,
            Nome = "Talhão",
            Cultura = "Soja",
            AreaHectares = 50m,
            CriadoEm = DateTimeOffset.UtcNow,
            Propriedade = propriedade
        };

        Assert.NotNull(talhao.Propriedade);
        Assert.Equal(propriedade.IdPropriedade, talhao.Propriedade.IdPropriedade);
    }

    [Fact]
    public void Talhao_DeveManterReferenciaPropriedade()
    {
        var idPropriedade = Guid.NewGuid();
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = idPropriedade,
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade",
            CriadoEm = DateTimeOffset.UtcNow
        };

        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = idPropriedade,
            Nome = "Talhão",
            Cultura = "Arroz",
            AreaHectares = 30m,
            CriadoEm = DateTimeOffset.UtcNow,
            Propriedade = propriedade
        };

        Assert.Equal(talhao.IdPropriedade, talhao.Propriedade.IdPropriedade);
    }

    [Fact]
    public void Talhao_DevePermitirPropriedadeNula()
    {
        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = Guid.NewGuid(),
            Nome = "Talhão",
            Cultura = "Trigo",
            AreaHectares = 40m,
            CriadoEm = DateTimeOffset.UtcNow,
            Propriedade = null
        };

        Assert.Null(talhao.Propriedade);
    }
}
