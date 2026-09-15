using Microsoft.EntityFrameworkCore;
using RoyalGamess.Contexts;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Repositorys
{
    public class GeneroRepository :  IGeneroRepository
    {

        private readonly Royal_GamesssContext _context;

        public GeneroRepository(Royal_GamesssContext context)
        {
            _context = context;
        }

        public List<Genero> Listar()
        {
            return _context.Genero.ToList();
        }

        public Genero ObterPorId(int id)
        {
            //Genero? genero =_context.Genero.Include(genero => genero.)
            return _context.Genero.FirstOrDefault(generoId => generoId.GeneroId == id);

        }

        public Genero ObterPorNome(string nomeGenero)
        {
            return _context.Genero.FirstOrDefault(generoNome => generoNome.Nome == nomeGenero);
        }



        public bool NomeGeneroExiste(string nomeGenero)
        {
            return _context.Genero.Any(nomeG => nomeG.Nome == nomeGenero);
        }

        public void Adicionar(Genero genero)
        {
            _context.Genero.Add(genero);
            _context.SaveChanges();
        }

        public void Atualizar(Genero genero)
        {
            Genero? generoBanco = _context.Genero.FirstOrDefault(generoId => generoId.GeneroId == genero.GeneroId);
            if(generoBanco == null)
            {
                return;
            }

            generoBanco.Nome = genero.Nome;

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Genero? generoBanco = _context.Genero.FirstOrDefault(generoId => generoId.GeneroId == id);

            if(generoBanco == null)
            {
                return;
            }

            _context.Genero.Remove(generoBanco);
            _context.SaveChanges();
        }
    }

}
