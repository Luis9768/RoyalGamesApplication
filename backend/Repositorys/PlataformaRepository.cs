using RoyalGamess.Contexts;
using RoyalGamess.Domains;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Repositorys
{
    public class PlataformaRepository : IPlataformaRepository
    {
        private readonly Royal_GamesssContext _context;
        public PlataformaRepository(Royal_GamesssContext context)
        {
            _context = context;
        }
        public List<Plataforma> Listar()
        {
            return _context.Plataforma.ToList();
        }
        public Plataforma? ObterPorId(int id)
        {
            Plataforma? plataforma = _context.Plataforma.Find(id);
            return plataforma;
        }
        public Plataforma? ObterPorNome(string nome)
        {
            return _context.Plataforma.FirstOrDefault(n => n.Nome == nome);
        }
        public bool NomeExiste(string nome, int? plataformaAtual = null)
        {
            var consulta = _context.Plataforma.AsQueryable();
            if (plataformaAtual.HasValue)
            {
                consulta = consulta.Where(p => p.PlataformaId != plataformaAtual.Value);
            }
            return consulta.Any(p => p.Nome == nome);
        }
        public void Adicionar(Plataforma plataforma)
        {
            _context.Plataforma.Add(plataforma);
            _context.SaveChanges();
        }
        public void Atualizar(int id, Plataforma plataforma)
        {
            Plataforma? plataforma1 = _context.Plataforma.FirstOrDefault(P => P.PlataformaId == id);
            if (plataforma1 == null)
            {
                return;
            }
            _context.Plataforma.Update(plataforma);
            _context.SaveChanges();
        }
        public void Remover(int id)
        {
            Plataforma? plataforma = _context.Plataforma.Find(id);
            if (plataforma == null)
            {
                return;
            }
            _context.Plataforma.Remove(plataforma);
            _context.SaveChanges();
        }
    }
}
