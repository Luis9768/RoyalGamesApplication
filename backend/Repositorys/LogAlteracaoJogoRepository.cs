using RoyalGamess.Contexts;
using RoyalGamess.Domains;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Repositorys
{
    public class LogAlteracaoJogoRepository : ILogAlteracaoJogoRepository
    {
        private readonly Royal_GamesssContext _context;
        public LogAlteracaoJogoRepository(Royal_GamesssContext context)
        {
            _context = context;
        }
        public List<Log_Alteracao_Jogo> Listar()
        {
            List<Log_Alteracao_Jogo> log = _context.Log_Alteracao_Jogo.OrderByDescending(l => l.DataAlteracao).ToList();
            return log;
        }
        public List<Log_Alteracao_Jogo> ListarPorProduto(int id)
        {
            List<Log_Alteracao_Jogo> AlteracaoProduto = _context.Log_Alteracao_Jogo.Where(log => log.JogoId == id).OrderByDescending(log => log.DataAlteracao).ToList();
            return AlteracaoProduto;
        }
    }
}
