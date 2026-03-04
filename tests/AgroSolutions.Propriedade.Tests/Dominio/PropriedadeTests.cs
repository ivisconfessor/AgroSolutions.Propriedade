using AgroSolutions.Propriedade.Dominio;

namespace AgroSolutions.Propriedade.Tests.Dominio;

public class PropriedadeTests
{
    [Fact]
    public void Propriedade_DeveSerCriada_ComTodosOsDados()
    {
        var id = Guid.NewGuid();
        var idProdutor = Guid.NewGuid();
        var agora = DateTimeOffset.UtcNow;

        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = id,
            IdProdutor = idProdutor,
            Nome = "Propriedade Teste",
            Descricao = "Descrição",
            CriadoEm = agora
        };

        Assert.Equal(id, propriedade.IdPropriedade);
        Assert.Equal(idProdutor, propriedade.IdProdutor);
        Assert.Equal("Propriedade Teste", propriedade.Nome);
        Assert.Equal("Descrição", propriedade.Descricao);
        Assert.Equal(agora, propriedade.CriadoEm);
        Assert.Null(propriedade.AtualizadoEm);
        Assert.Empty(propriedade.Talhoes);
    }

    [Fact]
    public void Propriedade_DevePermitirDescricaoNula()
    {
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade",
            Descricao = null,
            CriadoEm = DateTimeOffset.UtcNow
        };

        Assert.Null(propriedade.Descricao);
    }

    [Fact]
    public void Propriedade_DeveSerAtualizavel()
    {
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade",
            CriadoEm = DateTimeOffset.UtcNow
        };
        var dataAtualizacao = DateTimeOffset.UtcNow.AddDays(1);

        propriedade.AtualizadoEm = dataAtualizacao;

        Assert.NotNull(propriedade.AtualizadoEm);
        Assert.Equal(dataAtualizacao, propriedade.AtualizadoEm);
    }

    [Fact]
    public void Propriedade_DeveAdicionarTalhoes()
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
            Nome = "Talhão 1",
            Cultura = "Soja",
            AreaHectares = 100m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        propriedade.Talhoes.Add(talhao);

        Assert.Single(propriedade.Talhoes);
        Assert.Equal(talhao.IdTalhao, propriedade.Talhoes.First().IdTalhao);
    }

    [Fact]
    public void Propriedade_DeveAdicionarMultiplosTalhoes()
    {
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade",
            CriadoEm = DateTimeOffset.UtcNow
        };

        for (int i = 0; i < 3; i++)
        {
            propriedade.Talhoes.Add(new Talhao
            {
                IdTalhao = Guid.NewGuid(),
                IdPropriedade = propriedade.IdPropriedade,
                Nome = $"Talhão {i + 1}",
                Cultura = "Soja",
                AreaHectares = 50 + i,
                CriadoEm = DateTimeOffset.UtcNow
            });
        }

        Assert.Equal(3, propriedade.Talhoes.Count);
    }

    [Fact]
    public void Propriedade_DeveRemoverTalhao()
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
            Cultura = "Milho",
            AreaHectares = 75m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        propriedade.Talhoes.Add(talhao);
        propriedade.Talhoes.Remove(talhao);

        Assert.Empty(propriedade.Talhoes);
    }

    [Theory]
    [InlineData("Soja")]
    [InlineData("Milho")]
    [InlineData("Arroz")]
    public void Propriedade_DeveAceitarNomeValido(string nome)
    {
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = nome,
            CriadoEm = DateTimeOffset.UtcNow
        };

        Assert.Equal(nome, propriedade.Nome);
    }
}
