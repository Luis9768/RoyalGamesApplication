using RoyalGamess.Aplications.DTOs.UsuarioDto;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Interfaces;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace RoyalGamess.Aplications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        private static LerUsuarioDto LerDto(Usuario usuario)
        {
            LerUsuarioDto lerUsuario = new LerUsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Email = usuario.Email,
                Nome = usuario.Nome,
                StatusUsuario = usuario.StatusUsuario ?? true
            };
            return lerUsuario;
        }
        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> listaUsuario = _repository.Listar();

            List<LerUsuarioDto> usuarioDto = listaUsuario.Select(Usuario => LerDto(Usuario)).ToList();
            return usuarioDto;
        }
        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido!");
            }
        }
        private static void ValidarNome(string Nome)
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new DomainException("Nome inválido!");
            }
        }
        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                throw new DomainException("Senha Obrigatória!");
            }
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }
        public LerUsuarioDto ObterPorId(int id)
        {
            Usuario? usuario = _repository.ObterPorId(id);
            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado!");
            }
            return LerDto(usuario);
        }

        public LerUsuarioDto ObterPorEmail(string email)
        {
            Usuario? usuario = _repository.ObterPorEmail(email);
            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado!");
            }
            return LerDto(usuario);
        }
        public LerUsuarioDto Adicionar(CriarUsuarioDto criarDto)
        {
            ValidarEmail(criarDto.Email);
            ValidarNome(criarDto.Nome);
            if (_repository.EmailExiste(criarDto.Email))
            {
                throw new DomainException("Email já cadastrado!");
            }
            Usuario usuario = new Usuario
            {
                Email = criarDto.Email,
                Nome = criarDto.Nome,
                Senha = HashSenha(criarDto.Senha),
                StatusUsuario = true

            };
            _repository.Adicionar(usuario);
            return LerDto(usuario);
        }
        public LerUsuarioDto Atualizar(int id, CriarUsuarioDto criarDto)
        {
            Usuario usuarioBanco = _repository.ObterPorId(id);
            if (usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado!");
            }
            ValidarEmail(criarDto.Email);
            Usuario usuarioDto = _repository.ObterPorEmail(criarDto.Email);
            if (usuarioDto != null && usuarioDto.UsuarioId != id)
            {
                throw new DomainException("Usuário Inexistente!");
            }
            usuarioBanco.Nome = criarDto.Nome;
            usuarioBanco.Email = criarDto.Email;
            usuarioBanco.Senha = HashSenha(criarDto.Senha);
            _repository.Atualizar(usuarioBanco);
            return LerDto(usuarioBanco);
        }
        public void Remover(int id)
        {
            Usuario usuario = _repository.ObterPorId(id);
            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado!");
            }
            _repository.Remover(id);
        }
    }
}
