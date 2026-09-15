using RoyalGamess.Domains;

namespace RoyalGamess.Interfaces
{
    public interface IJogoRepository
    {

        List<Jogo> Listar();
        Jogo ObterPorId(int id);
        Jogo ObterPorNome(string nome);

        byte[] ObterImagem(int id);
         bool NomeJogoExiste(string nomeJogo, int? jogoId = null);

        void Adicionar(Jogo jogo, List<int> generoIds, List<int> plataformaIds);    //, int classificacaoId);
          void Atualizar(Jogo jogo, List<int> generoIds, List<int> plataformaIds);  //, int classificacaoId);
          void Remover(int id);

    }
}
