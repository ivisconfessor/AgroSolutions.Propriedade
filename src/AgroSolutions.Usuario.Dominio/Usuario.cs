namespace AgroSolutions.Usuario.Dominio;

public class Usuario
{
    public Guid IdUsuario { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public bool Ativo { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset? AtualizadoEm { get; set; }
}

