using AgroSolutions.Usuario.Dominio;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Usuario.Infra;

public class UsuarioDbContext : DbContext
{
    public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options)
        : base(options)
    {
    }

    public DbSet<AgroSolutions.Usuario.Dominio.Usuario> Usuarios => Set<AgroSolutions.Usuario.Dominio.Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entity = modelBuilder.Entity<AgroSolutions.Usuario.Dominio.Usuario>();

        entity.ToTable("usuarios");

        entity.HasKey(u => u.IdUsuario)
              .HasName("pk_usuarios");

        entity.Property(u => u.IdUsuario)
              .HasColumnName("id_usuario")
              .HasColumnType("uuid");

        entity.Property(u => u.Nome)
              .HasColumnName("nome")
              .HasMaxLength(150)
              .IsRequired();

        entity.Property(u => u.Email)
              .HasColumnName("email")
              .HasMaxLength(255)
              .IsRequired();

        entity.Property(u => u.SenhaHash)
              .HasColumnName("senha_hash")
              .HasMaxLength(255)
              .IsRequired();

        entity.Property(u => u.Ativo)
              .HasColumnName("ativo")
              .IsRequired();

        entity.Property(u => u.CriadoEm)
              .HasColumnName("criado_em")
              .HasColumnType("timestamptz")
              .IsRequired();

        entity.Property(u => u.AtualizadoEm)
              .HasColumnName("atualizado_em")
              .HasColumnType("timestamptz");

        entity.HasIndex(u => u.Email)
              .IsUnique()
              .HasDatabaseName("ux_usuarios_email");
    }
}

