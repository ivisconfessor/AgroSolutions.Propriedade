using Microsoft.EntityFrameworkCore;
using AgroSolutions.Propriedade.Infra;

namespace AgroSolutions.Propriedade.Tests.Fixtures;

/// <summary>
/// Fixture para configurar um banco de dados em memória para testes.
/// </summary>
public class DatabaseFixture : IDisposable
{
    public PropriedadeDbContext Context { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<PropriedadeDbContext>()
            .UseInMemoryDatabase($"AgroSolutions-Test-{Guid.NewGuid()}")
            .Options;

        Context = new PropriedadeDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }

    public void ResetDatabase()
    {
        Context.Propriedades.RemoveRange(Context.Propriedades);
        Context.Talhoes.RemoveRange(Context.Talhoes);
        Context.SaveChanges();
    }
}
