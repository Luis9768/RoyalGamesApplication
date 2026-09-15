using RoyalGamess.Aplications.Autenticacao;
using RoyalGamess.Aplications.DTOs.AutenticacaoDto;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Aplications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJWT _tokenJWT;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJWT tokenJWT)
        {
            _repository = repository;
            _tokenJWT = tokenJWT;
        }
        private static bool VerificarSenha(string senhaDigtada, byte[] senhaHashBanco)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigtada));
            return hashDigitado.SequenceEqual(senhaHashBanco);
        }
        public TokenDto Login(LoginDto loginDto)
        {
            Usuario? usuario = _repository.ObterPorEmail(loginDto.Email);
            if (usuario == null)
            {
                throw new DomainException("Email ou senha inválidos!");
            }

            if(usuario.StatusUsuario == false)
            {
                throw new DomainException("Email ou senha inválidos");
            }
            if (VerificarSenha(loginDto.Senha, usuario.Senha) == false)
            {
                throw new DomainException("Email ou senha inválidos!");
            }

            //gerar token
            var token = _tokenJWT.GerarToken(usuario);

            TokenDto novoToken = new TokenDto { Token = token };
            return novoToken;
        }

    }
}
