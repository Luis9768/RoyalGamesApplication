namespace RoyalGamess.Aplications.DTOs.UsuarioDto
{
    public class CriarUsuarioDto
    {
        public string Email { get; set; } = null!;
        public string Nome { get; set; } = null;
        public string Senha { get; set; } = null;

        public CriarUsuarioDto()
        {
           
        }
    }
}
