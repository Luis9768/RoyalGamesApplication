using RoyalGamess.Contexts;
using RoyalGamess.Domains;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Repositorys
{
    public class PromocaoRepository : IPromocaoRepository
    {
        private readonly Royal_GamesssContext _context;
        public PromocaoRepository(Royal_GamesssContext context)
        {
            _context = context;
        }
        public List<Promocao> Listar()
        {
            return _context.Promocao.ToList();
        }
        public Promocao ObterPorId(int id)
        {
            Promocao? promocao = _context.Promocao.FirstOrDefault(p => p.PromocaoId == id);
            return promocao;
        }
        public bool NomeExiste(string nome, int? promocaoAtual = null)
        {
            var consulta = _context.Promocao.AsQueryable();
            if (promocaoAtual.HasValue)
            {
                consulta = consulta.Where(p => p.PromocaoId != promocaoAtual.Value);
            }
            return consulta.Any(p => p.Nome == nome);
        }
        public void Adicionar(Promocao promocao)
        {
            _context.Promocao.Add(promocao);
            _context.SaveChanges();
        }
        public void Atualizar(int id, Promocao promocao)
        {
            Promocao? promocaoBanco = _context.Promocao.FirstOrDefault(p => p.PromocaoId == promocao.PromocaoId);
            if (promocaoBanco == null)
            {
                return;
            }
            promocaoBanco.Nome = promocao.Nome;
            promocaoBanco.DataExpiração = promocao.DataExpiração;
            promocaoBanco.StatusPromocao = promocao.StatusPromocao;
            _context.SaveChanges();
        }
        public void Remover(int id)
        {
            Promocao? promocao = _context.Promocao.FirstOrDefault(p => p.PromocaoId == id);
            if (promocao == null)
            {
                return;
            }
            _context.Promocao.Remove(promocao);
            _context.SaveChanges();
        }
    }

}
