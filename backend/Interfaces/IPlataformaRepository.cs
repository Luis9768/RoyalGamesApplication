using RoyalGamess.Domains;

namespace RoyalGamess.Interfaces
{
    public interface IPlataformaRepository
    {
        public List<Plataforma> Listar();
        Plataforma? ObterPorId(int id);
        Plataforma? ObterPorNome(string nome);
        bool NomeExiste(string nome, int? plataformaAtual = null);
        public void Adicionar(Plataforma plataforma); 
        public void Atualizar(int id, Plataforma plataforma);
        public void Remover(int id);
    }
}
