using Microsoft.EntityFrameworkCore;
using RoyalGamess.Domains;

namespace RoyalGamess.Interfaces
{
    public interface IUsuarioRepository
    {
        public List<Usuario> Listar();
        Usuario? ObterPorId(int id);
        Usuario? ObterPorEmail(string email);
        public bool NomeExiste(string nome);
        public bool EmailExiste(string email);
        public void Adicionar(Usuario usuario);
        public void Atualizar(Usuario usuario);
        public void Remover(int id);
    }
}


