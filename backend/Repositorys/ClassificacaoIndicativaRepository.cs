using Microsoft.EntityFrameworkCore;
using RoyalGamess.Contexts;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Repositorys
{
    public class ClassificacaoIndicativaRepository :  IClassificacaoIndicativa
    {

        private readonly Royal_GamesssContext _context;

        public ClassificacaoIndicativaRepository(Royal_GamesssContext context)
        {
            _context = context;
        }

        public List<ClassificacaoIndicativa> Listar()
        {
            return _context.ClassificacaoIndicativa.ToList();
        }

        public ClassificacaoIndicativa ObterPorId(int id)
        {
            return _context.ClassificacaoIndicativa.FirstOrDefault(classificacaoId => classificacaoId.ClassificacaoIndicativaId == id);
        }

        public bool ClassificacaoExiste(string nome)
        {
            return _context.ClassificacaoIndicativa.Any(classificacao => classificacao.Classificacao == nome);
        }

        public void Adicionar(ClassificacaoIndicativa classificacao)
        {
            _context.ClassificacaoIndicativa.Add(classificacao);
            _context.SaveChanges();
        }

        public void Atualizar(ClassificacaoIndicativa classificacao)
        {
            ClassificacaoIndicativa? classificacaoBanco = _context.ClassificacaoIndicativa.FirstOrDefault(classificacaoId =>
            classificacaoId.ClassificacaoIndicativaId == classificacao.ClassificacaoIndicativaId);
            if(classificacaoBanco == null)
            {
                return;
            }

            classificacaoBanco.Classificacao = classificacao.Classificacao;
            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            ClassificacaoIndicativa classificacaoBanco = _context.ClassificacaoIndicativa.FirstOrDefault(classificacaoId => classificacaoId.ClassificacaoIndicativaId == id);

            if(classificacaoBanco == null)
            {
                return;
            }

            _context.ClassificacaoIndicativa.Remove(classificacaoBanco);
            _context.SaveChanges();
        }
    }

}
