using RoyalGamess.Aplications.DTOs.LogAlteracaoJogoDto;
using RoyalGamess.Aplications.DTOs.PlataformaDto;
using RoyalGamess.Domains;
using RoyalGamess.Interfaces;
using RoyalGamess.Repositorys;

namespace RoyalGamess.Aplications.Services
{
    public class LogAlteracaoJogoService 
    {
        private readonly ILogAlteracaoJogoRepository _repository;
        public LogAlteracaoJogoService(ILogAlteracaoJogoRepository repository)
        {
            _repository = repository;
        }
        public List<LerLogAlteracaoJogoDto> Listar()
        {
            List<Log_Alteracao_Jogo> logs = _repository.Listar();
            List<LerLogAlteracaoJogoDto> listaProdutos = logs.Select(log => new LerLogAlteracaoJogoDto
            {
                LogId = log.Log_Alteracao_Jogo_Id,
                PrecoAnterior = log.PrecoAnterior,
                NomeAnterior = log.NomeAnterior,
                DataAlteracao = log.DataAlteracao,
                JogoId = log.JogoId,
            }).ToList();
            return listaProdutos;
        }
        public List<LerLogAlteracaoJogoDto> ListarPorProduto(int id)
        {
            List<Log_Alteracao_Jogo> logs = _repository.ListarPorProduto(id);
            List<LerLogAlteracaoJogoDto> listaProdutos = logs.Select(log => new LerLogAlteracaoJogoDto
            {
                LogId = log.Log_Alteracao_Jogo_Id,
                DataAlteracao = log.DataAlteracao,
                JogoId = log.JogoId,
                NomeAnterior = log.NomeAnterior,
                PrecoAnterior = log.PrecoAnterior
            }).ToList();
            return listaProdutos;
        }
    }
}
