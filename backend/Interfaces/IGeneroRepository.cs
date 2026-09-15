using RoyalGamess.Domains;

namespace RoyalGamess.Interfaces
{
    public interface IGeneroRepository
    {

        public List<Genero> Listar();

        Genero ObterPorId(int id);
        Genero ObterPorNome(string nome);
        //public bool NomeGeneroExiste(string? nomeGenero = null, int? generoIdAtual = null);
        public bool NomeGeneroExiste(string nomeGenero);

        public void Adicionar(Genero genero);
        public void Atualizar(Genero genero);
        public void Remover(int id);


    }
}
