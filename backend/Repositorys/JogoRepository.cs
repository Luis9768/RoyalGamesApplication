using Microsoft.EntityFrameworkCore;
using RoyalGamess.Contexts;
using RoyalGamess.Domains;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Repositorys
{
    public class JogoRepository : IJogoRepository
    {
        private readonly Royal_GamesssContext _context;
        public JogoRepository(Royal_GamesssContext context)
        {
            _context = context;
        }

        public List<Jogo> Listar()
        {
            List<Jogo> jogos = _context.Jogo
                .Include(jogo => jogo.ClassificaçãoIdFKNavigation)
                .Include(jogo => jogo.UsuarioIdFKNavigation)
                .Include(jogo => jogo.GeneroIdFK)
                .Include(jogo => jogo.PlataformaIdFK)
                .ToList();

            return jogos;
        }
        public Jogo ObterPorId(int id)
        {
            Jogo? jogoId = _context.Jogo
                .Include(jogo => jogo.ClassificaçãoIdFKNavigation)
                .Include(jogo => jogo.UsuarioIdFKNavigation)
                .Include(jogo => jogo.GeneroIdFK)
                .Include(jogo => jogo.PlataformaIdFK)
                .FirstOrDefault(jogo => jogo.JogoId == id);

            return jogoId;

        }

        public Jogo? ObterPorNome(string nome)
        {
            // return _context.Jogo.FirstOrDefault(jogoNome => jogoNome.Nome == nome);

            Jogo? jogoNome = _context.Jogo
                .Include(jogo => jogo.ClassificaçãoIdFKNavigation)
                .Include(jogo => jogo.UsuarioIdFKNavigation)
                .Include(jogo => jogo.GeneroIdFK)
                .Include(jogo => jogo.PlataformaIdFK)
                .FirstOrDefault(jogoNome => jogoNome.Nome == nome);

            return jogoNome;


        }

        public byte[] ObterImagem(int id)
        {
            var jogo = _context.Jogo.Where(jogo => jogo.JogoId == id)
            .Select(jogo => jogo.Imagem)
            .FirstOrDefault();
            return jogo;
        }

        public bool NomeJogoExiste(string nomeJogo, int? jogoId)
        {
            var jogo = _context.Jogo.AsQueryable();

            if (jogoId.HasValue)
                jogo = jogo.Where(jogo => jogo.JogoId != jogoId.Value);

            return jogo.Any(jogo => jogo.Nome == nomeJogo);
        }

        public void Adicionar(Jogo jogo, List<int> generoIds, List<int> plataformaIds)//, int classificacaoId)
        {
            List<Genero> generos = _context.Genero.Where(generoAux => generoIds.Contains(generoAux.GeneroId)).ToList();
            List<Plataforma> plataformas = _context.Plataforma.Where(plataformaAux => plataformaIds.Contains(plataformaAux.PlataformaId)).ToList();
            //ClassificacaoIndicativa? classificacaoId2 = _context.ClassificacaoIndicativa.FirstOrDefault(clasIndi => clasIndi.ClassificacaoIndicativaId == classificacaoId);

            jogo.GeneroIdFK = generos;
            jogo.PlataformaIdFK = plataformas;
            //jogo.ClassificaçãoIdFKNavigation = classificacaoId2;

            _context.Jogo.Add(jogo);
            _context.SaveChanges();
        }

        public void Atualizar(Jogo jogo, List<int> generoIds, List<int> plataformaIds)//, int classificacaoId)
        {
            Jogo? jogoAtualizar = _context.Jogo
               .Include(j => j.GeneroIdFK)
               .Include(j => j.PlataformaIdFK)
               .FirstOrDefault(j => j.JogoId == jogo.JogoId);

            if (jogoAtualizar == null)
            {
                return;
            }

            jogoAtualizar.Nome = jogo.Nome;
            jogoAtualizar.Preco = jogo.Preco;
            jogoAtualizar.Descricao = jogo.Descricao;
            //jogoAtualizar.ClassificaçãoIdFK = classificacaoId;

            if (jogo.Imagem != null && jogo.Imagem.Length > 0)
                jogoAtualizar.Imagem = jogo.Imagem;

            if (jogo.StatusJogo.HasValue)
                jogoAtualizar.StatusJogo = jogo.StatusJogo;

            var jogosGenero = _context.Genero.Where(genero => generoIds.Contains(genero.GeneroId)).ToList();
            jogoAtualizar.GeneroIdFK.Clear();
            foreach (var generosVar in jogosGenero)
            {
                jogoAtualizar.GeneroIdFK.Add(generosVar);
            }


            var jogosPlataforma = _context.Plataforma.Where(classificacao => plataformaIds.Contains(classificacao.PlataformaId)).ToList();
            jogoAtualizar.PlataformaIdFK.Clear();
            foreach (var plataformaVar in jogosPlataforma)
            {
                jogoAtualizar.PlataformaIdFK.Add(plataformaVar);
            }

            

            _context.SaveChanges();


        }

        public void Remover(int id)
        {
            Jogo? jogoBanco = _context.Jogo.FirstOrDefault(jogoI => jogoI.JogoId == id);
            if (jogoBanco == null)
                return;

            _context.Jogo.Remove(jogoBanco);
            _context.SaveChanges();
        }


    }
}
