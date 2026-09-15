namespace RoyalGamess.Aplications.DTOs.UsuarioDto
{
    public class LerUsuarioDto
    {
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int UsuarioId { get; set; }
        public bool StatusUsuario { get; set; }

    }
}
