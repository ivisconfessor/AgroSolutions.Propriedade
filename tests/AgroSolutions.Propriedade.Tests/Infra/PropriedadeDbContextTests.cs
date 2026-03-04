using AgroSolutions.Propriedade.Dominio;
using AgroSolutions.Propriedade.Tests.Fixtures;
using AgroSolutions.Propriedade.Infra;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Propriedade.Tests.Infra;

/// <summary>
/// Testes para o contexto de banco de dados PropriedadeDbContext.
/// Valida as operações CRUD de propriedades e talhões.
/// </summary>
public class PropriedadeDbContextTests : IDisposable
{
    private readonly DatabaseFixture _fixture;

    public PropriedadeDbContextTests()
    {
        _fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        _fixture?.Dispose();
    }

    [Fact]
    public async Task DbContext_DeveAdicionarPropriedadeComSucesso()
    {
        // Arrange
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Teste DbContext",
            Descricao = "Descrição de teste",
            CriadoEm = DateTimeOffset.UtcNow
        };

        // Act
        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var propriedadeNoDb = await _fixture.Context.Propriedades
            .FirstOrDefaultAsync(p => p.IdPropriedade == propriedade.IdPropriedade);

        Assert.NotNull(propriedadeNoDb);
        Assert.Equal(propriedade.Nome, propriedadeNoDb.Nome);
    }

    [Fact]
    public async Task DbContext_DeveRecuperarPropriedadePorId()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = idPropriedade,
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Para Recuperar",
            CriadoEm = DateTimeOffset.UtcNow
        };

        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var propriedadeRecuperada = await _fixture.Context.Propriedades
            .FirstOrDefaultAsync(p => p.IdPropriedade == idPropriedade);

        // Assert
        Assert.NotNull(propriedadeRecuperada);
        Assert.Equal(idPropriedade, propriedadeRecuperada.IdPropriedade);
    }

    [Fact]
    public async Task DbContext_DeveAtualizarPropriedadeComSucesso()
    {
        // Arrange
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Original",
            CriadoEm = DateTimeOffset.UtcNow
        };

        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Act
        propriedade.Nome = "Propriedade Atualizada";
        propriedade.AtualizadoEm = DateTimeOffset.UtcNow;
        _fixture.Context.Propriedades.Update(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var propriedadeAtualizada = await _fixture.Context.Propriedades
            .FirstOrDefaultAsync(p => p.IdPropriedade == propriedade.IdPropriedade);

        Assert.NotNull(propriedadeAtualizada);
        Assert.Equal("Propriedade Atualizada", propriedadeAtualizada.Nome);
        Assert.NotNull(propriedadeAtualizada.AtualizadoEm);
    }

    [Fact]
    public async Task DbContext_DeveRemoverPropriedadeComSucesso()
    {
        // Arrange
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = Guid.NewGuid(),
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Para Remover",
            CriadoEm = DateTimeOffset.UtcNow
        };

        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Act
        _fixture.Context.Propriedades.Remove(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var propriedadeRemovida = await _fixture.Context.Propriedades
            .FirstOrDefaultAsync(p => p.IdPropriedade == propriedade.IdPropriedade);

        Assert.Null(propriedadeRemovida);
    }

    [Fact]
    public async Task DbContext_DeveAdicionarTalhaoComSucesso()
    {
        // Arrange
        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = Guid.NewGuid(),
            Nome = "Talhão Teste DbContext",
            Cultura = "Soja",
            AreaHectares = 100m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        // Act
        await _fixture.Context.Talhoes.AddAsync(talhao);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var talhaoNoDb = await _fixture.Context.Talhoes
            .FirstOrDefaultAsync(t => t.IdTalhao == talhao.IdTalhao);

        Assert.NotNull(talhaoNoDb);
        Assert.Equal(talhao.Cultura, talhaoNoDb.Cultura);
        Assert.Equal(talhao.AreaHectares, talhaoNoDb.AreaHectares);
    }

    [Fact]
    public async Task DbContext_DeveCarregarPropriedadeComTalhoes()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = idPropriedade,
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Com Talhões",
            CriadoEm = DateTimeOffset.UtcNow,
            Talhoes = new List<Talhao>
            {
                new Talhao
                {
                    IdTalhao = Guid.NewGuid(),
                    IdPropriedade = idPropriedade,
                    Nome = "Talhão 1",
                    Cultura = "Milho",
                    AreaHectares = 50m,
                    CriadoEm = DateTimeOffset.UtcNow
                },
                new Talhao
                {
                    IdTalhao = Guid.NewGuid(),
                    IdPropriedade = idPropriedade,
                    Nome = "Talhão 2",
                    Cultura = "Soja",
                    AreaHectares = 75m,
                    CriadoEm = DateTimeOffset.UtcNow
                }
            }
        };

        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var propriedadeCarregada = await _fixture.Context.Propriedades
            .Include(p => p.Talhoes)
            .FirstOrDefaultAsync(p => p.IdPropriedade == idPropriedade);

        // Assert
        Assert.NotNull(propriedadeCarregada);
        Assert.Equal(2, propriedadeCarregada.Talhoes.Count);
    }

    [Fact]
    public async Task DbContext_DeveCarregarTalhaoComPropriedadeRelacionada()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = idPropriedade,
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Para Relacionamento",
            CriadoEm = DateTimeOffset.UtcNow
        };

        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = idPropriedade,
            Nome = "Talhão Relacionado",
            Cultura = "Arroz",
            AreaHectares = 60m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.Talhoes.AddAsync(talhao);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var talhaoCarregado = await _fixture.Context.Talhoes
            .Include(t => t.Propriedade)
            .FirstOrDefaultAsync(t => t.IdTalhao == talhao.IdTalhao);

        // Assert
        Assert.NotNull(talhaoCarregado);
        Assert.NotNull(talhaoCarregado.Propriedade);
        Assert.Equal(idPropriedade, talhaoCarregado.Propriedade.IdPropriedade);
    }

    [Fact]
    public async Task DbContext_DeveRemoverTalhoesAoRemoverPropriedade()
    {
        // Arrange
        var idPropriedade = Guid.NewGuid();
        var propriedade = new AgroSolutions.Propriedade.Dominio.Propriedade
        {
            IdPropriedade = idPropriedade,
            IdProdutor = Guid.NewGuid(),
            Nome = "Propriedade Com Exclusão em Cascata",
            CriadoEm = DateTimeOffset.UtcNow,
            Talhoes = new List<Talhao>
            {
                new Talhao
                {
                    IdTalhao = Guid.NewGuid(),
                    IdPropriedade = idPropriedade,
                    Nome = "Talhão",
                    Cultura = "Trigo",
                    AreaHectares = 40m,
                    CriadoEm = DateTimeOffset.UtcNow
                }
            }
        };

        await _fixture.Context.Propriedades.AddAsync(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Act
        _fixture.Context.Propriedades.Remove(propriedade);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var talhaoRestante = await _fixture.Context.Talhoes
            .FirstOrDefaultAsync(t => t.IdPropriedade == idPropriedade);

        Assert.Null(talhaoRestante);
    }

    [Fact]
    public async Task DbContext_DeveListarTodasAsPropriedades()
    {
        // Arrange
        _fixture.ResetDatabase();

        var propriedades = new List<AgroSolutions.Propriedade.Dominio.Propriedade>
        {
            new AgroSolutions.Propriedade.Dominio.Propriedade
            {
                IdPropriedade = Guid.NewGuid(),
                IdProdutor = Guid.NewGuid(),
                Nome = "Propriedade 1",
                CriadoEm = DateTimeOffset.UtcNow
            },
            new AgroSolutions.Propriedade.Dominio.Propriedade
            {
                IdPropriedade = Guid.NewGuid(),
                IdProdutor = Guid.NewGuid(),
                Nome = "Propriedade 2",
                CriadoEm = DateTimeOffset.UtcNow
            },
            new AgroSolutions.Propriedade.Dominio.Propriedade
            {
                IdPropriedade = Guid.NewGuid(),
                IdProdutor = Guid.NewGuid(),
                Nome = "Propriedade 3",
                CriadoEm = DateTimeOffset.UtcNow
            }
        };

        foreach (var propriedade in propriedades)
        {
            await _fixture.Context.Propriedades.AddAsync(propriedade);
        }

        await _fixture.Context.SaveChangesAsync();

        // Act
        var propriedadesNoDb = await _fixture.Context.Propriedades.ToListAsync();

        // Assert
        Assert.Equal(3, propriedadesNoDb.Count);
    }

    [Fact]
    public async Task DbContext_DeveListarTodosOsTalhoes()
    {
        // Arrange
        _fixture.ResetDatabase();

        var talhoes = new List<Talhao>
        {
            new Talhao
            {
                IdTalhao = Guid.NewGuid(),
                IdPropriedade = Guid.NewGuid(),
                Nome = "Talhão A",
                Cultura = "Soja",
                AreaHectares = 50m,
                CriadoEm = DateTimeOffset.UtcNow
            },
            new Talhao
            {
                IdTalhao = Guid.NewGuid(),
                IdPropriedade = Guid.NewGuid(),
                Nome = "Talhão B",
                Cultura = "Milho",
                AreaHectares = 75m,
                CriadoEm = DateTimeOffset.UtcNow
            }
        };

        foreach (var talhao in talhoes)
        {
            await _fixture.Context.Talhoes.AddAsync(talhao);
        }

        await _fixture.Context.SaveChangesAsync();

        // Act
        var talhoesNoDb = await _fixture.Context.Talhoes.ToListAsync();

        // Assert
        Assert.Equal(2, talhoesNoDb.Count);
    }

    [Fact]
    public async Task DbContext_DeveAtualizarTalhaoComSucesso()
    {
        // Arrange
        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = Guid.NewGuid(),
            Nome = "Talhão Original",
            Cultura = "Feijão",
            AreaHectares = 30m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        await _fixture.Context.Talhoes.AddAsync(talhao);
        await _fixture.Context.SaveChangesAsync();

        // Act
        talhao.Cultura = "Cana-de-açúcar";
        talhao.AreaHectares = 45m;
        talhao.AtualizadoEm = DateTimeOffset.UtcNow;
        _fixture.Context.Talhoes.Update(talhao);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var talhaoAtualizado = await _fixture.Context.Talhoes
            .FirstOrDefaultAsync(t => t.IdTalhao == talhao.IdTalhao);

        Assert.NotNull(talhaoAtualizado);
        Assert.Equal("Cana-de-açúcar", talhaoAtualizado.Cultura);
        Assert.Equal(45m, talhaoAtualizado.AreaHectares);
    }

    [Fact]
    public async Task DbContext_DeveRemoverTalhaoComSucesso()
    {
        // Arrange
        var talhao = new Talhao
        {
            IdTalhao = Guid.NewGuid(),
            IdPropriedade = Guid.NewGuid(),
            Nome = "Talhão Para Remover",
            Cultura = "Trigo",
            AreaHectares = 25m,
            CriadoEm = DateTimeOffset.UtcNow
        };

        await _fixture.Context.Talhoes.AddAsync(talhao);
        await _fixture.Context.SaveChangesAsync();

        // Act
        _fixture.Context.Talhoes.Remove(talhao);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var talhaoRemovido = await _fixture.Context.Talhoes
            .FirstOrDefaultAsync(t => t.IdTalhao == talhao.IdTalhao);

        Assert.Null(talhaoRemovido);
    }
}
