using RoyalGamess.Domains;

namespace RoyalGamess.Interfaces
{
    public interface ILogAlteracaoJogoRepository
    {
        List<Log_Alteracao_Jogo> Listar();
        List<Log_Alteracao_Jogo> ListarPorProduto(int id);
    }
}
