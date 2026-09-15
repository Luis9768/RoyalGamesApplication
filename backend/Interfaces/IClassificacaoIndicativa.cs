using RoyalGamess.Domains;

namespace RoyalGamess.Interfaces
{
    public interface IClassificacaoIndicativa
    {
        public List<ClassificacaoIndicativa> Listar();
        ClassificacaoIndicativa ObterPorId(int id);
        public bool ClassificacaoExiste(string nome);
        public void Adicionar(ClassificacaoIndicativa classificacaoI);
        public void Atualizar(ClassificacaoIndicativa classificacaoI);
        public void Remover(int id);
    }
}
