using RoyalGamess.Domains;
using System.Security.Cryptography;

namespace RoyalGamess.Interfaces
{
    public interface IPromocaoRepository
    {
        List<Promocao> Listar();
        Promocao? ObterPorId(int id);
        bool NomeExiste(string nome, int? promocaoAtual = null);
        void Adicionar(Promocao promocao);
        void Atualizar(int id, Promocao promocao);
        void Remover(int id);
    }
}
